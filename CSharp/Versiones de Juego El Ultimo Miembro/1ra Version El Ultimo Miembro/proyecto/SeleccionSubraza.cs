using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace proyecto
{
    public class SeleccionDeSubrazaForm : Form
    {
        private Button btnConfirmar;
        private PictureBox previewPersonaje;
        private Label lblTitulo;
        private Button btnIzquierda, btnDerecha;
        private Label lblNombreSubraza;

        private string razaSeleccionada;
        private string subrazaSeleccionada = "";

        private string[] subrazas;
        private string[] rutasSubrazas;
        private int indiceSubraza = 0;

        private readonly int anchoPantalla = Screen.PrimaryScreen.Bounds.Width;
        private readonly int altoPantalla = Screen.PrimaryScreen.Bounds.Height;

        public SeleccionDeSubrazaForm(string raza)
        {
            razaSeleccionada = raza.ToUpper();

            InicializarFormulario();
            CargarFuente();
            ConfigurarFondo();
            CrearTitulo();
            CrearPreviewPersonaje();
            CrearFlechasCarrusel();
            CrearNombrePersonaje();
            CrearBotonConfirmar();
            CrearBotonVolver();
            InicializarSubrazas();
        }

        #region Inicialización
        private void InicializarFormulario()
        {
            Text = "SELECCION DE SUBRAZA";
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
        }

        private void CargarFuente() => Fuente.CargarFuente();

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
                Text = "SELECCIONAR SUBRAZA",
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
                Width = 270,
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
            btnIzquierda.Click += (s, e) => CambiarSubraza(-1);
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
            btnDerecha.Click += (s, e) => CambiarSubraza(1);
            Controls.Add(btnDerecha);
        }

        private void CrearNombrePersonaje()
        {
            lblNombreSubraza = new Label()
            {
                Font = Fuente.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true
            };
            lblNombreSubraza.Top = 750;
            lblNombreSubraza.Left = (anchoPantalla - lblNombreSubraza.Width) / 2;
            Controls.Add(lblNombreSubraza);
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
                var formRaza = new SeleccionDeRazaForm();
                formRaza.Show();
                this.Hide();
            };

            btnVolver.BringToFront();
            Controls.Add(btnVolver);

            int radio = 25;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(btnVolver.Width - radio, 0, radio, radio, 270, 90);
            path.AddArc(btnVolver.Width - radio, btnConfirmar.Height - radio, radio, radio, 0, 90);
            path.AddArc(0, btnVolver.Height - radio, radio, radio, 90, 90);
            path.CloseAllFigures();
            btnVolver.Region = new Region(path);
        }
        #endregion

        #region Lógica Carrusel
        private void InicializarSubrazas()
        {
            Dictionary<string, string[]> subrazasPorRaza = new Dictionary<string, string[]>()
            {
                {"ELFO", new[] { "ALTO ELFO", "ELFO SILVANO", "ELFO NEGRO" } },
                {"ENANO", new[] { "ENANO DE LA COLINA", "ENANO DE LA MONTAÑA" } },
            };

            if (!subrazasPorRaza.ContainsKey(razaSeleccionada))
                subrazas = new string[] { "DEFAULT" };
            else
                subrazas = subrazasPorRaza[razaSeleccionada];

            rutasSubrazas = new string[subrazas.Length];

            for (int i = 0; i < subrazas.Length; i++)
                rutasSubrazas[i] = Path.Combine(Application.StartupPath, "Resources", $"{subrazas[i]}.png");

            MostrarSubrazaActual();
        }

        private void CambiarSubraza(int dir)
        {
            indiceSubraza += dir;
            if (indiceSubraza < 0) indiceSubraza = subrazas.Length - 1;
            if (indiceSubraza >= subrazas.Length) indiceSubraza = 0;

            MostrarSubrazaActual();
        }

        private void MostrarSubrazaActual()
        {
           
            if (previewPersonaje.Image != null)
            {
                previewPersonaje.Image.Dispose();
                previewPersonaje.Image = null;
            }

            string ruta = rutasSubrazas[indiceSubraza];
            if (File.Exists(ruta))
                previewPersonaje.Image = Image.FromFile(ruta);

            lblNombreSubraza.Text = subrazas[indiceSubraza];
            lblNombreSubraza.Left = (anchoPantalla - lblNombreSubraza.PreferredWidth) / 2;

            subrazaSeleccionada = subrazas[indiceSubraza];
        }
        #endregion

        #region Confirmar
        private void Confirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(subrazaSeleccionada))
            {
                MessageBox.Show("Debes elegir una subraza antes de continuar.");
                return;
            }

            GameData.PersonajeSubraza = subrazaSeleccionada;



            var formClase = new SeleccionDeClaseForm(razaSeleccionada, subrazaSeleccionada);
            formClase.Show();
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
