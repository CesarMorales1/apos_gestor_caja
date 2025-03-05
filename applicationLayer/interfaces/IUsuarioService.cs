using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apos_gestor_caja.Domain.Models;

namespace apos_gestor_caja.applicationLayer.interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> ObtenerUsuario(string nombre, string password);
    }
}
