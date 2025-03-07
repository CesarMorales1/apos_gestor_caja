using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using apos_gestor_caja.Domain.Models;

namespace apos_gestor_caja.applicationLayer.interfaces
{
    public interface ICierreCaja
    {
        Task<List<string>> ProcesarArchivoCierreCajaAsync(string filePath,int idCajero);
        Task<List<string>> ObtenerArchivosApos13(int idCajero, DateTime fechaDelCierre, int nroCaja);
        Task<List<CierreCaja>> ObtenerCierreCaja(int idCajero, DateTime fechaDelCierre, int nroCaja);
    }
}
