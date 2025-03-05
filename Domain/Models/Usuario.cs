using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apos_gestor_caja.Domain.Models
{

    public class Usuario
    {
        public int Id { get; set; } 
        public string Username { get;  set; }
        public string Password { get;  set; }
        public string Nombre { get;  set; }
        public bool   Activo { get;  set; }
    }
}
