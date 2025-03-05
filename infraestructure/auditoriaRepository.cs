using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MyApp.Infrastructure.Database;
using apos_gestor_caja.service;
using System.Drawing.Text;

namespace apos_gestor_caja.Infrastructure.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly SqlHelper _sqlHelper;

        protected RepositoryBase()
        {
            _sqlHelper = new SqlHelper();
        }

        protected async Task<int> ExecuteNonQueryWithAuditAsync(
            string query,
            MySqlParameter[] parameters,
            string accion,
            string tablaAfectada,
            Func<MySqlCommand, Task<int?>> getRegistroId = null,
            string detalles = null)
        {
            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine($"Conexión obtenida para {accion} en {tablaAfectada}");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Operación ejecutada - Filas afectadas: {rowsAffected}");

                    if (rowsAffected > 0)
                    {
                        // Obtener el ID del registro afectado si se proporciona una función
                        int? registroId = null;
                        if (getRegistroId != null)
                        {
                            // Asegurarnos de esperar el resultado de la promesa
                            registroId = await getRegistroId(command);
                        }

                        // Registrar auditoría
                        await RegistrarAuditoria(accion, tablaAfectada, registroId, detalles);
                    }

                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar operación con auditoría: {ex.Message}");
                throw new Exception($"Error al ejecutar {accion} en {tablaAfectada}: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine($"Conexión cerrada para {accion} en {tablaAfectada}");
                }
            }
        }

        //sobrecarga de la funcion con int para saber el id si ya se posee y no es necesario sacarlo con una funcion
        protected async Task<int> ExecuteNonQueryWithAuditAsync(
    string query,
    MySqlParameter[] parameters,
    string accion,
    string tablaAfectada,
    int registroId,
    string detalles = null)
        {
            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine($"Conexión obtenida para {accion} en {tablaAfectada}");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Operación ejecutada - Filas afectadas: {rowsAffected}");

                    if (rowsAffected > 0)
                    {

                        // Registrar auditoría
                        await RegistrarAuditoria(accion, tablaAfectada, registroId, detalles);
                    }

                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar operación con auditoría: {ex.Message}");
                throw new Exception($"Error al ejecutar {accion} en {tablaAfectada}: {ex.Message}", ex);
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine($"Conexión cerrada para {accion} en {tablaAfectada}");
                }
            }
        }

        // Método para registrar la auditoría
        private async Task RegistrarAuditoria(string accion, string tablaAfectada, int? registroId, string detalles)
        {
            DateTime fechaActual = DateTime.Now;
            if (!UserInformation.Instance.IsLoggedIn)
            {
                throw new InvalidOperationException("No hay usuario logueado para registrar la auditoría.");
            }

            string query = @"
        INSERT INTO auditorias (usuario_id, accion, tabla_afectada, registro_afectado, fecha)
        VALUES (@usuarioId, @accion, @tablaAfectada, @registroAfectado, @fechaActual)";

            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                Console.WriteLine($"Conexión obtenida para RegistrarAuditoria - Acción: {accion}, Tabla: {tablaAfectada}");

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@usuarioId", UserInformation.Instance.Id);
                    command.Parameters.AddWithValue("@accion", accion);
                    command.Parameters.AddWithValue("@tablaAfectada", tablaAfectada);
                    command.Parameters.AddWithValue("@registroAfectado", registroId.HasValue ? registroId.ToString() : null);
                    command.Parameters.AddWithValue("@fechaActual", fechaActual);

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Auditoría registrada - Filas afectadas: {rowsAffected}");
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error MySQL al registrar auditoría: {ex.Message}, Código: {ex.Number}, SQL: {query}");
                throw; // Propagar la excepción para ver el detalle completo
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al registrar auditoría: {ex.Message}");
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    _sqlHelper.CerrarConexion(connection);
                    Console.WriteLine("Conexión cerrada en RegistrarAuditoria");
                }
            }
        }

        // Método para consultas sin auditoría (SELECT)
        protected async Task<MySqlDataReader> ExecuteReaderAsync(string query, MySqlParameter[] parameters = null)
        {
            MySqlConnection connection = null;
            try
            {
                connection = _sqlHelper.ObtenerConexion();
                using (var command = new MySqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        await connection.OpenAsync();
                    }

                    return await command.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar consulta: {ex.Message}");
                throw new Exception($"Error al ejecutar consulta: {ex.Message}", ex);
            }
        }
    }
}