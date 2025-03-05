using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.applicationLayer.interfaces;
using MyApp.Infrastructure.Database;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace apos_gestor_caja.Infrastructure.Repositories
{
    public class EmisorRepository : RepositoryBase, IEmisorService
    {

        public EmisorRepository()
        {
        }

        public async Task<bool> AddEmisorAsync(Emisor emisor)
        {
            string query = @"INSERT INTO apos04 (d2, activo) 
                     VALUES (@nombre, @activo)";
            try
            {
                Console.WriteLine("Conexión obtenida para AddEmisorAsync");
                //creando array de parametros
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@nombre",emisor.Nombre),
                    new MySqlParameter("@activo",emisor.Activo ? 1 : 0)

                };

                Func<MySqlCommand, Task<int?>> getRegistroId = async (command) =>
                {
                    command.CommandText = "SELECT LAST_INSERT_ID()";
                    var result = await command.ExecuteScalarAsync();
                    return result != null && result != DBNull.Value ?
                    Convert.ToInt32(result) : throw new Exception("No se pudo obtener el id del ultimo registro");
                };

                string detalles = $"Emisor {emisor.Nombre} creado";

                int rowsAffected = await ExecuteNonQueryWithAuditAsync
                    (
                    query,
                    parameters,
                    "CREAR",
                    "apos04",
                    getRegistroId,
                    detalles);

                return rowsAffected > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir emisor: {ex.Message}");
                throw new Exception($"Error al añadir emisor: {ex.Message}", ex);
            } 
        }

        public async Task<bool> ActualizarEmisorAsync(Emisor emisor)
        {
            string query = @"UPDATE apos04 
                          SET d2 = @nombre, 
                              activo = @activo 
                          WHERE d1 = @id";
            try
            {
                Console.WriteLine($"Conexión obtenida para ActualizarEmisorAsync con ID: {emisor.Id}");

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id",emisor.Id),
                    new MySqlParameter("@nombre",emisor.Nombre),
                    new MySqlParameter("@activo",emisor.Activo ? 1 : 0)
                };

                int rowsAffected = await ExecuteNonQueryWithAuditAsync
                    (
                    query,
                    parameters,
                    "ACTUALIZAR",
                    "apos04",
                    emisor.Id
                    );
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar emisor: {ex.Message}");
                throw new Exception($"Error al actualizar emisor con ID {emisor.Id}: {ex.Message}", ex);
            }
        }

        public async Task<Emisor> ActivarEmisorAsync(Emisor emisor)
        {
            string query = @"UPDATE apos04 
                          SET activo = 1 
                          WHERE d1 = @id; 
                          SELECT 
                                d1 AS Id, 
                                d2 AS Nombre, 
                                activo AS Activo 
                          FROM apos04 
                          WHERE d1 = @id";

            MySqlConnection connection = null;
            try
            {
                Console.WriteLine($"Conexión obtenida para ActivarEmisorAsync con ID: {emisor.Id}");

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id",emisor.Id)
                };


                int rowsAffected = await ExecuteNonQueryWithAuditAsync(query, parameters, "ACTIVAR", "APOS04", emisor.Id);

                if(rowsAffected > 0)
                {
                    using (var reader = await ExecuteReaderAsync(query,parameters))
                    {
                        if (await reader.ReadAsync())
                        {
                            var emisorActualizado = new Emisor
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            };
                            Console.WriteLine($"Emisor activado - ID: {emisorActualizado.Id}, Activo: {emisorActualizado.Activo}");
                            return emisorActualizado;
                        }

                    }
                }

                Console.WriteLine($"No se encontró emisor con ID: {emisor.Id}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al activar emisor: {ex.Message}");
                throw new Exception($"Error al activar emisor con ID {emisor.Id}: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en ActivarEmisorAsync");
                }
            }
        }

        public async Task<Emisor> DesactivarEmisorAsync(Emisor emisor)
        {
            string query = @"UPDATE apos04 
                          SET activo = 0 
                          WHERE d1 = @id; 
                          SELECT 
                                d1 AS Id, 
                                d2 AS Nombre, 
                                activo AS Activo 
                          FROM apos04 
                          WHERE d1 = @id";

            try
            {
                Console.WriteLine($"Conexión obtenida para DesactivarEmisorAsync con ID: {emisor.Id}");

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id", emisor.Id)
                };

                int rowsAffected = await ExecuteNonQueryWithAuditAsync(query, parameters, "DESACTIVAR", "APOS04", emisor.Id); 

                if(rowsAffected > 0) { 
                    using(var reader = await ExecuteReaderAsync(query, parameters))
                    {
                        if(await reader.ReadAsync())
                        {
                            var emisorObtenido = new Emisor
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                            };
                        };
                    }

                    return emisor;
                }
                Console.WriteLine($"Ah ocurrido un error el desactivar el emisor con el id {emisor.Id}"); 
                return null;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error al desactivar emisor: {ex.Message}");
                throw new Exception($"Error al desactivar emisor con ID {emisor.Id}: {ex.Message}", ex);
            }
        }

        public async Task<List<Emisor>> ObtenerEmisoresAsync()
        {
            string query = @"SELECT 
                                d1 AS Id, 
                                d2 AS Nombre, 
                                activo AS Activo 
                          FROM apos04 
                          ORDER BY d1 ASC";

            var emisores = new List<Emisor>();
            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine("Conexión obtenida para ObtenerEmisoresAsync");

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            emisores.Add(new Emisor
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            });
                        }
                    }
                }
                Console.WriteLine($"Emisores obtenidos - Total: {emisores.Count}");
                return emisores;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener emisores: {ex.Message}");
                throw new Exception($"Error al obtener emisores: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en ObtenerEmisoresAsync");
                }
            }
        }

        public async Task<Emisor> GetEmisorByIdAsync(int id)
        {
            string query = @"SELECT 
                                d1 AS Id, 
                                d2 AS Nombre, 
                                activo AS Activo 
                          FROM apos04 
                          WHERE d1 = @id";

            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine($"Conexión obtenida para GetEmisorByIdAsync con ID: {id}");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var emisor = new Emisor
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            };
                            Console.WriteLine($"Emisor encontrado - ID: {emisor.Id}, Nombre: {emisor.Nombre}");
                            return emisor;
                        }
                        Console.WriteLine($"No se encontró emisor con ID: {id}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener emisor: {ex.Message}");
                throw new Exception($"Error al obtener emisor con ID {id}: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en GetEmisorByIdAsync");
                }
            }
        }

        public async Task<bool> VerificarEmisorExistenteAsync(string nombre)
        {
            string query = @"SELECT COUNT(*) 
                          FROM apos04 
                          WHERE d2 = @nombre";

            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine("Conexión obtenida para VerificarEmisorExistenteAsync");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                    int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    Console.WriteLine($"Verificación de emisor existente - Nombre: {nombre}, Existe: {count > 0}");
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar emisor: {ex.Message}");
                throw new Exception($"Error al verificar emisor: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en VerificarEmisorExistenteAsync");
                }
            }
        }
    }
}
