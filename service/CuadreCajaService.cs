using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using apos_gestor_caja.applicationLayer.interfaces;
using apos_gestor_caja.ApplicationLayer.Services;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.Infrastructure.Repositories;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.SqlServer.Server;


namespace apos_gestor_caja.service
{
    public class CuadreCajaService : ICierreCaja
    {
        private readonly string directorio = @"\\192.168.1.5\sambaconserca-2";
        //private readonly string directorio = @"C:\Users\Cesar Morales\OneDrive\Escritorio\facturas";
        private static readonly ConcurrentDictionary<string, CacheItem> cache = new ConcurrentDictionary<string, CacheItem>();
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);
        //usar en el global para cargarlos al iniciar la app
        private readonly CajeroService _cajeroService = new CajeroService();
        private readonly TipoDePagoRepository _pagoService = new TipoDePagoRepository();
        private readonly BancoRepository _bancoService = new BancoRepository();

        private class CacheItem
        {
            public List<string> Files { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public class informacionCuadreCaja
        {
            public string Abreviacion { get; set; }
            public string TipoPago { get; set; }
            public double CantidadSistema { get; set; }
            public double ConfirmacionSistema { get; set; }
            public double MontoRecibido { get; set; } = 0;
            public double BaucheRecibido { get; set; } = 0;

        }

        public CuadreCajaService()
        {

        }

        public async Task<List<informacionCuadreCaja>> GetSummaryLines(List<CierreCaja> elementosAProcesar, int idCajero)
        {
            Console.WriteLine($"llega");
            return await LineasAMostrar(elementosAProcesar, idCajero);
        }

        //funcion para obtener todos los archivos del apos13
        public async Task<List<string>> ObtenerArchivosApos13(int idCajero, DateTime fechaDelCierre, int nroCaja)
        {
            //TODO:crear cache ya que el mismo archivo se busca 3 veces al dia tener en cuenta que su contenido puede cambiar aunque la funcion obtiene su direccion
            // Validate input
            if (idCajero < 1)
                throw new ArgumentException("El ID de cajero debe ser un número positivo.", nameof(idCajero));

            //validando cajas recordar que la 0 es la interna de administracion
            if (nroCaja < 0)
                throw new ArgumentException("Los numeros de caja van desde el 0 hacia adelante", nameof(nroCaja));

            // Pad the cajero number with leading zero if needed
            string cajaFormatted = nroCaja.ToString().PadLeft(2, '0');

            // Format date as required: YY + MM + DD
            string fechaFormatted = fechaDelCierre.ToString("yy") +
                                    fechaDelCierre.ToString("MM") +
                                    fechaDelCierre.ToString("dd");

            // regex para obtener la informacion
            // Format: [caja].[fecha].apos13.iep
            //var filePattern = new Regex($@"^{cajaFormatted}\.{fechaFormatted}\.apos13\.iep$", RegexOptions.Compiled);

            // Create a cache key
            string cacheKey = $"{cajaFormatted}.{fechaFormatted}";

            // Check if the result is already in the cache and not expired
            if (cache.TryGetValue(cacheKey, out var cachedItem) && (DateTime.Now - cachedItem.Timestamp) < CacheDuration)
            {
                return cachedItem.Files;
            }

            try
            {
                // Obtener solo los archivos que coinciden con la fecha de hoy
                Console.WriteLine($"{cajaFormatted}.{fechaFormatted}.apos13.iep");
                string[] allFiles = await Task.Run(() =>
                    Directory.GetFiles(directorio, $"{cajaFormatted}.{fechaFormatted}*.apos13.iep", SearchOption.TopDirectoryOnly));

                var matchingFiles = allFiles.ToList();

                // Store the result in the cache
                cache[cacheKey] = new CacheItem
                {
                    Files = matchingFiles,
                    Timestamp = DateTime.Now
                };

                return matchingFiles;
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
                throw new Exception($"Error al obtener los archivos APOS13: {ex.Message}", ex);
            }
        }

        public async Task<List<string>> ProcesarArchivoCierreCajaAsync(string filePath, int idCajero)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"El archivo {filePath} no existe.");

            List<CierreCaja> cierres = new List<CierreCaja>();

