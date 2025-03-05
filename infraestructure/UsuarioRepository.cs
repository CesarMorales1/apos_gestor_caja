using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.applicationLayer.interfaces;
using MyApp.Infrastructure.Database;
using DocumentFormat.OpenXml.Wordprocessing;
using static ClosedXML.Excel.XLPredefinedFormat;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace apos_gestor_caja.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioService
    {
        private readonly SqlHelper _sqlHelper;

        public UsuarioRepository()
        {
            _sqlHelper = new SqlHelper();
        }

        public async Task<Usuario> ObtenerUsuario(string nombre, string password)
        {
            string query = @"
                SELECT  
                    id     AS Id,
                    nombre AS Nombre, 
                    password AS Password, 
                    usuario AS Username, 
                    activo AS Activo 
                FROM usuarios 
                WHERE nombre = @nombre AND password = @password";

            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine($"Conexión obtenida para ObtenerUsuario - Nombre: {nombre}");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@password", password);

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var usuario = new Usuario
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            };
                            Console.WriteLine($"Usuario encontrado - ID:  Nombre: {usuario.Nombre}");
                            return usuario;
                        }
                        Console.WriteLine($"No se encontró usuario con Nombre: {nombre}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuario: {ex.Message}");
                throw new Exception($"Error al obtener usuario con Nombre {nombre}: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en ObtenerUsuario");
                }
            }
        }

        // Método opcional para agregar un usuario (similar a AddEmisorAsync)
        public async Task<bool> AddUsuarioAsync(Usuario usuario)
        {
            string query = @"
                INSERT INTO usuarios (nombre, contrasena, usuario, activo) 
                VALUES (@nombre, @password, @username, @activo)";

            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine($"Conexión obtenida para AddUsuarioAsync - Nombre: {usuario.Nombre}");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@password", usuario.Password);
                    command.Parameters.AddWithValue("@username", usuario.Username);
                    command.Parameters.AddWithValue("@activo", usuario.Activo ? 1 : 0);

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Usuario añadido - Nombre: {usuario.Nombre}, Filas afectadas: {rowsAffected}");
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir usuario: {ex.Message}");
                throw new Exception($"Error al añadir usuario: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en AddUsuarioAsync");
                }
            }
        }

        // Método opcional para obtener todos los usuarios (similar a ObtenerEmisoresAsync)
        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            string query = @"
                SELECT 
                    id AS Id, 
                    nombre AS Nombre, 
                    contrasena AS Password, 
                    usuario AS Username, 
                    activo AS Activo 
                FROM usuarios 
                ORDER BY id ASC";

            var usuarios = new List<Usuario>();
            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine("Conexión obtenida para ObtenerUsuariosAsync");

                using (var command = new MySqlCommand(query, connection))
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(new Usuario
                            {
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            });
                        }
                    }
                }
                Console.WriteLine($"Usuarios obtenidos - Total: {usuarios.Count}");
                return usuarios;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
                throw new Exception($"Error al obtener usuarios: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en ObtenerUsuariosAsync");
                }
            }
        }
    }
}