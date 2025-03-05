using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using apos_gestor_caja.applicationLayer.interfaces;
using apos_gestor_caja.Domain.Models;
using MyApp.Infrastructure.Database;
using System.Linq;

namespace apos_gestor_caja.service
{
    public class UploadVentas : IArchivoService
    {
        private readonly string directorio = @"\\192.168.1.5\sambaconserca-2";
        private readonly SqlHelper _sqlHelper;

        // Cache structure: key is "startDate_endDate_caja", value is the list of matching files
        private static readonly ConcurrentDictionary<string, Tuple<List<string>, DateTime>> _fileCache
            = new ConcurrentDictionary<string, Tuple<List<string>, DateTime>>();

        // Cache expiration time in minutes
        private const int CACHE_EXPIRATION_MINUTES = 15;

        public UploadVentas()
        {
            _sqlHelper = new SqlHelper();
        }

        public async Task<List<string>> ObtenerArchivosVentasAsync(DateTime fechaInicio, DateTime fechaFin, string caja = null)
        {
            if (fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha de fin.");

            if (!string.IsNullOrWhiteSpace(caja))
            {
                if (!int.TryParse(caja, out _))
                    throw new ArgumentException("El número de caja debe ser un valor numérico válido.");
                caja = caja.PadLeft(2, '0');
            }

            // Create a cache key based on the query parameters
            string cacheKey = $"{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}_{caja ?? "all"}";

            try
            {
                // Check if we have a valid cache entry
                if (_fileCache.TryGetValue(cacheKey, out var cachedResult))
                {
                    // Check if the cache is still valid (not expired)
                    if (DateTime.Now.Subtract(cachedResult.Item2).TotalMinutes < CACHE_EXPIRATION_MINUTES)
                    {
                        return cachedResult.Item1;
                    }

                    // Cache expired, remove it
                    _fileCache.TryRemove(cacheKey, out _);
                }

                // Use a regex pattern to match only the files we need
                // This regex matches filename pattern: [caja].[date].apos08.iep
                var filePattern = new Regex(@"^(\d{2})\.(\d{6})\.apos08\.iep$", RegexOptions.Compiled);

                // Get all files first
                string[] allFiles = await Task.Run(() => Directory.GetFiles(directorio, "*.iep", SearchOption.TopDirectoryOnly));

                // Process files in parallel
                var archivosEnRangoFecha = new ConcurrentBag<string>();

                await Task.Run(() => {
                    Parallel.ForEach(allFiles, archivo => {
                        string fileName = Path.GetFileName(archivo);
                        var match = filePattern.Match(fileName);

                        if (!match.Success)
                            return;

                        string cajaArchivo = match.Groups[1].Value;
                        string fechaStr = match.Groups[2].Value;

                        // Filter by caja if specified
                        if (!string.IsNullOrWhiteSpace(caja) && cajaArchivo != caja)
                            return;

                        // Parse date more efficiently
                        if (fechaStr.Length == 6 &&
                            int.TryParse(fechaStr.Substring(0, 2), out int ano) &&
                            int.TryParse(fechaStr.Substring(2, 2), out int mes) &&
                            int.TryParse(fechaStr.Substring(4, 2), out int dia))
                        {
                            int anoCompleto = ano < 50 ? 2000 + ano : 1900 + ano;

                            // Validate date components before creating DateTime
                            if (mes < 1 || mes > 12 || dia < 1 || dia > 31)
                                return;

                            DateTime fechaArchivo;
                            try
                            {
                                fechaArchivo = new DateTime(anoCompleto, mes, dia);
                            }
                            catch
                            {
                                return; // Invalid date
                            }

                            if (fechaArchivo >= fechaInicio && fechaArchivo <= fechaFin)
                            {
                                archivosEnRangoFecha.Add(archivo);
                            }
                        }
                    });
                });

                // Convert to list and cache the result with current timestamp
                var resultList = archivosEnRangoFecha.ToList();
                _fileCache[cacheKey] = new Tuple<List<string>, DateTime>(resultList, DateTime.Now);

                return resultList;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new Exception($"No tienes permiso para acceder al directorio: {ex.Message}", ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception($"El directorio no se encontró: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los archivos de ventas: {ex.Message}", ex);
            }
        }

        public async Task<bool> SubirArchivosVentasAsync(DateTime fechaInicio, DateTime fechaFin, string caja = null)
        {
            try
            {
                var archivos = await ObtenerArchivosVentasAsync(fechaInicio, fechaFin, caja);
                if (archivos.Count == 0)
                    return false;

                MySqlConnection connection = null;
                try
                {
                    connection = _sqlHelper.ObtenerConexion();

                    // Process files in batches
                    int batchSize = 100;

                    foreach (string archivo in archivos)
                    {
                        List<UploadVenta> ventasBatch = new List<UploadVenta>();

                        using (var reader = new StreamReader(archivo))
                        {
                            string linea;
                            while ((linea = await reader.ReadLineAsync()) != null)
                            {
                                UploadVenta venta = ParsearLinea(linea);
                                ventasBatch.Add(venta);

                                // Process in batches to reduce database roundtrips
                                if (ventasBatch.Count >= batchSize)
                                {
                                    await InsertarVentasEnLotesAsync(connection, ventasBatch);
                                    ventasBatch.Clear();
                                }
                            }

                            // Insert remaining items
                            if (ventasBatch.Count > 0)
                            {
                                await InsertarVentasEnLotesAsync(connection, ventasBatch);
                            }
                        }
                    }

                    return true;
                }
                finally
                {
                    if (connection != null)
                        _sqlHelper.CerrarConexion(connection);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al subir los archivos de ventas a SQL: {ex.Message}", ex);
            }
        }

        // Method to manually clear the cache if needed
        public static void LimpiarCache()
        {
            _fileCache.Clear();
        }

        // Method to clear expired cache entries
        public static void LimpiarCacheExpirado()
        {
            var keysToRemove = _fileCache
                .Where(x => DateTime.Now.Subtract(x.Value.Item2).TotalMinutes >= CACHE_EXPIRATION_MINUTES)
                .Select(x => x.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                _fileCache.TryRemove(key, out _);
            }
        }

        private async Task InsertarVentasEnLotesAsync(MySqlConnection connection, List<UploadVenta> ventas)
        {
            // Use transaction for batch operations
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using (var command = new MySqlCommand())
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;

                        // Create bulk insert command
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.Append(@"
                            INSERT INTO apos08 (d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15, d16, d17, d18, d19, d20, d21, d22, d23, d24, d25, d26, d27, d28, d29, d30, d31, d32, d33)
                            VALUES ");

                        for (int i = 0; i < ventas.Count; i++)
                        {
                            queryBuilder.Append($"(@d1_{i}, @d2_{i}, @d3_{i}, @d4_{i}, @d5_{i}, @d6_{i}, @d7_{i}, @d8_{i}, @d9_{i}, @d10_{i}, " +
                                              $"@d11_{i}, @d12_{i}, @d13_{i}, @d14_{i}, @d15_{i}, @d16_{i}, @d17_{i}, @d18_{i}, @d19_{i}, @d20_{i}, " +
                                              $"@d21_{i}, @d22_{i}, @d23_{i}, @d24_{i}, @d25_{i}, @d26_{i}, @d27_{i}, @d28_{i}, @d29_{i}, @d30_{i}, " +
                                              $"@d31_{i}, @d32_{i}, @d33_{i})");

                            if (i < ventas.Count - 1)
                                queryBuilder.Append(",");
                        }

                        queryBuilder.Append(@"
                            ON DUPLICATE KEY UPDATE
                                d1 = VALUES(d1), d2 = VALUES(d2), d3 = VALUES(d3), d4 = VALUES(d4), d5 = VALUES(d5),
                                d6 = VALUES(d6), d7 = VALUES(d7), d8 = VALUES(d8), d9 = VALUES(d9), d10 = VALUES(d10),
                                d11 = VALUES(d11), d12 = VALUES(d12), d13 = VALUES(d13), d14 = VALUES(d14), d15 = VALUES(d15),
                                d16 = VALUES(d16), d17 = VALUES(d17), d18 = VALUES(d18), d19 = VALUES(d19), d20 = VALUES(d20),
                                d21 = VALUES(d21), d22 = VALUES(d22), d23 = VALUES(d23), d24 = VALUES(d24), d25 = VALUES(d25),
                                d26 = VALUES(d26), d27 = VALUES(d27), d28 = VALUES(d28), d29 = VALUES(d29), d30 = VALUES(d30),
                                d31 = VALUES(d31), d32 = VALUES(d32), d33 = VALUES(d33)");

                        command.CommandText = queryBuilder.ToString();

                        // Add parameters for each batch item
                        for (int i = 0; i < ventas.Count; i++)
                        {
                            var venta = ventas[i];
                            command.Parameters.AddWithValue($"@d1_{i}", venta.D1);
                            command.Parameters.AddWithValue($"@d2_{i}", venta.D2);
                            command.Parameters.AddWithValue($"@d3_{i}", venta.D3);
                            command.Parameters.AddWithValue($"@d4_{i}", venta.D4);
                            command.Parameters.AddWithValue($"@d5_{i}", venta.D5);
                            command.Parameters.AddWithValue($"@d6_{i}", venta.D6);
                            command.Parameters.AddWithValue($"@d7_{i}", venta.D7);
                            command.Parameters.AddWithValue($"@d8_{i}", venta.D8);
                            command.Parameters.AddWithValue($"@d9_{i}", venta.D9);
                            command.Parameters.AddWithValue($"@d10_{i}", venta.D10);
                            command.Parameters.AddWithValue($"@d11_{i}", venta.D11);
                            command.Parameters.AddWithValue($"@d12_{i}", venta.D12);
                            command.Parameters.AddWithValue($"@d13_{i}", venta.D13);
                            command.Parameters.AddWithValue($"@d14_{i}", venta.D14);
                            command.Parameters.AddWithValue($"@d15_{i}", venta.D15);
                            command.Parameters.AddWithValue($"@d16_{i}", venta.D16);
                            command.Parameters.AddWithValue($"@d17_{i}", venta.D17);
                            command.Parameters.AddWithValue($"@d18_{i}", venta.D18);
                            command.Parameters.AddWithValue($"@d19_{i}", venta.D19);
                            command.Parameters.AddWithValue($"@d20_{i}", venta.D20);
                            command.Parameters.AddWithValue($"@d21_{i}", venta.D21);
                            command.Parameters.AddWithValue($"@d22_{i}", venta.D22);
                            command.Parameters.AddWithValue($"@d23_{i}", venta.D23);
                            command.Parameters.AddWithValue($"@d24_{i}", venta.D24);
                            command.Parameters.AddWithValue($"@d25_{i}", venta.D25);
                            command.Parameters.AddWithValue($"@d26_{i}", venta.D26);
                            command.Parameters.AddWithValue($"@d27_{i}", venta.D27);
                            command.Parameters.AddWithValue($"@d28_{i}", venta.D28);
                            command.Parameters.AddWithValue($"@d29_{i}", venta.D29);
                            command.Parameters.AddWithValue($"@d30_{i}", venta.D30);
                            command.Parameters.AddWithValue($"@d31_{i}", venta.D31);
                            command.Parameters.AddWithValue($"@d32_{i}", venta.D32);
                            command.Parameters.AddWithValue($"@d33_{i}", venta.D33);
                        }

                        await command.ExecuteNonQueryAsync();
                    }

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private UploadVenta ParsearLinea(string linea)
        {
            // Dividir la línea por '|' y manejar el campo vacío extra
            string[] campos = linea.Split('|');
            if (campos.Length != 34) // Son 34 campos debido al || extra
                throw new Exception($"Formato de línea inválido: {linea}. Se esperaban 34 campos, se encontraron {campos.Length}");

            // Combinar el penúltimo campo vacío con el anterior (d31 = "00000000")
            campos[30] = campos[30] + campos[31]; // "00000000" + "" = "00000000"
            Array.Resize(ref campos, 33); // Reducir a 33 campos eliminando el penúltimo vacío

            return new UploadVenta
            {
                D1 = campos[0],
                D2 = campos[1],
                D3 = campos[2],
                D4 = campos[3],
                D5 = campos[4],
                D6 = campos[5],
                D7 = campos[6],
                D8 = campos[7],
                D9 = campos[8],
                D10 = campos[9],
                D11 = campos[10],
                D12 = campos[11],
                D13 = campos[12],
                D14 = campos[13],
                D15 = campos[14],
                D16 = campos[15],
                D17 = campos[16],
                D18 = campos[17],
                D19 = campos[18],
                D20 = campos[19],
                D21 = campos[20],
                D22 = campos[21],
                D23 = campos[22],
                D24 = campos[23],
                D25 = campos[24],
                D26 = campos[25],
                D27 = campos[26],
                D28 = campos[27],
                D29 = campos[28],
                D30 = campos[29],
                D31 = campos[30], // "00000000" combinado quitan el antepenultimo
                D32 = campos[31],
                D33 = campos[32]
            };
        }
    }
}