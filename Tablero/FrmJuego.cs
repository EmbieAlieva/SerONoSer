using Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tablero {
    public partial class FrmJuego : Form {

        public FrmJuego() { InitializeComponent(); }
        private void FrmJuego_Load(object sender, EventArgs e) {
            this.Location = new Point(150, 0);
            this.Size = new Size(1030, Screen.PrimaryScreen.WorkingArea.Size.Height);

            tmrTiempoTotal.Interval = 1000;
        }

        int nivel = 1;
        private void btnComenzar_Click(object sender, EventArgs e) { Jugar(nivel); }

        PreguntaDTO preguntaDto;
        int contValido = 0;
        int contNoValido = 0;
        private void btn_Click(object sender, EventArgs e) {
            Button btn = sender as Button;

            if (btn.Text != "") {

                RespuestaDTO respuesta = new RespuestaDTO(btn.Text);             
                respuesta = preguntaDto.Respuestas.Single(res => res.PosibleRespuesta == respuesta.PosibleRespuesta);

                int posi = preguntaDto.Respuestas.IndexOf(respuesta);
                if (preguntaDto.Respuestas[posi].Valida) {
                    btn.BackColor = Color.Green;
                    contValido += 1;
                    btn.Enabled = false;
                } else {
                    btn.BackColor = Color.Red;
                    btn.Enabled = false;
                    contNoValido += 1;
                    lblRespuestaValida.Text = respuesta.ExplicacionErronea.ToString();
                }

                if (contValido == 8) { 
                    AciertosFallos("acertado", 8); 
                } else if (contNoValido == 4) {
                    AciertosFallos("fallado", 4);
                }   
            }         
        }
        private void AciertosFallos(string acierto_fallo, int num) {
            tmrTiempoTotal.Stop();

            DialogResult dr;
            if (num == 0) {
                dr = MessageBox.Show($"Te has quedado sin tiempo para esta pregunta, ¿deseas continuar con otra pregunta? No finalizará el programa.", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            } else if (num == 1) {
                dr = DialogResult.Yes;
            } else {
                dr = MessageBox.Show($"Has {acierto_fallo} las {num} preguntas, ¿desea continuar con otra pregunta?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
                    
            if (dr == DialogResult.Yes) {
                contNoValido = 0;
                contValido = 0;
                nivel += 1;
                tiempo = 10;
                Jugar(nivel);
            }
        }

        int tiempo = 10;
        private void Jugar(int nivel)  {
            preguntaDto = Program.gest.PreguntasNivel(nivel, out String msj);
            if (msj != null) {
                if (msj.Equals("Error")) { return; }
                if (msj.Contains("nivel máximo")) { 
                    VaciarControles(); }              
                MessageBox.Show(msj, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                if (msj.Contains("No disponemos")) { AciertosFallos("noDisponemos", 1); }
                return;
            }

            int num = 12;
            VaciarControles();            

            lblTiempo.Text = tiempo.ToString();
            tmrTiempoTotal.Start();

            lblNivel.Text = preguntaDto.Nivel.ToString();
            lblEnunciado.Text = preguntaDto.Enunciado;
            num = 12;
            Controls.OfType<Button>().ToList().ForEach(button => {
                if (button.Name == "btn" + num) {
                    button.Text = preguntaDto.Respuestas[num - 1].PosibleRespuesta;
                    num -= 1;
                }
            });
            
            // foreach (Control control in frm.Controls) { }
            // for (int i = 0; i < Controls.OfType<Button>().Count(); i++) { }           
            /*foreach (var button in Controls.OfType<Button>())  {
               if (button.Name == "btn"+num) {
                    button.Text = preguntaDto.Respuestas[num-1].PosibleResuesta;
                    num -= 1;
               }
            }*/
        }

        public void VaciarControles() {
            int num = 12;
            Controls.OfType<Button>().ToList().ForEach(button => {
                if (button.Name == "btn" + num) {
                    button.Text = "";
                    button.Enabled = true;
                    button.BackColor = Color.Gainsboro;
                    num -= 1;
                }
            });
            Controls.OfType<Label>().ToList().ForEach(label => { label.Text = ""; });
        }
              
        private void tmrTiempoTotal_Tick(object sender, EventArgs e) {
            tiempo -= 1;
            lblTiempo.Text = tiempo.ToString();
            if (tiempo == 0) {
                tmrTiempoTotal.Stop();
                AciertosFallos("noTiempo", 0);              
            }
        }

        private void btnFinalizar_Click(object sender, EventArgs e) { Close(); }
    }
}