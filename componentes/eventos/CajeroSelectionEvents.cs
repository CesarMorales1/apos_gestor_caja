using System;
using apos_gestor_caja.Domain.Models;

namespace apos_gestor_caja.Events
{
    public class CajeroSelectionEventArgs : EventArgs
    {
        public Cajero SelectedCajero { get; }

        public CajeroSelectionEventArgs(Cajero selectedCajero)
        {
            SelectedCajero = selectedCajero;
        }
    }
}
