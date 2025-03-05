using System;
using apos_gestor_caja.Domain.Models;

namespace apos_gestor_caja.service
{
    public class UserInformation
    {
        private static UserInformation instance;
        private static readonly object lockObject = new object();
        private Usuario usuarioLogueado;

        // Constructor privado para evitar instanciación directa
        private UserInformation()
        {
            usuarioLogueado = null; // Inicialmente no hay usuario logueado
        }

        // Método estático para inicializar o actualizar el usuario logueado
        public static void SetUsuario(Usuario usuario)
        {
            lock (lockObject) // Asegura thread-safety
            {
                if (instance == null)
                {
                    instance = new UserInformation();
                }
                instance.usuarioLogueado = usuario;
            }
        }

        // Propiedad para acceder a la instancia única
        public static UserInformation Instance
        {
            get
            {
                lock (lockObject) // Asegura thread-safety
                {
                    if (instance == null)
                    {
                        instance = new UserInformation();
                    }
                    return instance;
                }
            }
        }

        // Método para obtener el usuario logueado
        public Usuario GetUsuario()
        {
            return usuarioLogueado;
        }

        // Método para limpiar el usuario (por ejemplo, al cerrar sesión)
        public void ClearUsuario()
        {
            usuarioLogueado = null;
        }

        // Propiedades útiles para acceder a datos del usuario directamente

        public int Id => usuarioLogueado?.Id ?? 0;
        public string Nombre => usuarioLogueado?.Nombre ?? string.Empty;
        public string Username => usuarioLogueado?.Username ?? string.Empty;
        public bool Activo => usuarioLogueado?.Activo ?? false;
        public bool IsLoggedIn => usuarioLogueado != null;
    }
}