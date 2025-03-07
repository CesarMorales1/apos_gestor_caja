using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using apos_gestor_caja.Domain.Models;
using apos_gestor_caja.applicationLayer.interfaces;
using apos_gestor_caja.Infrastructure.Repositories;

namespace apos_gestor_caja.ApplicationLayer.Services
{
    public class TipoDePagoService : ITipoDePago
    {
        private readonly TipoDePagoRepository _tipoDePagoRepository;

        public TipoDePagoService(TipoDePagoRepository tipoDePagoRepository)
        {
            _tipoDePagoRepository = tipoDePagoRepository ?? throw new ArgumentNullException(nameof(tipoDePagoRepository));
        }

        public async Task<List<TipoDePagoModel>> ObtenerTodosLosTiposDePago()
        {
            Console.WriteLine("Obteniendo todos los tipos de pago");
            var tiposDePago = await _tipoDePagoRepository.ObtenerTodosLosTiposDePago();
            Console.WriteLine($"Tipos de pago obtenidos - Total: {tiposDePago.Count}");
            return tiposDePago;
        }



    }
}