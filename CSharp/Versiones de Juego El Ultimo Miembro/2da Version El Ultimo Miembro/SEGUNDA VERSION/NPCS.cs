using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace PROYECTO_5TO___TOTЯ
{
    public class DialogoNPC
    {
        public string NombreNPC { get; set; }
        public string ImagenNPC { get; set; }
        public List<string> Mensajes { get; set; }
        public List<OpcionDialogo> Opciones { get; set; }
        public string TipoNPC { get; set; }

        public DialogoNPC()
        {
            Mensajes = new List<string>();
            Opciones = new List<OpcionDialogo>();
        }
    }

    public class OpcionDialogo
    {
        public string Texto { get; set; }
        public Action Accion { get; set; }
        public Color Color { get; set; }

        public OpcionDialogo(string texto, Action accion, Color? color = null)
        {
            Texto = texto;
            Accion = accion;
            Color = color ?? Color.FromArgb(80, 150, 220);
        }
    }

    public class DIALOGO_NPC : Form
    {
        private DialogoNPC dialogo;
        private Panel panelPrincipal;
        private PictureBox picNPC;
        private Label lblNombreNPC;
        private Panel panelMensajes;
        private FlowLayoutPanel panelOpciones;
        private int indiceMensajeActual = 0;
        private System.Windows.Forms.Timer timerTexto;
        private string textoCompleto = "";
        private string textoMostrado = "";
        private int indiceCaracter = 0;
        private bool textoCompletado = false;

        public DIALOGO_NPC(DialogoNPC dialogoNPC)
        {
            dialogo = dialogoNPC;
            InicializarFormulario();
            CrearInterfaz();
            MostrarMensaje(0);
        }

        private void InicializarFormulario()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(1400, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 20, 25);
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Space)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;

                    if (!textoCompletado)
                    {
                        CompletarTextoInstantaneo();
                    }
                    else
                    {
                        SiguienteMensaje();
                    }
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (textoCompletado)
                        SiguienteMensaje();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
            };

        }

        private void CrearInterfaz()
        {
            // PANEL PRINCIPAL DE DIALOGO
            panelPrincipal = new Panel
            {
                Size = new Size(1350, 650),
                Location = new Point(25, 25),
                BackColor = Color.FromArgb( 15, 18, 25)
            };
            this.Controls.Add(panelPrincipal);
            RedondearPanel(panelPrincipal, 30);

            // BORDE SUPERIOR DECORATIVO
            Panel bordeSuperior = new Panel
            {
                Size = new Size(1350, 5),
                Location = new Point(0, 0),
                BackColor = ObtenerColorPorTipo(dialogo.TipoNPC)
            };
            panelPrincipal.Controls.Add(bordeSuperior);
            RedondearPanel(bordeSuperior, 30);

            // ------------------------------------------------------- SECCION IZQUIERDA 
            Panel panelIzquierdo = new Panel
            {
                Size = new Size(450, 650),
                Location = new Point(0, 5),
                BackColor = Color.FromArgb( 10, 13, 20)
            };
            panelPrincipal.Controls.Add(panelIzquierdo);

            // IMAGEN DEL NPC
            picNPC = new PictureBox
            {
                Size = new Size(400, 500),
                Location = new Point(25, 80),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            panelIzquierdo.Controls.Add(picNPC);
            CargarImagenNPC();

            // NOMBRE DEL NPC CON FONDO
            Panel panelNombre = new Panel
            {
                Size = new Size(400, 60),
                Location = new Point(25, 10),
                BackColor = Color.FromArgb( 20, 25, 35)
            };
            panelIzquierdo.Controls.Add(panelNombre);
            RedondearPanel(panelNombre, 15);

            lblNombreNPC = new Label
            {
                Text = dialogo.NombreNPC.ToUpper(),
                Font = FUENTE.ObtenerFont(24),
                ForeColor = ObtenerColorPorTipo(dialogo.TipoNPC),
                AutoSize = false,
                Size = new Size(380, 60),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(10, 0)
            };
            panelNombre.Controls.Add(lblNombreNPC);

            // ETIQUETA DE TIPO
            Label lblTipo = new Label
            {
                Text = ObtenerEtiquetaTipo(dialogo.TipoNPC),
                Font = FUENTE.ObtenerFont(12),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(400, 30),
                TextAlign = ContentAlignment.TopCenter,
                Location = new Point(25, 590)
            };
            panelIzquierdo.Controls.Add(lblTipo);

            // ------------------------------------------------------- SECCION DERECHA
            Panel panelDerecho = new Panel
            {
                Size = new Size(895, 645),
                Location = new Point(452, 5),
                BackColor = Color.FromArgb(10, 13, 20)
            };
            panelPrincipal.Controls.Add(panelDerecho);

            // PANEL DE MENSAJES (CUADRO DE TEXTO)
            panelMensajes = new Panel
            {
                Size = new Size(870, 450),
                Location = new Point(10, 10),
                BackColor = Color.FromArgb( 20, 25, 35),
                AutoScroll = false
            };
            panelDerecho.Controls.Add(panelMensajes);
            RedondearPanel(panelMensajes, 15);

            // PANEL DE OPCIONES (BOTONES)
            panelOpciones = new FlowLayoutPanel
            {
                Size = new Size(870, 170),
                Location = new Point(10, 470),
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = false
            };
            panelDerecho.Controls.Add(panelOpciones);

            // BOTON CERRAR
            Button btnCerrar = new Button
            {
                Text = "✕",
                Font = FUENTE.ObtenerFont(20),
                Size = new Size(40, 40),
                Location = new Point(1295, 10),
                BackColor = Color.FromArgb(80, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            RedondearBoton(btnCerrar, 10);
            btnCerrar.Click += (s, e) => this.Close();
            btnCerrar.MouseEnter += (s, e) => btnCerrar.BackColor = Color.FromArgb(120, 60, 60);
            btnCerrar.MouseLeave += (s, e) => btnCerrar.BackColor = Color.FromArgb(80, 50, 50);
            panelPrincipal.Controls.Add(btnCerrar);

            // INDICADOR DE CONTINUACION
            Label lblContinuar = new Label
            {
                Text = "▼ PRESIONA ESPACIO PARA CONTINUAR",
                Font = FUENTE.ObtenerFont(11),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(870, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(10, 640),
                Visible = false,
                Name = "lblContinuar"
            };
            panelDerecho.Controls.Add(lblContinuar);

            // ANIMACION DE PARPADEO
            System.Windows.Forms.Timer parpadeoTimer = new System.Windows.Forms.Timer { Interval = 500 };
            parpadeoTimer.Tick += (s, e) =>
            {
                if (lblContinuar.Visible)
                    lblContinuar.ForeColor = lblContinuar.ForeColor == Color.Gray ? Color.White : Color.Gray;
            };
            parpadeoTimer.Start();
        }

        private void CargarImagenNPC()
        {
            string rutaBase = Path.Combine(Application.StartupPath, "Resources");
            string ruta = Path.Combine(rutaBase, dialogo.ImagenNPC);

            if (!File.Exists(ruta))
                ruta = Path.Combine(rutaBase, "NPC PANTALLA.PNG");

            if (File.Exists(ruta))
            {
                try
                {
                    using (var fs = new FileStream(ruta, FileMode.Open, FileAccess.Read))
                    using (var imgTemp = Image.FromStream(fs))
                    {
                        Bitmap bmp = new Bitmap(400, 500);
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(imgTemp, 0, 0, 400, 500);
                        }
                        picNPC.Image = bmp;
                    }
                }
                catch
                {
                    picNPC.BackColor = Color.FromArgb(50, 50, 60);
                }
            }
        }

        private void MostrarMensaje(int indice)
        {
            if (indice >= dialogo.Mensajes.Count)
            {
                MostrarOpciones();
                return;
            }

            indiceMensajeActual = indice;
            textoCompleto = dialogo.Mensajes[indice];
            textoMostrado = "";
            indiceCaracter = 0;
            textoCompletado = false;

            panelMensajes.Controls.Clear();

            Label lblTexto = new Label
            {
                Text = "",
                Font = FUENTE.ObtenerFont(18),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(830, 410),
                Location = new Point(20, 20),
                BackColor = Color.Transparent,
                Name = "lblTextoDialogo"
            };
            panelMensajes.Controls.Add(lblTexto);

            var lblContinuar = panelPrincipal.Controls.Find("lblContinuar", true);
            if (lblContinuar.Length > 0)
                lblContinuar[0].Visible = false;

            timerTexto = new System.Windows.Forms.Timer { Interval = 30 };
            timerTexto.Tick += (s, e) =>
            {
                if (indiceCaracter < textoCompleto.Length)
                {
                    textoMostrado += textoCompleto[indiceCaracter];
                    lblTexto.Text = textoMostrado;
                    indiceCaracter++;
                }
                else
                {
                    timerTexto.Stop();
                    textoCompletado = true;

                    if (indiceMensajeActual < dialogo.Mensajes.Count - 1)
                    {
                        var lblCont = panelPrincipal.Controls.Find("lblContinuar", true);
                        if (lblCont.Length > 0)
                            lblCont[0].Visible = true;
                    }
                    else
                    {
                        MostrarOpciones();
                    }
                }
            };
            timerTexto.Start();
        }

        private void CompletarTextoInstantaneo()
        {
            if (timerTexto != null)
                timerTexto.Stop();

            var lblTexto = panelMensajes.Controls.Find("lblTextoDialogo", true);
            if (lblTexto.Length > 0)
            {
                lblTexto[0].Text = textoCompleto;
                textoCompletado = true;

                if (indiceMensajeActual < dialogo.Mensajes.Count - 1)
                {
                    var lblCont = panelPrincipal.Controls.Find("lblContinuar", true);
                    if (lblCont.Length > 0)
                        lblCont[0].Visible = true;
                }
                else
                {
                    MostrarOpciones();
                }
            }
        }
        private void SiguienteMensaje()
        {
            if (indiceMensajeActual < dialogo.Mensajes.Count - 1)
            {
                MostrarMensaje(indiceMensajeActual + 1);
            }
        }

        private void MostrarOpciones()
        {
            panelOpciones.Controls.Clear();

            var lblContinuar = panelPrincipal.Controls.Find("lblContinuar", true);
            if (lblContinuar.Length > 0)
                lblContinuar[0].Visible = false;

            if (dialogo.Opciones.Count == 0)
            {
                Button btnCerrarDefault = CrearBotonOpcion("Adiós", () => this.Close(), Color.FromArgb(80, 80, 100));
                panelOpciones.Controls.Add(btnCerrarDefault);
            }
            else
            {
                foreach (var opcion in dialogo.Opciones)
                {
                    Button btnOpcion = CrearBotonOpcion(opcion.Texto, opcion.Accion, opcion.Color);
                    panelOpciones.Controls.Add(btnOpcion);
                }
            }
        }

        private Button CrearBotonOpcion(string texto, Action accion, Color color)
        {
            Button btn = new Button
            {
                Text = texto,
                Size = new Size(850, 50),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = FUENTE.ObtenerFont(16),
                Cursor = Cursors.Hand,
                Margin = new Padding(5),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            RedondearBoton(btn, 10);

            Color colorOriginal = color;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(
                Math.Min(255, colorOriginal.R + 30),
                Math.Min(255, colorOriginal.G + 30),
                Math.Min(255, colorOriginal.B + 30)
            );
            btn.MouseLeave += (s, e) => btn.BackColor = colorOriginal;

            btn.Click += (s, e) =>
            {
                accion?.Invoke();
                this.Close();
            };

            return btn;
        }

        private Color ObtenerColorPorTipo(string tipo)
        {
            return tipo switch
            {
                "quest" => Color.FromArgb(255, 200, 80),
                "tienda" => Color.FromArgb(100, 220, 100),
                "info" => Color.FromArgb(100, 150, 255),
                "enemigo" => Color.FromArgb(220, 80, 80),
                _ => Color.FromArgb(180, 180, 180)
            };
        }

        private string ObtenerEtiquetaTipo(string tipo)
        {
            return tipo switch
            {
                "quest" => "⭐ DADOR DE MISIONES",
                "tienda" => "💰 COMERCIANTE",
                "info" => "💬 HABITANTE",
                "enemigo" => "⚔ ENEMIGO",
                _ => "👤 NPC"
            };
        }

        private void RedondearPanel(Panel panel, int radio)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(panel.Width - radio, 0, radio, radio, 270, 90);
            path.AddArc(panel.Width - radio, panel.Height - radio, radio, radio, 0, 90);
            path.AddArc(0, panel.Height - radio, radio, radio, 90, 90);
            path.CloseFigure();
            panel.Region = new Region(path);
        }

        private void RedondearBoton(Button boton, int radio)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(boton.Width - radio, 0, radio, radio, 270, 90);
            path.AddArc(boton.Width - radio, boton.Height - radio, radio, radio, 0, 90);
            path.AddArc(0, boton.Height - radio, radio, radio, 90, 90);
            path.CloseFigure();
            boton.Region = new Region(path);
        }
    }
}