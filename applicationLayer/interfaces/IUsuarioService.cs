
using System.Collections.Generic;
using System.Threading.Tasks;
using apos_gestor_caja.Domain.Models;

namespace apos_gestor_caja.applicationLayer.interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> ObtenerUsuario(string nombre, string password);
        Task<bool> AddUsuarioAsync(Usuario usuario);
        Task<List<Usuario>> ObtenerUsuariosAsync();
        Task<Usuario> ObtenerUsuarioPorIdAsync(int id);
        Task<bool> ActualizarUsuarioAsync(Usuario usuario);
        Task<List<Usuario>> BuscarUsuariosPorNombreAsync(string nombre);
        Task<Usuario> DesactivarUsuarioPorIdAsync(int id);
        Task<Usuario> ActivarUsuarioPorIdAsync(int id);
        Task<bool> VerificarUsuarioExistenteAsync(string username);
    }
}
