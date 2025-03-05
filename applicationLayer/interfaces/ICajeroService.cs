using System.Collections.Generic;
using System.Threading.Tasks;
using apos_gestor_caja.Domain.Models;

namespace apos_gestor_caja.ApplicationLayer.Interfaces
{
    public interface ICajeroService
    {
        Task<bool> CrearCajeroAsync(Cajero cajero);
        Task<bool> VerificarUsuarioExistenteAsync(string usuario);
        Task<List<Cajero>> ObtenerCajerosAsync();
        Task<Cajero> ObtenerCajeroPorIdAsync(int id);
        Task<bool> ActualizarCajeroAsync(Cajero cajero);
        Task<List<Cajero>> BuscarCajerosPorNombreAsync(string nombre);
        Task<Cajero> DesactivarCajeroPorIdAsync(int id);

        Task<Cajero> ActivarCajeroPorIdAsync(int id);
    }
}