using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class PreguntaDTO
    {

        public int NumPregunta { get; set; }
        public String Enunciado { get; set; }
        public int Nivel { get; set; }
        public List<RespuestaDTO> Respuestas { get; set; }

        public PreguntaDTO() { }

        public PreguntaDTO(int numPregunta, string enunciado, int nivel, List<RespuestaDTO> respuestas)
        {
            NumPregunta = numPregunta;
            Enunciado = enunciado;
            Nivel = nivel;
            Respuestas = respuestas;
        }
    }
}
