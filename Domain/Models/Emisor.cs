using System;

namespace apos_gestor_caja.Domain.Models
{
    public class Emisor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}
