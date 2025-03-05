using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apos_gestor_caja.applicationLayer.interfaces
{
    public interface IArchivoService
    {
        Task<List<string>> ObtenerArchivosVentasAsync(DateTime fechaInicio, DateTime fechaFin, string caja = null);
        Task<bool> SubirArchivosVentasAsync(DateTime fechaInicio, DateTime fechaFin, string caja = null);
    }
}