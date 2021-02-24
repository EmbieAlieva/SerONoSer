using CapaDatos;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tablero
{
    static class Program
    {
        public static GestionCNegocio gest;
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            gest = new GestionCNegocio(out string msj);
            if (!string.IsNullOrWhiteSpace(msj)) MessageBox.Show(msj);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmJuego());
        }
    }
}
