using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using apos_gestor_caja.Domain.Models;
using DocumentFormat.OpenXml.Bibliography;

namespace apos_gestor_caja.applicationLayer.interfaces
{
    public interface IEmisorService
    {
        Task<bool> AddEmisorAsync(Emisor emisor);
        Task<List<Emisor>> ObtenerEmisoresAsync();
        Task<Emisor> GetEmisorByIdAsync(int id);
        Task<bool> VerificarEmisorExistenteAsync(string nombre);
        Task<bool> ActualizarEmisorAsync(Emisor emisor);
        Task<Emisor> ActivarEmisorAsync(Emisor emisor);
        Task<Emisor> DesactivarEmisorAsync(Emisor emisor);
    }
}
