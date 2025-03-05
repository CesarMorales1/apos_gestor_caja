using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.Infrastructure.Database;
using apos_gestor_caja.Domain.Models;

namespace apos_gestor_caja.applicationLayer.interfaces
{
    public interface IBancoService
    {
        Task<bool> AddBancoAsync(Banco banco);
        Task<List<Banco>> ObtenerBancosAsync();
        Task<Banco> GetBancoByIdAsync(int id);
        Task<bool> VerificarBancoExistenteAsync(string nombre);
        Task<bool> ActualizarBancoAsync(Banco banco);
        Task<Banco> ActivarBancoAsync(Banco banco);
        Task<Banco> DesactivarBancoAsync(Banco banco);
    }
}