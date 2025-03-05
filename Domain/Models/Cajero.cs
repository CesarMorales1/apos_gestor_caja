using System;

namespace apos_gestor_caja.Domain.Models
{
    public class Cajero
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public int NivelAcceso { get; set; }
        public int Barra { get; set; }
        public bool Activo { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Usuario} - {Nombre}";
        }
    }
}