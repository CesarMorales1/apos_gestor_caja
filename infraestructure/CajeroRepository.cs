using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using apos_gestor_caja.Domain.Models;
using MyApp.Infrastructure.Database;
using apos_gestor_caja.Infrastructure.Repositories;

namespace apos_gestor_caja.Infrastructure.Repositories
{
    public class CajeroRepository : RepositoryBase
    {
        public CajeroRepository() : base()
        {
        }

        public async Task<List<Cajero>> BuscarCajerosPorUsuarioAsync(string usuario)
        {
            string query = @"SELECT 
                    d1 as Id, 
                    d2 as Usuario, 
                    d3 as Clave,
                    d4 as Barra,
                    d5 as Nombre, 
                    d6 as NivelAcceso,
                    activo as Activo
                    FROM apos03 WHERE d2 LIKE @usuario";

            var cajeros = new List<Cajero>();
            try
            {
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@usuario", $"%{usuario}%")
                };

                using (var reader = await ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        cajeros.Add(new Cajero
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Usuario = reader["Usuario"].ToString(),
                            Clave = reader["Clave"].ToString(),
                            Barra = Convert.ToInt32(reader["Barra"]),
                            Nombre = reader["Nombre"].ToString(),
                            NivelAcceso = Convert.ToInt32(reader["NivelAcceso"]),
                            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                        });
                    }
                }
                return cajeros;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar cajeros por usuario: {ex.Message}", ex);
            }
        }

        public async Task<bool> CrearCajeroAsync(Cajero cajero)
        {
            string query = @"INSERT INTO apos03 (d2, d3, d4, d5, d6, activo) 
                           VALUES (@usuario, @clave, @barra, @nombre, @nivelAcceso, 1)";

            try
            {
                Console.WriteLine("Iniciando CrearCajeroAsync");

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@usuario", cajero.Usuario),
                    new MySqlParameter("@clave", cajero.Clave),
                    new MySqlParameter("@barra", cajero.Barra),
                    new MySqlParameter("@nombre", cajero.Nombre),
                    new MySqlParameter("@nivelAcceso", cajero.NivelAcceso)
                };

                // Define función para obtener el ID del registro insertado
                Func<MySqlCommand, Task<int?>> getRegistroId = async (command) =>
                {
                    command.CommandText = "SELECT LAST_INSERT_ID()";
                    var result = await command.ExecuteScalarAsync();
                    return result != null && result != DBNull.Value ? Convert.ToInt32(result) : throw new Exception("No se pudo obtener el ID del registro insertado");
                };

                // Detalles adicionales para la auditoría
                string detalles = $"Usuario: {cajero.Usuario}, Nombre: {cajero.Nombre}, Barra: {cajero.Barra}";

                int rowsAffected = await ExecuteNonQueryWithAuditAsync(
                    query,
                    parameters,
                    "CREAR",
                    "apos03",
                    getRegistroId,
                    detalles);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear cajero: {ex.Message}");
                throw new Exception($"Error al crear cajero: {ex.Message}", ex);
            }
        }

        public async Task<bool> VerificarUsuarioExistenteAsync(string usuario)
        {
            string query = "SELECT COUNT(*) FROM apos03 WHERE d2 = @usuario";

            try
            {
                Console.WriteLine("Conexión obtenida para VerificarUsuarioExistenteAsync");
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@usuario", usuario)
                };

                using (var reader = await ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        int count = Convert.ToInt32(reader.GetInt64(0));
                        return count > 0;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar usuario: {ex.Message}");
                throw new Exception($"Error al verificar usuario: {ex.Message}", ex);
            }
        }

        public async Task<List<Cajero>> ObtenerCajerosAsync()
        {
            Console.WriteLine("Iniciando ObtenerCajerosAsync");
            string query = @"SELECT 
                           d1 as Id, 
                           d2 as Usuario, 
                           d3 as Clave,
                           d4 as Barra,
                           d5 as Nombre, 
                           d6 as NivelAcceso,
                           activo as Activo
                           FROM apos03 
                           ORDER BY d1 ASC";

            var cajeros = new List<Cajero>();
            try
            {
                using (var reader = await ExecuteReaderAsync(query))
                {
                    Console.WriteLine("DataReader obtenido, comenzando lectura");
                    while (await reader.ReadAsync())
                    {
                        var cajero = new Cajero
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                            Clave = reader.GetString(reader.GetOrdinal("Clave")),
                            Barra = reader.GetInt32(reader.GetOrdinal("Barra")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            NivelAcceso = reader.GetInt32(reader.GetOrdinal("NivelAcceso")),
                            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                        };
                        Console.WriteLine($"Cajero leído - ID: {cajero.Id}, Usuario: {cajero.Usuario}");
                        cajeros.Add(cajero);
                    }
                }
                Console.WriteLine($"Lectura completada. Total cajeros: {cajeros.Count}");
                return cajeros;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerCajerosAsync: {ex.Message}");
                throw new Exception($"Error al obtener cajeros: {ex.Message}", ex);
            }
        }

        public async Task<Cajero> DesactivarCajeroPorIdAsync(int id)
        {
            string updateQuery = @"UPDATE apos03 SET activo = 0 WHERE d1 = @id";
            string selectQuery = @"SELECT 
                                d1 as Id, 
                                d2 as Usuario, 
                                d3 as Clave,
                                d4 as Barra,
                                d5 as Nombre, 
                                d6 as NivelAcceso,
                                activo as Activo 
                                FROM apos03 
                                WHERE d1 = @id";

            try
            {
                Console.WriteLine($"Iniciando DesactivarCajeroPorIdAsync para ID: {id}");

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id", id)
                };

                // Ejecutamos el update con auditoría, pasando el ID directamente
                int rowsAffected = await ExecuteNonQueryWithAuditAsync(
                    updateQuery,
                    parameters,
                    "DESACTIVAR",
                    "apos03",
                    id, // No necesitamos getRegistroId porque ya tenemos el ID
                    $"Desactivación de cajero ID: {id}"
                );

                if (rowsAffected > 0)
                {
                    using (var reader = await ExecuteReaderAsync(selectQuery, parameters))
                    {
                        if (await reader.ReadAsync())
                        {
                            var cajero = new Cajero
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                                Clave = reader.GetString(reader.GetOrdinal("Clave")),
                                Barra = reader.GetInt32(reader.GetOrdinal("Barra")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                NivelAcceso = reader.GetInt32(reader.GetOrdinal("NivelAcceso")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            };
                            Console.WriteLine($"Cajero desactivado - ID: {cajero.Id}, Activo: {cajero.Activo}");
                            return cajero;
                        }
                        Console.WriteLine($"No se encontró cajero con ID: {id}");
                        return null;
                    }
                }
                Console.WriteLine($"No se afectaron filas al desactivar cajero ID: {id}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al desactivar cajero: {ex.Message}");
                throw new Exception($"Error al desactivar cajero con ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<Cajero> ActivarCajeroPorIdAsync(int id)
        {
            string updateQuery = @"UPDATE apos03 SET activo = 1 WHERE d1 = @id";
            string selectQuery = @"SELECT 
                                d1 as Id, 
                                d2 as Usuario, 
                                d3 as Clave,
                                d4 as Barra,
                                d5 as Nombre, 
                                d6 as NivelAcceso,
                                activo as Activo 
                                FROM apos03 
                                WHERE d1 = @id";

            try
            {
                Console.WriteLine($"Iniciando ActivarCajeroPorIdAsync para ID: {id}");

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id", id)
                };

                // Ejecutamos el update con auditoría, pasando el ID directamente
                int rowsAffected = await ExecuteNonQueryWithAuditAsync(
                    updateQuery,
                    parameters,
                    "ACTIVAR",
                    "apos03",
                    id,
                    $"Activación de cajero ID: {id}"
                );

                if (rowsAffected > 0)
                {
                    using (var reader = await ExecuteReaderAsync(selectQuery, parameters))
                    {
                        if (await reader.ReadAsync())
                        {
                            var cajero = new Cajero
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                                Clave = reader.GetString(reader.GetOrdinal("Clave")),
                                Barra = reader.GetInt32(reader.GetOrdinal("Barra")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                NivelAcceso = reader.GetInt32(reader.GetOrdinal("NivelAcceso")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            };
                            Console.WriteLine($"Cajero activado - ID: {cajero.Id}, Activo: {cajero.Activo}");
                            return cajero;
                        }
                        Console.WriteLine($"No se encontró cajero con ID: {id}");
                        return null;
                    }
                }
                Console.WriteLine($"No se afectaron filas al activar cajero ID: {id}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al activar cajero: {ex.Message}");
                throw new Exception($"Error al activar cajero con ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<Cajero> ObtenerCajeroPorIdAsync(int id)
        {
            Console.WriteLine($"Iniciando ObtenerCajeroPorIdAsync para ID: {id}");
            string query = @"SELECT 
                           d1 as Id, 
                           d2 as Usuario, 
                           d3 as Clave,
                           d4 as Barra,
                           d5 as Nombre, 
                           d6 as NivelAcceso,
                           activo as Activo
                           FROM apos03 
                           WHERE d1 = @id";

            try
            {
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id", id)
                };

                using (var reader = await ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        var cajero = new Cajero
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                            Clave = reader.GetString(reader.GetOrdinal("Clave")),
                            Barra = reader.GetInt32(reader.GetOrdinal("Barra")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            NivelAcceso = reader.GetInt32(reader.GetOrdinal("NivelAcceso")),
                            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                        };
                        Console.WriteLine($"Cajero encontrado - ID: {cajero.Id}, Usuario: {cajero.Usuario} con la barra: {cajero.Barra}");
                        return cajero;
                    }
                    Console.WriteLine($"No se encontró cajero con ID: {id}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerCajeroPorIdAsync: {ex.Message}");
                throw new Exception($"Error al obtener cajero por ID: {ex.Message}", ex);
            }
        }

        public async Task<bool> ActualizarCajeroAsync(Cajero cajero)
        {
            string query;
            MySqlParameter[] parameters;

            if (!string.IsNullOrWhiteSpace(cajero.Clave))
            {
                query = @"UPDATE apos03 
                         SET d2 = @usuario, 
                             d3 = @clave,
                             d4 = @barra,
                             d5 = @nombre, 
                             d6 = @nivelAcceso
                         WHERE d1 = @id";

                parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id", cajero.Id),
                    new MySqlParameter("@usuario", cajero.Usuario),
                    new MySqlParameter("@clave", cajero.Clave),
                    new MySqlParameter("@barra", cajero.Barra),
                    new MySqlParameter("@nombre", cajero.Nombre),
                    new MySqlParameter("@nivelAcceso", cajero.NivelAcceso)
                };
            }
            else
            {
                query = @"UPDATE apos03 
                         SET d2 = @usuario, 
                             d4 = @barra,
                             d5 = @nombre, 
                             d6 = @nivelAcceso
                         WHERE d1 = @id";

                parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id", cajero.Id),
                    new MySqlParameter("@usuario", cajero.Usuario),
                    new MySqlParameter("@barra", cajero.Barra),
                    new MySqlParameter("@nombre", cajero.Nombre),
                    new MySqlParameter("@nivelAcceso", cajero.NivelAcceso)
                };
            }

            try
            {
                Console.WriteLine($"Iniciando ActualizarCajeroAsync para ID: {cajero.Id}");

                // Detalles para la auditoría
                string detalles = $"Usuario: {cajero.Usuario}, Nombre: {cajero.Nombre}, Barra: {cajero.Barra}";

                int rowsAffected = await ExecuteNonQueryWithAuditAsync(
                    query,
                    parameters,
                    "ACTUALIZAR",
                    "apos03",
                    cajero.Id,
                    detalles);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar cajero: {ex.Message}");
                throw new Exception($"Error al actualizar cajero: {ex.Message}", ex);
            }
        }

        public async Task<List<Cajero>> BuscarCajerosPorNombreAsync(string nombre)
        {
            string query = @"SELECT 
                           d1 as Id, 
                           d2 as Usuario, 
                           d3 as Clave,
                           d4 as Barra,
                           d5 as Nombre, 
                           d6 as NivelAcceso,
                           activo as Activo 
                           FROM apos03 WHERE d5 LIKE @nombre";

            var cajeros = new List<Cajero>();
            try
            {
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@nombre", $"%{nombre}%")
                };

                using (var reader = await ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        cajeros.Add(new Cajero
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Usuario = reader["Usuario"].ToString(),
                            Clave = reader["Clave"].ToString(),
                            Barra = Convert.ToInt32(reader["Barra"]),
                            Nombre = reader["Nombre"].ToString(),
                            NivelAcceso = Convert.ToInt32(reader["NivelAcceso"]),
                            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                        });
                    }
                }
                return cajeros;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar cajeros: {ex.Message}");
                throw new Exception($"Error al buscar cajeros: {ex.Message}", ex);
            }
        }
    }
}