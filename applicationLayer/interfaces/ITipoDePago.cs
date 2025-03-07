using apos_gestor_caja.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apos_gestor_caja.applicationLayer.interfaces
{
    interface ITipoDePago
    {
        Task<List<TipoDePagoModel>> ObtenerTodosLosTiposDePago();
    }
}
