using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class RespuestaDTO
    {
        public int NumPregunta { get; set; }
        public int NumRespuesta { get; set; }
        public String PosibleRespuesta { get; set; }
        public bool Valida { get; set; }
        public String ExplicacionErronea { get; set; }

        public RespuestaDTO() { }

        public RespuestaDTO(string posibleRespuesta) { PosibleRespuesta = posibleRespuesta; }

        public RespuestaDTO(int numPregunta, int numRespuesta, string posibleRespuesta, bool valida, string explicacionErronea)
        {
            NumPregunta = numPregunta;
            NumRespuesta = numRespuesta;
            PosibleRespuesta = posibleRespuesta;
            Valida = valida;
            ExplicacionErronea = explicacionErronea;
        }
    }
}
