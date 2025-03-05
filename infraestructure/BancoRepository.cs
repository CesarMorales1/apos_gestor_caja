using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.applicationLayer.interfaces;

namespace apos_gestor_caja.Infrastructure.Repositories
{
    public class BancoRepository : RepositoryBase, IBancoService
    {
        public BancoRepository() : base()
        {
            
        }

        public async Task<bool> AddBancoAsync(Banco banco)
        {
            string query = @"INSERT INTO apos01 (d2, activo) 
                             VALUES (@nombre, @activo);
                             SELECT LAST_INSERT_ID();";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@nombre", banco.Nombre),
                new MySqlParameter("@activo", banco.Activo ? 1 : 0)
            };

            try
            {
                // Crear una función para obtener el ID del registro insertado
                Func<MySqlCommand, Task<int?>> getRegistroId = async (cmd) => {
                    try
                    {
                        // Necesitamos ejecutar la consulta en la misma conexión
                        var query1 = "SELECT LAST_INSERT_ID()";
                        using (var idCmd = new MySqlCommand(query1, cmd.Connection))
                        {
                            var result = await idCmd.ExecuteScalarAsync();
                            if (result != null && result != DBNull.Value)
                            {
                                return Convert.ToInt32(result);
                            }
                        }
                        return null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al obtener ID: {ex.Message}");
                        return null;
                    }
                };


                // Ejecutar la operación con auditoría
                int rowsAffected = await ExecuteNonQueryWithAuditAsync(
                    query,
                    parameters,
                    "CREAR",
                    "apos01",
                    getRegistroId,
                    $"Creación de banco: {banco.Nombre}"
                );

                Console.WriteLine($"{getRegistroId}");


                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir banco: {ex.Message}");
                throw new Exception($"Error al añadir banco: {ex.Message}", ex);
            }
        }

        public async Task<bool> ActualizarBancoAsync(Banco banco)
        {
            string query = @"UPDATE apos01 
                             SET d2 = @nombre, 
                                 activo = @activo 
                             WHERE d1 = @id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@id", banco.Id),
                new MySqlParameter("@nombre", banco.Nombre),
                new MySqlParameter("@activo", banco.Activo ? 1 : 0)
            };

            try
            {
                // Ejecutar la operación con auditoría
                int rowsAffected = await ExecuteNonQueryWithAuditAsync(
                    query,
                    parameters,
                    "ACTUALIZAR",
                    "apos01",
                    banco.Id,
                    $"Actualización de banco ID: {banco.Id}, Nombre: {banco.Nombre}"
                );

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar banco: {ex.Message}");
                throw new Exception($"Error al actualizar banco con ID {banco.Id}: {ex.Message}", ex);
            }
        }

        public async Task<Banco> ActivarBancoAsync(Banco banco)
        {
            string updateQuery = @"UPDATE apos01 
                                  SET activo = 1 
                                  WHERE d1 = @id";

            string selectQuery = @"SELECT 
                                   d1 AS Id, 
                                   d2 AS Nombre, 
                                   activo AS Activo 
                                  FROM apos01 
                                  WHERE d1 = @id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@id", banco.Id)
            };

            try
            {
                // Primero ejecutar la actualización con auditoría
                int rowsAffected = await ExecuteNonQueryWithAuditAsync(
                    updateQuery,
                    parameters,
                    "ACTIVAR",
                    "apos01",
                    banco.Id,
                    $"Activación de banco ID: {banco.Id}"
                );

                if (rowsAffected > 0)
                {
                    // Luego obtener el banco actualizado
                    using (var reader = await ExecuteReaderAsync(selectQuery, parameters))
                    {
                        if (await reader.ReadAsync())
                        {
                            var bancoObtenido = new Banco
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            };
                            return bancoObtenido;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al activar banco: {ex.Message}");
                throw new Exception($"Error al activar banco con ID {banco.Id}: {ex.Message}", ex);
            }
        }

        public async Task<Banco> DesactivarBancoAsync(Banco banco)
        {
            string updateQuery = @"UPDATE apos01 
                                  SET activo = 0 
                                  WHERE d1 = @id";

            string selectQuery = @"SELECT 
                                   d1 AS Id, 
                                   d2 AS Nombre, 
                                   activo AS Activo 
                                  FROM apos01 
                                  WHERE d1 = @id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@id", banco.Id)
            };

            try
            {
                // Primero ejecutar la actualización con auditoría
                int rowsAffected = await ExecuteNonQueryWithAuditAsync(
                    updateQuery,
                    parameters,
                    "DESACTIVAR",
                    "apos01",
                    banco.Id,
                    $"Desactivación de banco ID: {banco.Id}"
                );

                if (rowsAffected > 0)
                {
                    // Luego obtener el banco actualizado
                    using (var reader = await ExecuteReaderAsync(selectQuery, parameters))
                    {
                        if (await reader.ReadAsync())
                        {
                            var bancoObtenido = new Banco
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            };
                            return bancoObtenido;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al desactivar banco: {ex.Message}");
                throw new Exception($"Error al desactivar banco con ID {banco.Id}: {ex.Message}", ex);
            }
        }

        public async Task<Banco> GetBancoByIdAsync(int id)
        {
            string query = @"SELECT 
                                   d1 AS Id, 
                                   d2 AS Nombre, 
                                   activo AS Activo 
                             FROM apos01 
                             WHERE d1 = @id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@id", id)
            };

            try
            {
                // Para lecturas (SELECT) usamos ExecuteReaderAsync sin auditoría
                using (var reader = await ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        var banco = new Banco
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                        };
                        return banco;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener banco: {ex.Message}");
                throw new Exception($"Error al obtener banco con ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<List<Banco>> ObtenerBancosAsync()
        {
            string query = @"SELECT 
                                   d1 AS Id, 
                                   d2 AS Nombre, 
                                   activo AS Activo 
                             FROM apos01 
                             ORDER BY d1 ASC";

            var bancos = new List<Banco>();

            try
            {
                // Para lecturas (SELECT) usamos ExecuteReaderAsync sin auditoría
                using (var reader = await ExecuteReaderAsync(query))
                {
                    while (await reader.ReadAsync())
                    {
                        bancos.Add(new Banco
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                        });
                    }
                }
                return bancos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener bancos: {ex.Message}");
                throw new Exception($"Error al obtener bancos: {ex.Message}", ex);
            }
        }

        public async Task<bool> VerificarBancoExistenteAsync(string nombre)
        {
            string query = @"SELECT COUNT(*) 
                             FROM apos01 
                             WHERE d2 = @nombre";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@nombre", nombre)
            };

            try
            {
                // Usar el método ExecuteReaderAsync de la clase base
                using (var reader = await ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        int count = reader.GetInt32(0);
                        return count > 0;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar banco: {ex.Message}");
                throw new Exception($"Error al verificar banco: {ex.Message}", ex);
            }
        }
    }
}