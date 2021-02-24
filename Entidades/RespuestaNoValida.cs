using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    class RespuestaNoValida
    {
        public int NumPregunta { get; set; }
        public int NumRespuesta { get; set; }
        public String Explicacion { get; set;  }

        public RespuestaNoValida() { }

        public RespuestaNoValida(int numPregunta, int numRespuesta, string explicacion)
        {
            NumPregunta = numPregunta;
            NumRespuesta = numRespuesta;
            Explicacion = explicacion;
        }

        public override bool Equals(object obj)
        {
            return obj is RespuestaNoValida valida &&
                   NumPregunta == valida.NumPregunta &&
                   NumRespuesta == valida.NumRespuesta;
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
