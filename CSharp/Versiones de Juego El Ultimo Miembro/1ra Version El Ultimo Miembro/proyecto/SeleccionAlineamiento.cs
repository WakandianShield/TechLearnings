using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace proyecto
{
    public class SeleccionDeAlineamientoForm : Form
    {
        private string alineamientoSeleccionado = "";
        private PictureBox previewAlineamiento;
        private Label lblTitulo;
        private Label lblNombreAlineamiento;
        private Button btnConfirmar;
        private Button btnIzquierda, btnDerecha;

        private string[] alineamientos;
        private int indiceAlineamiento = 0;
        private string[] rutasAlineamientos;

        private readonly int anchoPantalla = Screen.PrimaryScreen.Bounds.Width;
        private readonly int altoPantalla = Screen.PrimaryScreen.Bounds.Height;

        private SeleccionDeTrasfondoForm formTrasfondoAnterior;

        public SeleccionDeAlineamientoForm(SeleccionDeTrasfondoForm trasfondoForm)
        {
            formTrasfondoAnterior = trasfondoForm;

            InicializarFormulario();
            Fuente.CargarFuente();
            ConfigurarFondo();
            CrearTitulo();
            CrearFlechasCarrusel();
            CrearNombreAlineamiento();
            CrearBotonConfirmar();
            CrearBotonVolver();
            InicializarAlineamientos();
        }

        #region Inicialización
        private void InicializarFormulario()
        {
            Text = "SELECCIÓN DE ALINEAMIENTO";
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
                Text = "SELECCIONAR ALINEAMIENTO",
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
            btnIzquierda.Click += (s, e) => CambiarAlineamiento(-1);
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
            btnDerecha.Click += (s, e) => CambiarAlineamiento(1);
            Controls.Add(btnDerecha);
        }

        private void CrearNombreAlineamiento()
        {
            lblNombreAlineamiento = new Label()
            {
                Font = Fuente.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true
            };
            lblNombreAlineamiento.Top = 510;
            lblNombreAlineamiento.Left = (anchoPantalla - lblNombreAlineamiento.Width) / 2;
            Controls.Add(lblNombreAlineamiento);
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
                formTrasfondoAnterior.Show();
                this.Close();
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
        private void InicializarAlineamientos()
        {
            alineamientos = new[]
            {
                "LEGAL BUENO", "NEUTRAL BUENO", "CAOTICO BUENO",
                "NEUTRAL NEUTRAL", "CAOTICO NEUTRAL",
                "LEGAL MALVADO", "NEUTRAL MALVADO", "CAOTICO MALVADO"
            };

            rutasAlineamientos = new string[alineamientos.Length];
            for (int i = 0; i < alineamientos.Length; i++)
                rutasAlineamientos[i] = Path.Combine(Application.StartupPath, "Resources", $"{alineamientos[i]}.png");

            MostrarAlineamientoActual();
        }

        private void CambiarAlineamiento(int dir)
        {
            indiceAlineamiento += dir;
            if (indiceAlineamiento < 0) indiceAlineamiento = alineamientos.Length - 1;
            if (indiceAlineamiento >= alineamientos.Length) indiceAlineamiento = 0;

            MostrarAlineamientoActual();
        }

        private void MostrarAlineamientoActual()
        {
            lblNombreAlineamiento.Text = alineamientos[indiceAlineamiento];
            lblNombreAlineamiento.Left = (anchoPantalla - lblNombreAlineamiento.PreferredWidth) / 2;

            alineamientoSeleccionado = alineamientos[indiceAlineamiento];

        }
        #endregion

        #region Confirmar
        private void Confirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(alineamientoSeleccionado))
            {
                MessageBox.Show("Debes elegir un alineamiento antes de continuar.");
                return;
            }

            GameData.PersonajeAlineamiento = alineamientoSeleccionado;

            var ResumenPartidaForm = new ResumenPartidaForm(this);
            ResumenPartidaForm.Show();
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
