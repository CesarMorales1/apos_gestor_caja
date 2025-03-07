
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.applicationLayer.interfaces;
using MyApp.Infrastructure.Database;

namespace apos_gestor_caja.Infrastructure.Repositories
{
    public class UsuarioRepository : RepositoryBase,IUsuarioService
    {


        public UsuarioRepository()
        {

        }

        public async Task<Usuario> ObtenerUsuario(string nombre, string password)
        {
            string query = @"
                SELECT  
                    id      AS Id,
                    nombre  AS Nombre, 
                    password AS Password, 
                    usuario  AS Username, 
                    activo  AS Activo 
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
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            };
                            Console.WriteLine($"Usuario encontrado - ID: {usuario.Id}, Nombre: {usuario.Nombre}");
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

        public async Task<bool> AddUsuarioAsync(Usuario usuario)
        {
            string query = @"
                INSERT INTO usuarios (nombre, password, usuario, activo) 
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

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            string query = @"
                SELECT 
                    id AS Id, 
                    nombre AS Nombre, 
                    password AS Password, 
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
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
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

        public async Task<Usuario> ObtenerUsuarioPorIdAsync(int id)
        {
            string query = @"
                SELECT 
                    id AS Id, 
                    nombre AS Nombre, 
                    password AS Password, 
                    usuario AS Username, 
                    activo AS Activo 
                FROM usuarios 
                WHERE id = @id";

            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine($"Conexión obtenida para ObtenerUsuarioPorIdAsync - ID: {id}");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

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
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            };
                            Console.WriteLine($"Usuario encontrado - ID: {usuario.Id}, Nombre: {usuario.Nombre}");
                            return usuario;
                        }
                        Console.WriteLine($"No se encontró usuario con ID: {id}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuario por ID: {ex.Message}");
                throw new Exception($"Error al obtener usuario con ID {id}: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en ObtenerUsuarioPorIdAsync");
                }
            }
        }

        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario)
        {
            string query = @"
                UPDATE usuarios 
                SET nombre = @nombre, 
                    password = @password, 
                    usuario = @username, 
                    activo = @activo 
                WHERE id = @id";

            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine($"Conexión obtenida para ActualizarUsuarioAsync - ID: {usuario.Id}");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", usuario.Id);
                    command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@password", usuario.Password);
                    command.Parameters.AddWithValue("@username", usuario.Username);
                    command.Parameters.AddWithValue("@activo", usuario.Activo ? 1 : 0);

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Usuario actualizado - ID: {usuario.Id}, Filas afectadas: {rowsAffected}");
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar usuario: {ex.Message}");
                throw new Exception($"Error al actualizar usuario con ID {usuario.Id}: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en ActualizarUsuarioAsync");
                }
            }
        }

        public async Task<List<Usuario>> BuscarUsuariosPorNombreAsync(string nombre)
        {
            string query = @"
                SELECT 
                    id AS Id, 
                    nombre AS Nombre, 
                    password AS Password, 
                    usuario AS Username, 
                    activo AS Activo 
                FROM usuarios 
                WHERE nombre LIKE @nombre 
                ORDER BY id ASC";

            var usuarios = new List<Usuario>();
            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine($"Conexión obtenida para BuscarUsuariosPorNombreAsync - Nombre: {nombre}");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", $"%{nombre}%");

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
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            });
                        }
                    }
                }
                Console.WriteLine($"Usuarios encontrados - Total: {usuarios.Count}");
                return usuarios;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar usuarios: {ex.Message}");
                throw new Exception($"Error al buscar usuarios con nombre {nombre}: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en BuscarUsuariosPorNombreAsync");
                }
            }
        }

        public async Task<Usuario> DesactivarUsuarioPorIdAsync(int id)
        {
            string updateQuery = "UPDATE usuarios SET activo = 0 WHERE id = @id";
            string selectQuery = @"
                SELECT 
                    id AS Id, 
                    nombre AS Nombre, 
                    password AS Password, 
                    usuario AS Username, 
                    activo AS Activo 
                FROM usuarios 
                WHERE id = @id";

            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine($"Conexión obtenida para DesactivarUsuarioPorIdAsync - ID: {id}");

                int rowsAffected;
                using (var command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    rowsAffected = await command.ExecuteNonQueryAsync();
                }

                if (rowsAffected > 0)
                {
                    using (var command = new MySqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var usuario = new Usuario
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Password = reader.GetString(reader.GetOrdinal("Password")),
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                                };
                                Console.WriteLine($"Usuario desactivado - ID: {usuario.Id}, Activo: {usuario.Activo}");
                                return usuario;
                            }
                            Console.WriteLine($"No se encontró usuario con ID: {id}");
                            return null;
                        }
                    }
                }
                Console.WriteLine($"No se afectaron filas al desactivar usuario ID: {id}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al desactivar usuario: {ex.Message}");
                throw new Exception($"Error al desactivar usuario con ID {id}: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en DesactivarUsuarioPorIdAsync");
                }
            }
        }

        public async Task<Usuario> ActivarUsuarioPorIdAsync(int id)
        {
            string updateQuery = "UPDATE usuarios SET activo = 1 WHERE id = @id";
            string selectQuery = @"
                SELECT 
                    id AS Id, 
                    nombre AS Nombre, 
                    password AS Password, 
                    usuario AS Username, 
                    activo AS Activo 
                FROM usuarios 
                WHERE id = @id";

            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine($"Conexión obtenida para ActivarUsuarioPorIdAsync - ID: {id}");

                int rowsAffected;
                using (var command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    rowsAffected = await command.ExecuteNonQueryAsync();
                }

                if (rowsAffected > 0)
                {
                    using (var command = new MySqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var usuario = new Usuario
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Password = reader.GetString(reader.GetOrdinal("Password")),
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                                };
                                Console.WriteLine($"Usuario activado - ID: {usuario.Id}, Activo: {usuario.Activo}");
                                return usuario;
                            }
                            Console.WriteLine($"No se encontró usuario con ID: {id}");
                            return null;
                        }
                    }
                }
                Console.WriteLine($"No se afectaron filas al activar usuario ID: {id}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al activar usuario: {ex.Message}");
                throw new Exception($"Error al activar usuario con ID {id}: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en ActivarUsuarioPorIdAsync");
                }
            }
        }

        public async Task<bool> VerificarUsuarioExistenteAsync(string username)
        {
            string query = "SELECT COUNT(*) FROM usuarios WHERE usuario = @username";

            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine($"Conexión obtenida para VerificarUsuarioExistenteAsync - Username: {username}");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    long count = (long)await command.ExecuteScalarAsync();
                    Console.WriteLine($"Verificación de usuario existente - Username: {username}, Existe: {count > 0}");
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar usuario existente: {ex.Message}");
                throw new Exception($"Error al verificar si existe un usuario con username {username}: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en VerificarUsuarioExistenteAsync");
                }
            }
        }
    }
}
