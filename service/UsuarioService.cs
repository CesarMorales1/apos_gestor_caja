
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using apos_gestor_caja.applicationLayer.interfaces;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.Infrastructure.Repositories;

namespace apos_gestor_caja.service
{
    public class UsuarioService : RepositoryBase,IUsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService()
        {
            _usuarioRepository = new UsuarioRepository();
        }

        public async Task<Usuario> ObtenerUsuario(string nombre, string password)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("El nombre de usuario y la contraseña no pueden estar vacíos.");

            return await _usuarioRepository.ObtenerUsuario(nombre, password);
        }

        public async Task<bool> AddUsuarioAsync(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            ValidarDatosUsuario(usuario);

            return await _usuarioRepository.AddUsuarioAsync(usuario);
        }

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            return await _usuarioRepository.ObtenerUsuariosAsync();
        }

        public async Task<Usuario> ObtenerUsuarioPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del usuario no es válido.");

            return await _usuarioRepository.ObtenerUsuarioPorIdAsync(id);
        }

        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            ValidarDatosUsuario(usuario);

            return await _usuarioRepository.ActualizarUsuarioAsync(usuario);
        }

        public async Task<List<Usuario>> BuscarUsuariosPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return await ObtenerUsuariosAsync();

            return await _usuarioRepository.BuscarUsuariosPorNombreAsync(nombre);
        }

        public async Task<Usuario> DesactivarUsuarioPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del usuario no es válido.");

            var usuario = await _usuarioRepository.DesactivarUsuarioPorIdAsync(id);
            if (usuario == null)
                throw new Exception($"No se encontró un usuario con el ID {id} para desactivar.");

            return usuario;
        }

        public async Task<Usuario> ActivarUsuarioPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del usuario no es válido.");

            var usuario = await _usuarioRepository.ActivarUsuarioPorIdAsync(id);
            if (usuario == null)
                throw new Exception($"No se encontró un usuario con el ID {id} para activar.");

            return usuario;
        }

        public async Task<bool> VerificarUsuarioExistenteAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");

            return await _usuarioRepository.VerificarUsuarioExistenteAsync(username);
        }

        private void ValidarDatosUsuario(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                throw new ArgumentException("El nombre del usuario no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(usuario.Username))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(usuario.Password))
                throw new ArgumentException("La contraseña no puede estar vacía.");
        }
    }
}
