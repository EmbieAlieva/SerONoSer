using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class GestionCNegocio {

        DatosDSet datosDSet;
        bool error = false;
        public GestionCNegocio(out string msj) {
            datosDSet = new DatosDSet(out String mensaje);
            if (!string.IsNullOrWhiteSpace(mensaje)) {
                error = true;
                msj = mensaje;
                return;
            }
            msj = "";
        }

        public PreguntaDTO PreguntasNivel(int nivel, out String mensaje) {
            Random rnd = new Random();
            
            List<PreguntaDTO> preguntasList = datosDSet.PreguntasDeNivel(nivel, out String msj);
            if (!msj.Equals(""))  {              
                mensaje = msj;
                return null;
            }
            if (error == true) {
                mensaje = "Error";
                return null;
            }

            int numPreg = rnd.Next(preguntasList.Count());
            PreguntaDTO pregunta = preguntasList[numPreg];

            mensaje = null;
            return pregunta;
        }
    }
}
