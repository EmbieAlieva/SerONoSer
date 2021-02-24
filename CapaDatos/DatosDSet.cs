using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos.DSSerNoSerTableAdapters;
using Entidades;

namespace CapaDatos
{
    public class DatosDSet
    {
        DSSerNoSer dsSerONoSer = new DSSerNoSer();

        public DatosDSet(out string msj) { 
            msj = LlenarTablas();
            ListaPreguntasDTO(out string msjPreg);
            if (msjPreg != "") { msj = msjPreg; }
            ComprobacionErrores(out string mensaje);
            if (mensaje != "") { msj = mensaje; }
        }      

        private string LlenarTablas() {
            // pone un using al principio de la pág y es más fácil
            PreguntasTableAdapter daPreguntas = new PreguntasTableAdapter();
            RespuestasTableAdapter daRespuestas = new RespuestasTableAdapter();
            RespNoValidasTableAdapter daResNoValidas = new RespNoValidasTableAdapter();

            // Llenamos las tablas del DataSet con la info de la BD filtrada por el adaptador
            // Asegurarse de que los datos se puedan traer
            try {
                daPreguntas.Fill(dsSerONoSer.Preguntas);
                daRespuestas.Fill(dsSerONoSer.Respuestas);
                daResNoValidas.Fill(dsSerONoSer.RespNoValidas);
            } catch (Exception e) {
                return $" No se puede conectar con la base de datos 'Ser o no ser'.\n{e.Message}";
            }
            return "";
        }

        private List<PreguntaDTO> ListaPreguntasDTO(out string msj) {

            List<PreguntaDTO> preguntasList = dsSerONoSer.Preguntas.Select(drPregunta => new PreguntaDTO
            {NumPregunta = drPregunta.NumPregunta, Enunciado = drPregunta.Enunciado, Nivel = drPregunta.Nivel, Respuestas = ListaRespuestasDTO(drPregunta.NumPregunta) }).ToList();         

            msj = "";
            return preguntasList;
        }

        private List<PreguntaDTO> ComprobacionErrores(out string msj) {
            List<PreguntaDTO> preguntasList = dsSerONoSer.Preguntas.Select(drPregunta => new PreguntaDTO
            { NumPregunta = drPregunta.NumPregunta, 
                Enunciado = drPregunta.Enunciado, 
                Nivel = drPregunta.Nivel, 
                Respuestas = ListaRespuestasDTO(drPregunta.NumPregunta) }).ToList();


            /* Que no tenga preguntas */
            if (preguntasList == null)
            {
                msj = "La lista de preguntas está a null. Contacte con su administrador.";
                return null;
            }
            string msjError = "";


            /* Que teniendo preguntas haya algunas que no tengan 12 respuestas (indicando los números de las que sean). */
            List<PreguntaDTO> pregsSinRespuesta = preguntasList.Where(preg => preg.Respuestas.Count() != 12).ToList();
            if (pregsSinRespuesta.Count() != 0)
            {
                msjError = "La pregunta: ";
                foreach (var preg in pregsSinRespuesta) { msjError += $"{ preg.NumPregunta } "; }
                msjError += $"no tiene 12 respuestas.\n";
            }


            /* Que hay preguntas que no tengan la relación correcta de 8 válidas y 4 erróneas. */
            string numDePregsNoValidas = "";
            foreach (var pregunta in preguntasList) {
                int prgValidos = pregunta.Respuestas.Count(preg => preg.Valida == true);
                int prgNoValidos = pregunta.Respuestas.Count(preg => preg.Valida == false);

                if ((prgValidos != 8) && (prgNoValidos != 4)) numDePregsNoValidas += $"{pregunta.NumPregunta} ";
            }
            if (numDePregsNoValidas != "") {
                msjError += $"Las siguientes preguntas no tienen 8 respuestas válidas y 4 erróneas: {numDePregsNoValidas}\n";
            }
            //List<int> numDePregsNoValidas2 = preguntasList.Where(preg => preg.Respuestas.Count(p => p.Valida == true) != 8 ?  null : preg.NumPregunta).toList();


            /* Que no hay preguntas de un nivel inferior al máximo */
            int nivelMax = preguntasList.Max(preg => preg.Nivel);
            List<int> nivelesTengo = preguntasList.Select(preg => preg.Nivel).Distinct().ToList();

            if (nivelesTengo.Count() != nivelMax) {
                msjError += $"El nivel máximo es {nivelMax} pero no hay preguntas en el nivel o niveles: ";
                for (int i = 0; i < nivelMax; i++) {
                    if (!nivelesTengo.Contains(i + 1)) { msjError += $"{i + 1} "; }
                }

                msjError += "\n\nPongase en contacto con su Administrador!";
            }

            if (msjError != "") {
                msj = msjError;
                return null;
            }

            msj = "";
            return preguntasList;
        }

