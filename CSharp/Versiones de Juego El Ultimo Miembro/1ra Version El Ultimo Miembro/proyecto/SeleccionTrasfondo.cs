using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace proyecto
{
    public class SeleccionDeTrasfondoForm : Form
    {
        private string trasfondoSeleccionado = "";
        private PictureBox previewTrasfondo;
        private Label lblTitulo;
        private Label lblNombreTrasfondo;
        private Button btnConfirmar;
        private Button btnIzquierda, btnDerecha;

        private string[] trasfondos;
        private int indiceTrasfondo = 0;
        private string[] rutasTrasfondos;

        private readonly int anchoPantalla = Screen.PrimaryScreen.Bounds.Width;
        private readonly int altoPantalla = Screen.PrimaryScreen.Bounds.Height;

        private SeleccionDeClaseForm formClaseAnterior;
        public SeleccionDeTrasfondoForm(SeleccionDeClaseForm claseForm)
        {
            formClaseAnterior = claseForm;

            InicializarFormulario();
            Fuente.CargarFuente();
            ConfigurarFondo();
            CrearTitulo();
            CrearPreviewTrasfondo();
            CrearFlechasCarrusel();
            CrearNombreTrasfondo();
            CrearBotonConfirmar();
            CrearBotonVolver();
            InicializarTrasfondos();
        }

        #region Inicialización
        private void InicializarFormulario()
        {
            Text = "SELECCIÓN DE TRASFONDO";
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
                Text = "SELECCIONAR TRASFONDO",
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

        private void CrearPreviewTrasfondo()
        {
            previewTrasfondo = new PictureBox()
            {
                Width = 300,
                Height = 500,
                Left = (anchoPantalla - 270) / 2,
                Top = 225,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            Controls.Add(previewTrasfondo);
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
            btnIzquierda.Click += (s, e) => CambiarTrasfondo(-1);
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
            btnDerecha.Click += (s, e) => CambiarTrasfondo(1);
            Controls.Add(btnDerecha);
        }

        private void CrearNombreTrasfondo()
        {
            lblNombreTrasfondo = new Label()
            {
                Font = Fuente.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true
            };
            lblNombreTrasfondo.Top = 750;
            lblNombreTrasfondo.Left = (anchoPantalla - lblNombreTrasfondo.Width) / 2;
            Controls.Add(lblNombreTrasfondo);
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

            int radio = 25;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(btnVolver.Width - radio, 0, radio, radio, 270, 90);
            path.AddArc(btnVolver.Width - radio, btnConfirmar.Height - radio, radio, radio, 0, 90);
            path.AddArc(0, btnVolver.Height - radio, radio, radio, 90, 90);
            path.CloseAllFigures();
            btnVolver.Region = new Region(path);

            btnVolver.Click += (s, e) =>
            {
                formClaseAnterior.Show();
                this.Close();
            };

            Controls.Add(btnVolver);
        }
        #endregion

        #region Lógica Carrusel
        private void InicializarTrasfondos()
        {
            trasfondos = new[] { "SOLDADO", "MERCADER", "SABIO", "EXPLORADOR", "CRIMINAL", "ARTESANO", "NOBLE", "FORASTERO", "HERMITAÑO", "CHARLATAN" };

            rutasTrasfondos = new string[trasfondos.Length];
            for (int i = 0; i < trasfondos.Length; i++)
                rutasTrasfondos[i] = Path.Combine(Application.StartupPath, "Resources", $"{trasfondos[i]}.png");

            MostrarTrasfondoActual();
        }

        private void CambiarTrasfondo(int dir)
        {
            indiceTrasfondo += dir;
            if (indiceTrasfondo < 0) indiceTrasfondo = trasfondos.Length - 1;
            if (indiceTrasfondo >= trasfondos.Length) indiceTrasfondo = 0;

            MostrarTrasfondoActual();
        }

        private void MostrarTrasfondoActual()
        {
            lblNombreTrasfondo.Text = trasfondos[indiceTrasfondo];
            lblNombreTrasfondo.Left = (anchoPantalla - lblNombreTrasfondo.PreferredWidth) / 2;

            trasfondoSeleccionado = trasfondos[indiceTrasfondo];

            if (previewTrasfondo.Image != null)
            {
                previewTrasfondo.Image.Dispose();
                previewTrasfondo.Image = null;
            }

            string ruta = rutasTrasfondos[indiceTrasfondo];
            if (File.Exists(ruta))
                previewTrasfondo.Image = Image.FromFile(ruta);
        }
        #endregion

        #region Confirmar
        private void Confirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(trasfondoSeleccionado))
            {
                MessageBox.Show("Debes elegir un trasfondo antes de continuar.");
                return;
            }

            GameData.PersonajeTrasfondo = trasfondoSeleccionado;

            var SeleccionDeAlineamientoForm = new SeleccionDeAlineamientoForm(this);
            SeleccionDeAlineamientoForm.Show();
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