            try
            {
                // Leer el archivo de forma asíncrona usando FileStream
                string[] lineas;
                using (var reader = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true)))
                {
                    var content = await reader.ReadToEndAsync();
                    lineas = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                }

                if (lineas.Length == 0)
                    throw new Exception("El archivo está vacío.");

                //Agregar las lineas cuyo cajero sea el buscado
                string[] ventasCajero = lineas.Where(venta => venta.Split('|')[2] == idCajero.ToString()).ToArray();

                return ventasCajero.ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar el archivo de cierre de caja: {ex.Message}");
                throw;
            }
        }

        public async Task<List<CierreCaja>> ObtenerCierreCaja(int idCajero, DateTime fechaDelCierre, int nroCaja)
        {
            var archivos = await ObtenerArchivosApos13(idCajero, fechaDelCierre, nroCaja);
            var cierres = new ConcurrentBag<CierreCaja>();

            await Task.WhenAll(archivos.Select(async archivo =>
            {
                var cierreList = await ProcesarArchivoCierreCajaAsync(archivo, idCajero);
                foreach (var cierre in cierreList.Select(ConvertirLineaACierreCaja).Where(c => c != null))
                {
                    cierres.Add(cierre);
                }
            }));

            

            return cierres.ToList();
        }

        public async Task<List<informacionCuadreCaja>> LineasAMostrar(List<CierreCaja> ElementosAProcesar, int idCajero)
        {
            List<Cajero> cajeros;
            List<Banco> bancos;
            List<TipoDePagoModel> pagos;
            try
            {
                cajeros = await _cajeroService.ObtenerCajerosAsync();
                bancos = await _bancoService.ObtenerBancosAsync();
                pagos = await _pagoService.ObtenerTodosLosTiposDePago();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            if (cajeros.Find(cajero => cajero.Id == idCajero) == null)
                throw new Exception("El cajero no existe");

            // Filtramos primero los elementos del cajero
            List<CierreCaja> cierreCajaLines = ElementosAProcesar
                .Where(e => e.Cajero == idCajero)
                .ToList();

            Console.WriteLine($"Cierres encontrados: {cierreCajaLines.Count}");

            // Usamos una colección thread-safe para agregar elementos desde múltiples hilos
            var lineasAMostrarConcurrent = new System.Collections.Concurrent.ConcurrentBag<informacionCuadreCaja>();

            // Procesamiento paralelo de las líneas
            Parallel.ForEach(cierreCajaLines, cierreCaja =>
            {
                if (cierreCaja.Credito == 2)
                {
                    lineasAMostrarConcurrent.Add(new informacionCuadreCaja
                    {
                        Abreviacion = "CR",
                        TipoPago = "Credito",
                        CantidadSistema = cierreCaja.Monto,
                        ConfirmacionSistema = cierreCaja.cantidad,
                    });
                    return; // equivalent to continue in a regular loop
                }

                if (cierreCaja.Emisor == 75)
                {
                    Console.WriteLine($"Prueba monto en dolares {cierreCaja.Monto}, cantidad {cierreCaja.cantidad}");
                    lineasAMostrarConcurrent.Add(new informacionCuadreCaja
                    {
                        Abreviacion = "USD",
                        TipoPago = "Dolares",
                        CantidadSistema = cierreCaja.cantidad,
                        ConfirmacionSistema = cierreCaja.Monto,
                    });
                    return;
                }

                if (cierreCaja.FormaPago == 77)
                {
                    lineasAMostrarConcurrent.Add(new informacionCuadreCaja
                    {
                        Abreviacion = "PP",
                        TipoPago = "Paypal",
                        // cuando es moneda extranjera el monto es la tasa de cambio
                        CantidadSistema = cierreCaja.Monto * cierreCaja.cantidad,
                        ConfirmacionSistema = 1.00,
                    });
                    return;
                }

                string abreviacionEncontrada = pagos.Find(pago => pago.Id == cierreCaja.FormaPago).Abreviacion;
                // si tiene bauche es un banco así que tomar la información de ahí
                string nombreFormaPago;
                double cierreCajaMontoSistema;
                double cierreCajaConfirmacion;

                if (cierreCaja.Referencia != 0)
                {
                    Banco bancoUsado = bancos.Find(banco => banco.Id == cierreCaja.Emisor);
                    nombreFormaPago = bancoUsado.Nombre;
                }
                else if (cierreCaja.Emisor == 4 || cierreCaja.Emisor == 5)
                {
                    Banco bancoUsado = bancos.Find(banco => banco.Id == cierreCaja.Emisor);
                    nombreFormaPago = bancoUsado.Nombre;
                }
                else
                {
                    nombreFormaPago = pagos.Find(pago => pago.Id == cierreCaja.FormaPago).Nombre;
                }

                cierreCajaMontoSistema = cierreCaja.Monto;
                cierreCajaConfirmacion = 1.00;

                lineasAMostrarConcurrent.Add(new informacionCuadreCaja
                {
                    Abreviacion = abreviacionEncontrada,
                    TipoPago = nombreFormaPago,
                    CantidadSistema = cierreCajaMontoSistema,
                    BaucheRecibido = cierreCaja.Monto,
                    ConfirmacionSistema = 1.00
                });
            });

            // Convertimos la colección concurrente a una lista normal
            return lineasAMostrarConcurrent.ToList();
        }

        private CierreCaja ConvertirLineaACierreCaja(string linea)
        {
            if (string.IsNullOrWhiteSpace(linea))
                return null;

            string[] campos = linea.Split('|');

            // Verificar que la línea tenga el formato esperado (mínimo de campos necesarios)
            const int camposMinimosRequeridos = 10;
            if (campos.Length < camposMinimosRequeridos)
            {
                Console.WriteLine($"Formato de línea inválido. Se esperaban al menos {camposMinimosRequeridos} campos, pero se encontraron {campos.Length}: {linea}");
                return null;
            }

            try
            {
                var cierre = new CierreCaja();

                // Usar TryParse para campos numéricos para evitar excepciones
                if (!int.TryParse(campos[0], out int caja))
                    Console.WriteLine($"Error al convertir el campo Caja: {campos[0]}");
                cierre.Caja = caja;

                if (!int.TryParse(campos[1], out int credito))
                    Console.WriteLine($"Error al convertir el campo Credito: {campos[1]}");
                cierre.Credito = credito;

                if (!int.TryParse(campos[2], out int cajero))
                    Console.WriteLine($"Error al convertir el campo Cajero: {campos[2]}");
                cierre.Cajero = cajero;

                if (!int.TryParse(campos[3], out int emisor))
                    Console.WriteLine($"Error al convertir el campo Emisor: {campos[3]}");
                cierre.Emisor = emisor;

                cierre.Cedula = campos[4]?.Trim() ?? string.Empty;
                cierre.CajaRespaldo = campos[5]?.Trim() ?? string.Empty;

                if (!int.TryParse(campos[6], out int nroFactura))
                    Console.WriteLine($"Error al convertir el campo NroFactura: {campos[6]}");
                cierre.NroFactura = nroFactura;

                if (!int.TryParse(campos[7], out int formaPago))
                    Console.WriteLine($"Error al convertir el campo FormaPago: {campos[7]}");
                cierre.FormaPago = formaPago;

                if (!double.TryParse(campos[8], out double cantidad))
                    Console.WriteLine($"Error al convertir el campo cantidad: {campos[8]}");
                cierre.cantidad = cantidad;

                if (!double.TryParse(campos[9], out double monto))
                    Console.WriteLine($"Error al convertir el campo Monto: {campos[9]}");
                cierre.Monto = monto;

                // Campos opcionales
                cierre.Referencia = campos.Length > 10 && !string.IsNullOrEmpty(campos[10]) && int.TryParse(campos[10], out int referencia)
                    ? referencia
                    : 0;

                cierre.Fecha = campos.Length > 11 ? campos[11]?.Trim() ?? string.Empty : string.Empty;
                cierre.Hora = campos.Length > 12 ? campos[12]?.Trim() ?? string.Empty : string.Empty;

                return cierre;
            }
            catch (Exception ex)
            {
                // Mejorar el registro de errores con más contexto
                Console.WriteLine($"Error inesperado al convertir línea a CierreCaja: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"Línea problemática: {linea}");
                return null;
            }
        }
    }
}