        private List<RespuestaDTO> ListaRespuestasDTO(int numPreg) {

            List<RespuestaDTO> respuestasDTO = dsSerONoSer.Respuestas.Where(drResp => drResp.NumPregunta == numPreg)
                .Select(drResp => new RespuestaDTO
                (drResp.NumPregunta, drResp.NumRespuesta, drResp.PosibleRespuesta, drResp.Valida, dsSerONoSer.RespNoValidas.Where(drRnv => drRnv.NumPregunta.Equals(numPreg) && drRnv.NumRespuesta.Equals(drResp.NumRespuesta)).Select(drRnv => drRnv.Explicacion).FirstOrDefault())).ToList();

            respuestasDTO = respuestasDTO.Select(resp => new RespuestaDTO(resp.NumPregunta, resp.NumRespuesta, resp.PosibleRespuesta, resp.Valida, resp.ExplicacionErronea == null ? "Errónea, aunque no sabemos el motivo, debes investigarlo" : resp.ExplicacionErronea)).ToList();
            return respuestasDTO;
        }

        public List<PreguntaDTO> PreguntasDeNivel(int nivel, out string msj) {
            int nivelMax = dsSerONoSer.Preguntas.Max(drPreg => drPreg.Nivel);
            if (nivel > nivelMax) {
                msj = "El nivel máximo del que disponemos es el 4. Discuple las molestias";
                return null;
            } else if (nivel <= 0) {
                msj = "El nivel mímimo del que disponemos es el 1. Discuple las molestias";
                return null;
            }

            var drPreguntasDtoNivel = dsSerONoSer.Preguntas.Where(drPreg => drPreg.Nivel == nivel).ToList();
            // pordía dejarlo como PreguntasRow o podría pasarlo a lista con un Select
            if (drPreguntasDtoNivel.Count() == 0) {
                msj = $"No disponemos del nivel {nivel} pero le pasamos al nivel 3.\nDisculpe las molestias";
                return null;
            }
            var preguntasDtoNivel = (from drPreg in drPreguntasDtoNivel
                     select new PreguntaDTO { NumPregunta = drPreg.NumPregunta, Enunciado = drPreg.Enunciado, Nivel = drPreg.Nivel, Respuestas = ListaRespuestasDTO(drPreg.NumPregunta) }).ToList();
            msj = "";
            /*RespuestasPregunta(drPreg)}*/
            return preguntasDtoNivel;
        }

        /*List<RespuestaDTO>RespuestasPregunta(DSSerNoSer.PreguntasRow drPreg)
        {
            List<RespuestaDTO> respuestasDTO = drPreg.GetRespuestasRows().Select(preg => new RespuestaDTO { NumPregunta = preg.NumPregunta, NumRespuesta = preg.NumRespuesta, PosibleResuesta = preg.PosibleRespuesta, Valida = preg.Valida, ExplicacionErronea = preg.GetRespNoValidasRows().Where(resp => resp.NumPregunta == preg.NumPregunta).Select(respNV => new {respNV.Explicacion }).ToString() }).ToList();
           
                //.Where(preg => preg.NumPregunta == drPreg.NumPregunta)
            return null;
        }*/
    }
   
}
