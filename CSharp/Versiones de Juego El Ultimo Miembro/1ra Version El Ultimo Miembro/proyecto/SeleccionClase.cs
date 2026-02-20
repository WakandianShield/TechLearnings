using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace proyecto
{
    public class SeleccionDeClaseForm : Form
    {
        private string razaSeleccionada;
        private string subrazaSeleccionada;
        private string claseSeleccionada = "";
        private PictureBox previewPersonaje;
        private Label lblTitulo;
        private Label lblNombreClase;
        private Button btnConfirmar;
        private Button btnIzquierda, btnDerecha;

        private string[] clases;
        private int indiceClase = 0;
        private string[] rutasClases;

        private readonly int anchoPantalla = Screen.PrimaryScreen.Bounds.Width;
        private readonly int altoPantalla = Screen.PrimaryScreen.Bounds.Height;

        public SeleccionDeClaseForm(string raza, string subraza = null)
        {
            razaSeleccionada = raza.ToUpper();
            subrazaSeleccionada = subraza?.ToUpper();

            InicializarFormulario();
            Fuente.CargarFuente();
            ConfigurarFondo();
            CrearTitulo();
            CrearPreviewPersonaje();
            CrearFlechasCarrusel();
            CrearNombreClase();
            CrearBotonConfirmar();
            CrearBotonVolver();
            InicializarClases();
        }

        #region Inicialización
        private void InicializarFormulario()
        {
            Text = "SELECCION DE CLASE";
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
        }

        private void ConfigurarFondo()
        {
            string rutaFondo = Path.Combine(Application.StartupPath, "Resources", "fondoblur.png");
            if (File.Exists(rutaFondo))
                BackgroundImage = Image.FromFile(rutaFondo);
            BackgroundImageLayout = ImageLayout.Stretch;
        }
        #endregion

        #region Controles
        private void CrearTitulo()
        {
            lblTitulo = new Label()
            {
                Text = "SELECCIONAR CLASE",
                Font = Fuente.ObtenerFont(40),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblTitulo.Left = (anchoPantalla - lblTitulo.PreferredWidth) / 2;
            lblTitulo.Top = 20;
            Controls.Add(lblTitulo);
        }

        private void CrearPreviewPersonaje()
        {
            previewPersonaje = new PictureBox()
            {
                Width = 300,
                Height = 500,
                Left = (anchoPantalla - 270) / 2,
                Top = 225,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            Controls.Add(previewPersonaje);
        }

        private void CrearFlechasCarrusel()
        {
            btnIzquierda = new Button()
            {
                Text = "<",
                Font = Fuente.ObtenerFont(40),
                Width = 100,
                Height = 100,
                Left = 50,
                Top = (altoPantalla - 100) / 2,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            btnIzquierda.FlatAppearance.BorderSize = 0;
            btnIzquierda.Click += (s, e) => CambiarClase(-1);
            Controls.Add(btnIzquierda);

            btnDerecha = new Button()
            {
                Text = ">",
                Font = Fuente.ObtenerFont(40),
                Width = 100,
                Height = 100,
                Left = anchoPantalla - 150,
                Top = (altoPantalla - 100) / 2,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            btnDerecha.FlatAppearance.BorderSize = 0;
            btnDerecha.Click += (s, e) => CambiarClase(1);
            Controls.Add(btnDerecha);
        }

        private void CrearNombreClase()
        {
            lblNombreClase = new Label()
            {
                Font = Fuente.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true
            };
            lblNombreClase.Top = 750;
            lblNombreClase.Left = (anchoPantalla - lblNombreClase.Width) / 2;
            Controls.Add(lblNombreClase);
        }

        private void CrearBotonConfirmar()
        {
            btnConfirmar = new Button()
            {
                Text = "CONFIRMAR",
                Width = 250,
                Height = 100,
                Left = (anchoPantalla - 250) / 2,
                Top = 820,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = Fuente.ObtenerFont(20)
            };
            btnConfirmar.FlatAppearance.BorderSize = 0;


            int radio = 25;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(btnConfirmar.Width - radio, 0, radio, radio, 270, 90);
            path.AddArc(btnConfirmar.Width - radio, btnConfirmar.Height - radio, radio, radio, 0, 90);
            path.AddArc(0, btnConfirmar.Height - radio, radio, radio, 90, 90);
            path.CloseAllFigures();
            btnConfirmar.Region = new Region(path);

            AplicarDegradadoOscuroHover(btnConfirmar, Color.Purple, Color.MediumPurple);

            btnConfirmar.Click += Confirmar_Click;
            Controls.Add(btnConfirmar);
        }

        private void CrearBotonVolver()
        {
            Button btnVolver = new Button()
            {
                Text = "VOLVER",
                Width = 150,
                Height = 40,
                Left = 10,
                Top = 10,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = Fuente.ObtenerFont(14),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnVolver.FlatAppearance.BorderSize = 0;
            AplicarDegradadoOscuroHover(btnVolver, Color.Purple, Color.MediumPurple);

            btnVolver.Click += (s, e) =>
            {
                if (subrazaSeleccionada != null)
                {
                    var formSubraza = new SeleccionDeSubrazaForm(razaSeleccionada);
                    formSubraza.Show();
                }
                else
                {
                    var formRaza = new SeleccionDeRazaForm();
                    formRaza.Show();
                }
                this.Hide();
            };

            int radio = 25;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(btnVolver.Width - radio, 0, radio, radio, 270, 90);
            path.AddArc(btnVolver.Width - radio, btnConfirmar.Height - radio, radio, radio, 0, 90);
            path.AddArc(0, btnVolver.Height - radio, radio, radio, 90, 90);
            path.CloseAllFigures();
            btnVolver.Region = new Region(path);

            Controls.Add(btnVolver);
        }
        #endregion

        #region Lógica Carrusel
        private void InicializarClases()
        {
            if (razaSeleccionada == "HUMANO")
                clases = new[] { "GUERRERO", "MAGO", "CLERIGO", "PICARO", "BARBARO" };
            else if (razaSeleccionada == "ORCO")
                clases = new[] { "GUERRERO", "BARBARO", "PICARO" };
            else if (razaSeleccionada == "ELFO")
            {
                clases = subrazaSeleccionada switch
                {
                    "ALTO ELFO" => new[] { "MAGO", "EXPLORADOR" },
                    "ELFO SILVANO" => new[] { "EXPLORADOR", "PICARO", "GUARDABOSQUES" },
                    "ELFO NEGRO" => new[] { "MAGO", "PICARO", "BRUJO" },
                    _ => new string[0]
                };
            }
            else if (razaSeleccionada == "ENANO")
            {
                clases = subrazaSeleccionada switch
                {
                    "ENANO DE LA COLINA" => new[] { "GUERRERO", "CLERIGO" },
                    "ENANO DE LA MONTAÑA" => new[] { "GUERRERO", "CLERIGO", "ARTIFICE" },
                    _ => new string[0]
                };
            }
            else
                clases = new[] { "AVENTURERO" };


            // ESTO ES PA LA FOTOS, YO LO USARE DPS PARA SUBIR LAS FOTOS BIEN DE NUEVO
            rutasClases = new string[clases.Length];
            for (int i = 0; i < clases.Length; i++)
            {
                if (subrazaSeleccionada != null) 
                    rutasClases[i] = Path.Combine(Application.StartupPath, "Resources", $"{subrazaSeleccionada} {clases[i]}.png");
                else 
                    rutasClases[i] = Path.Combine(Application.StartupPath, "Resources", $"{razaSeleccionada} {clases[i]}.png");
            }

            MostrarClaseActual();
        }

        private void CambiarClase(int dir)
        {
            indiceClase += dir;
            if (indiceClase < 0) indiceClase = clases.Length - 1;
            if (indiceClase >= clases.Length) indiceClase = 0;

            MostrarClaseActual();
        }

        private void MostrarClaseActual()
        {
            lblNombreClase.Text = clases[indiceClase];
            lblNombreClase.Left = (anchoPantalla - lblNombreClase.PreferredWidth) / 2;

            claseSeleccionada = clases[indiceClase];

            if (previewPersonaje.Image != null)
            {
                previewPersonaje.Image.Dispose();
                previewPersonaje.Image = null;
            }

            string ruta = rutasClases[indiceClase];
            if (File.Exists(ruta))
                previewPersonaje.Image = Image.FromFile(ruta);
        }
        #endregion

        #region Confirmar
        private void Confirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(claseSeleccionada))
            {
                MessageBox.Show("Debes elegir una clase antes de continuar.");
                return;
            }

            GameData.PersonajeClase = claseSeleccionada;

            var trasfondoForm = new SeleccionDeTrasfondoForm(this);
            trasfondoForm.Show();
            this.Hide();
        }
        #endregion

        #region Estilo botones
        private void AplicarDegradadoOscuroHover(Button btn, Color colorInicio, Color colorFin)
        {
            Color inicioActual = colorInicio;
            Color finActual = colorFin;

            btn.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(btn.ClientRectangle, inicioActual, finActual, 90f))
                    e.Graphics.FillRectangle(brush, btn.ClientRectangle);
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, btn.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            btn.MouseEnter += (s, e) =>
            {
                inicioActual = ControlPaint.Dark(colorInicio, 0.2f);
                finActual = ControlPaint.Dark(colorFin, 0.2f);
                btn.Invalidate();
            };
            btn.MouseLeave += (s, e) =>
            {
                inicioActual = colorInicio;
                finActual = colorFin;
                btn.Invalidate();
            };
        }
        #endregion
    }
}
