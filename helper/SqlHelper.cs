using System;
using MySql.Data.MySqlClient;

namespace MyApp.Infrastructure.Database
{
    public class SqlHelper
    {
        private string cadenaConexion = "Server=localhost;Database=geobit;Uid=root;Pwd=asd.223211;";

        public SqlHelper()
        {
            // No necesitamos una conexión estática aquí
        }

        public MySqlConnection ObtenerConexion()
        {
            var connection = new MySqlConnection(cadenaConexion);
            try
            {
                connection.Open();
                Console.WriteLine("Conexión abierta exitosamente.");
                return connection;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error al abrir la conexión: {ex.Message}");
                throw;
            }
        }

        public void CerrarConexion(MySqlConnection connection)
        {
            try
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Conexión cerrada exitosamente.");
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error al cerrar la conexión: {ex.Message}");
            }
        }
    }
}