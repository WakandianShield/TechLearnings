using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace proyecto
{
    public class SeleccionDeRazaForm : Form
    {
        private Button btnConfirmar;
        private PictureBox previewPersonaje;
        private Label lblNombrePersonaje;
        private Button btnIzquierda, btnDerecha;

        private string[] nombres = { "HUMANO", "ELFO", "ENANO", "ORCO" };
        private string[] rutasImagenes;

        private int indiceActual = 0;

        private readonly int anchoPantalla = Screen.PrimaryScreen.Bounds.Width;
        private readonly int altoPantalla = Screen.PrimaryScreen.Bounds.Height;

        public SeleccionDeRazaForm()
        {
            rutasImagenes = new string[]
            {
                Path.Combine(Application.StartupPath, "Resources", "Humano.png"),
                Path.Combine(Application.StartupPath, "Resources", "Elfo.png"),
                Path.Combine(Application.StartupPath, "Resources", "Enano.png"),
                Path.Combine(Application.StartupPath, "Resources", "Orco.png")
            };

            InicializarFormulario();
            CargarFuente();
            ConfigurarFondo();
            CrearPreviewPersonaje();
            CrearFlechasCarrusel();
            CrearNombrePersonaje();
            CrearBotonConfirmar();
            CrearTitulo();
            MostrarPersonajeActual(); 
            CrearBotonVolver();
        }

        #region Inicialización

        private void InicializarFormulario()
        {
            Text = "SELECCION DE RAZA";
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
        }

        private void CargarFuente()
        {
            Fuente.CargarFuente();
        }

        private void ConfigurarFondo()
        {
            string rutaFondo = Path.Combine(Application.StartupPath, "Resources", "fondoblur.png");
            if (File.Exists(rutaFondo))
                BackgroundImage = Image.FromFile(rutaFondo);
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion

        private void CrearTitulo()
        {
            Label lblTitulo = new Label()
            {
                Text = "SELECCIONAR RAZA",
                Font = Fuente.ObtenerFont(50),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblTitulo.Left = (anchoPantalla - lblTitulo.PreferredWidth) / 2;
            lblTitulo.Top = 20;
            Controls.Add(lblTitulo);
        }

        #region Controles Carrusel

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
            btnIzquierda.Click += (s, e) => CambiarPersonaje(-1);
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
            btnDerecha.Click += (s, e) => CambiarPersonaje(1);
            Controls.Add(btnDerecha);
        }

        private void CrearNombrePersonaje()
        {
            lblNombrePersonaje = new Label()
            {
                Font = Fuente.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true
            };
            lblNombrePersonaje.Top = 750;
            lblNombrePersonaje.Left = (anchoPantalla - lblNombrePersonaje.Width) / 2;
            Controls.Add(lblNombrePersonaje);
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

            int radio = 25;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(btnVolver.Width - radio, 0, radio, radio, 270, 90);
            path.AddArc(btnVolver.Width - radio, btnConfirmar.Height - radio, radio, radio, 0, 90);
            path.AddArc(0, btnVolver.Height - radio, radio, radio, 90, 90);
            path.CloseAllFigures();
            btnVolver.Region = new Region(path);

            AplicarDegradadoOscuroHover(btnVolver, Color.Purple, Color.MediumPurple);

            btnVolver.Click += (s, e) =>
            {
                var MainMenuForm = new MainMenuForm();
                MainMenuForm.Show();
                this.Hide();
            };

            btnVolver.BringToFront();
            Controls.Add(btnVolver);


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

        private void CambiarPersonaje(int direccion)
        {
            indiceActual += direccion;
            if (indiceActual < 0) indiceActual = nombres.Length - 1;
            if (indiceActual >= nombres.Length) indiceActual = 0;

            MostrarPersonajeActual();
        }

        private void MostrarPersonajeActual()
        {
            string ruta = rutasImagenes[indiceActual];
            if (File.Exists(ruta))
                previewPersonaje.Image = Image.FromFile(ruta);

            lblNombrePersonaje.Text = nombres[indiceActual];
            lblNombrePersonaje.Left = (anchoPantalla - lblNombrePersonaje.PreferredWidth) / 2;
        }

        private void Confirmar_Click(object sender, EventArgs e)
        {
            string razaSeleccionada = nombres[indiceActual];

            GameData.PersonajeRaza = razaSeleccionada; 
            GameData.PersonajeSubraza = (razaSeleccionada == "HUMANO" || razaSeleccionada == "ORCO") ? "DEFAULT" : null;
            GameData.TienePartida = true;

            if (razaSeleccionada == "HUMANO" || razaSeleccionada == "ORCO")
            {
                var formClase = new SeleccionDeClaseForm(razaSeleccionada, GameData.PersonajeSubraza);
                formClase.Show();
                this.Hide();
            }
            else
            {
                var formSubraza = new SeleccionDeSubrazaForm(razaSeleccionada);
                formSubraza.Show();
                this.Hide();
            }
        }


        #endregion

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
    }
}
