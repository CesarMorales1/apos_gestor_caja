using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using apos_gestor_caja;
using apos_gestor_caja.componentes;
using apos_gestor_caja.Infrastructure.Repositories;
using MyApp.Infrastructure.Database;

namespace apos_gestor_caja
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
