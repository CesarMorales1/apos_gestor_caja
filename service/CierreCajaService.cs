
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using apos_gestor_caja.Domain.Models;

namespace apos_gestor_caja.service
{
    public class CierreCajaService
    {
        /// <summary>
        /// Procesa un archivo de cierre de caja y devuelve el ID del cajero encontrado
        /// </summary>
        /// <param name="filePath">Ruta del archivo a procesar</param>
        /// <returns>ID del cajero encontrado en el archivo</returns>
        public async Task<int> ProcesarArchivoCierreCajaAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"El archivo {filePath} no existe.");

            List<CierreCaja> cierres = new List<CierreCaja>();
            int idCajero = 0;

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

                // Procesar la primera línea para obtener el ID del cajero
                string primeraLinea = lineas[0];
                string[] campos = primeraLinea.Split('|');

                // Verificar que la línea tenga el formato esperado
                if (campos.Length < 3)
                    throw new FormatException("El formato del archivo no es válido.");

                // El ID del cajero está en la posición 2 (índice 2) según el modelo
                if (int.TryParse(campos[2], out idCajero))
                {
                    Console.WriteLine($"ID de cajero encontrado: {idCajero}");
                }
                else
                {
                    throw new FormatException("No se pudo extraer el ID del cajero del archivo.");
                }

                // Procesar todas las líneas y convertirlas a objetos CierreCaja
                foreach (string linea in lineas)
                {
                    CierreCaja cierre = ConvertirLineaACierreCaja(linea);
                    if (cierre != null)
                    {
                        cierres.Add(cierre);
                    }
                }

                Console.WriteLine($"Se procesaron {cierres.Count} líneas de cierre de caja.");
                return idCajero;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar el archivo de cierre de caja: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Convierte una línea del archivo a un objeto CierreCaja
        /// </summary>
        private CierreCaja ConvertirLineaACierreCaja(string linea)
        {
            if (string.IsNullOrWhiteSpace(linea))
                return null;

            string[] campos = linea.Split('|');

            // Verificar que la línea tenga el formato esperado
            if (campos.Length < 13) // Al menos debería tener los campos básicos
                return null;

            try
            {
                CierreCaja cierre = new CierreCaja
                {
                    Caja = int.Parse(campos[0]),
                    Credito = int.Parse(campos[1]),
                    Cajero = int.Parse(campos[2]),
                    Emisor = int.Parse(campos[3]),
                    Cedula = campos[4],
                    CajaRespaldo = campos[5],
                    NroFactura = int.Parse(campos[6]),
                    FormaPago = int.Parse(campos[7]),
                    cantidad = double.Parse(campos[8]),
                    Monto = double.Parse(campos[9])
                };

                // Algunos campos pueden ser opcionales o tener formato diferente
                if (campos.Length > 10)
                {
                    cierre.Referencia = string.IsNullOrEmpty(campos[10]) ? 0 : int.Parse(campos[10]);
                }

                if (campos.Length > 11)
                {
                    cierre.Fecha = campos[11];
                }

                if (campos.Length > 12)
                {
                    cierre.Hora = campos[12];
                }

                return cierre;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al convertir línea a CierreCaja: {ex.Message}, Línea: {linea}");
                return null;
            }
        }

        /// <summary>
        /// Procesa un archivo y devuelve la lista completa de objetos CierreCaja
        /// </summary>
        public async Task<List<CierreCaja>> ObtenerCierresCajaAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"El archivo {filePath} no existe.");

            List<CierreCaja> cierres = new List<CierreCaja>();

            try
            {
                string[] lineas;
                using (var reader = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true)))
                {
                    var content = await reader.ReadToEndAsync();
                    lineas = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                }

                foreach (string linea in lineas)
                {
                    CierreCaja cierre = ConvertirLineaACierreCaja(linea);
                    if (cierre != null)
                    {
                        cierres.Add(cierre);
                    }
                }

                return cierres;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener cierres de caja: {ex.Message}");
                throw;
            }
        }
    }
}
