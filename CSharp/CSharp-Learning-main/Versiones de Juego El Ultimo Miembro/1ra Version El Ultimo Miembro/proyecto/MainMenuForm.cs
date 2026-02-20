using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace proyecto
{
    public class MainMenuForm : Form
    {
        private Button btnNuevaPartida, btnContinuar, btnSalir;
        private PictureBox logoPictureBox;

        public MainMenuForm()
        {
            Fuente.CargarFuente();
            InicializarFormulario();
            CrearControles();
        }

        private void InicializarFormulario()
        {
            Text = "Menú Principal";
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;

            string rutaFondo = Path.Combine(Application.StartupPath, "Resources", "fondo.png");
            if (File.Exists(rutaFondo))
            {
                BackgroundImage = Image.FromFile(rutaFondo);
                BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void CrearControles()
        {
            int pantallaAncho = Screen.PrimaryScreen.Bounds.Width;
            int botonWidth = 500;
            int botonHeight = 60;

            int topInicial = 700;

            btnNuevaPartida = CrearBoton("NUEVA PARTIDA", pantallaAncho / 2 - botonWidth / 2, topInicial, botonWidth, botonHeight);
            btnNuevaPartida.Click += (s, e) =>
            {
                var seleccionForm = new SeleccionDeRazaForm();
                seleccionForm.Show();
                this.Hide();
            };
            AplicarBordesRedondeados(btnNuevaPartida, 30, redondearArriba: true, redondearAbajo: false);


            // BOTON PARA CONTINUAR

            btnContinuar = CrearBoton("CONTINUAR", pantallaAncho / 2 - botonWidth / 2, topInicial + botonHeight, botonWidth, botonHeight);
            btnContinuar.Click += (s, e) =>
            {
                var cargarPartidaForm = new CargarPartidaForm();
                cargarPartidaForm.Show();
                this.Hide();
            };

            btnSalir = CrearBoton("SALIR", pantallaAncho / 2 - botonWidth / 2, topInicial + 2 * botonHeight, botonWidth, botonHeight);
            btnSalir.Click += (s, e) => Application.Exit();
            AplicarBordesRedondeados(btnSalir, 30, redondearArriba: false, redondearAbajo: true);

            // LOGO
            string rutaLogo = Path.Combine(Application.StartupPath, "Resources", "LOGO.png");
            if (File.Exists(rutaLogo))
            {
                logoPictureBox = new PictureBox()
                {
                    Image = Image.FromFile(rutaLogo),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 800,
                    Height = 400,
                    Left = pantallaAncho / 2 - 400,
                    Top = 250,
                    BackColor = Color.Transparent
                };
                Controls.Add(logoPictureBox);
            }

            Controls.Add(btnNuevaPartida);
            Controls.Add(btnContinuar);
            Controls.Add(btnSalir);
        }

        private Button CrearBoton(string texto, int left, int top, int width, int height)
        {
            Button btn = new Button()
            {
                Text = texto,
                Width = width,
                Height = height,
                Left = left,
                Top = top,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = Fuente.ObtenerFont(16),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };

            btn.FlatAppearance.BorderSize = 0;

            return btn;
        }

        private void AplicarBordesRedondeados(Button btn, int radio, bool redondearArriba, bool redondearAbajo)
        {
            GraphicsPath path = new GraphicsPath();

            if (redondearArriba && redondearAbajo)
            { 
                path.AddArc(0, 0, radio, radio, 180, 90);
                path.AddArc(btn.Width - radio, 0, radio, radio, 270, 90);
                path.AddArc(btn.Width - radio, btn.Height - radio, radio, radio, 0, 90);
                path.AddArc(0, btn.Height - radio, radio, radio, 90, 90);
                path.CloseAllFigures();
            }
            else if (redondearArriba)
            {
                path.AddArc(0, 0, radio, radio, 180, 90);
                path.AddArc(btn.Width - radio, 0, radio, radio, 270, 90);
                path.AddLine(btn.Width, btn.Height, 0, btn.Height);
                path.CloseAllFigures();
            }
            else if (redondearAbajo)
            {
                path.AddLine(0, 0, btn.Width, 0);
                path.AddLine(btn.Width, 0, btn.Width, btn.Height - radio);
                path.AddArc(btn.Width - radio, btn.Height - radio, radio, radio, 0, 90);
                path.AddArc(0, btn.Height - radio, radio, radio, 90, 90);
                path.CloseAllFigures();
            }

            btn.Region = new Region(path);
        }
    }
}
