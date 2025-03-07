using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.applicationLayer.interfaces;

namespace apos_gestor_caja.Infrastructure.Repositories
{
    public class TipoDePagoRepository : RepositoryBase, ITipoDePago
    {
        public TipoDePagoRepository() : base()
        {
        }

        public async Task<List<TipoDePagoModel>> ObtenerTodosLosTiposDePago()
        {

            string query = @"SELECT 
                                   d1 AS Abreviacion, 
                                   d2 AS Id, 
                                   d3 AS Nombre 
                             FROM apos07 
                             ORDER BY d1 ASC";

            var tiposDePago = new List<TipoDePagoModel>();

            try
            {
                using (var reader = await ExecuteReaderAsync(query))
                {
                    while (await reader.ReadAsync())
                    {
                        tiposDePago.Add(new TipoDePagoModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Abreviacion = reader.GetString(reader.GetOrdinal("Abreviacion"))
                        });
                    }
                }
                return tiposDePago;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener tipos de pago: {ex.Message}");
                throw new Exception($"Error al obtener tipos de pago: {ex.Message}", ex);
            }
        }


    }
}