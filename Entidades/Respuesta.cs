using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Respuesta
    {
        public int NumPregunta { get; set; }
        public int NumRespuesta { get; set; }
        public String PosibleRespuesta { get; set; }
        public bool Valida { get; set; }

        public Respuesta() { }

        public Respuesta(int numPregunta, int numRespuesta, string posibleResuesta, bool valida)
        {
            NumPregunta = numPregunta;
            NumRespuesta = numRespuesta;
            PosibleRespuesta = posibleResuesta;
            Valida = valida;
        }

        public override bool Equals(object obj)
        {
            return obj is Respuesta respuesta &&
                   NumPregunta == respuesta.NumPregunta &&
                   NumRespuesta == respuesta.NumRespuesta;
        }

        public override int GetHashCode()
        {
            int hashCode = 2132540932;
            hashCode = hashCode * -1521134295 + NumPregunta.GetHashCode();
            hashCode = hashCode * -1521134295 + NumRespuesta.GetHashCode();
            return hashCode;
        }
    }
}
