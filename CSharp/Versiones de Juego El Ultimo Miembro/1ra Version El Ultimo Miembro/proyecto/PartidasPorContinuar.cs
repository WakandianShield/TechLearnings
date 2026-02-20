using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace proyecto
{
    public class CargarPartidaForm : Form
    {
        private ListBox lstPartidas;
        private Label lblDetalle;
        private Button btnAbrir, btnVolver;

        private readonly int anchoPantalla = Screen.PrimaryScreen.Bounds.Width;
        private readonly int altoPantalla = Screen.PrimaryScreen.Bounds.Height;

        private string carpetaSaves = Path.Combine(Application.StartupPath, "Saves");
        private string archivoSeleccionado = "";

        public CargarPartidaForm()
        {
            InicializarFormulario();
            Fuente.CargarFuente();
            ConfigurarControles();
            CargarListaPartidas();
            ConfigurarFondo();
        }

        #region Inicialización
        private void InicializarFormulario()
        {
            Text = "CARGAR PARTIDA";
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

        private void ConfigurarControles()
        {
            lstPartidas = new ListBox()
            {
                Font = Fuente.ObtenerFont(20),
                Width = 400,
                Height = 600,
                Left = 50,
                Top = 50
            };
            lstPartidas.SelectedIndexChanged += LstPartidas_SelectedIndexChanged;
            Controls.Add(lstPartidas);

            lblDetalle = new Label()
            {
                Font = Fuente.ObtenerFont(20),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Left = 500,
                Top = 50,
                Width = anchoPantalla - 550,
                Height = 600,
                AutoSize = false
            };
            Controls.Add(lblDetalle);

            btnAbrir = new Button()
            {
                Text = "ABRIR PARTIDA",
                Width = 250,
                Height = 80,
                Left = 50,
                Top = 700,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = Fuente.ObtenerFont(14)
            };
            btnAbrir.FlatAppearance.BorderSize = 0;
            AplicarDegradadoOscuroHover(btnAbrir, Color.Purple, Color.MediumPurple);
            btnAbrir.Click += BtnAbrir_Click;
            Controls.Add(btnAbrir);

            btnVolver = new Button()
            {
                Text = "VOLVER",
                Width = 150,
                Height = 50,
                Left = 350,
                Top = 700,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = Fuente.ObtenerFont(14)
            };
            btnVolver.FlatAppearance.BorderSize = 0;
            AplicarDegradadoOscuroHover(btnVolver, Color.Purple, Color.MediumPurple);
            
            btnVolver.Click += (s, e) =>
            {
                var MainMenuForm = new MainMenuForm();
                MainMenuForm.Show();
                this.Hide();
            };

            Controls.Add(btnVolver);
        }
        #endregion

        #region Lógica
        private void CargarListaPartidas()
        {
            lstPartidas.Items.Clear();
            if (!Directory.Exists(carpetaSaves)) Directory.CreateDirectory(carpetaSaves);

            string[] archivos = Directory.GetFiles(carpetaSaves, "*.json");
            foreach (var archivo in archivos)
            {
                lstPartidas.Items.Add(Path.GetFileNameWithoutExtension(archivo));
            }
        }

        private void LstPartidas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPartidas.SelectedItem == null) return;

            archivoSeleccionado = Path.Combine(carpetaSaves, lstPartidas.SelectedItem.ToString() + ".json");

            try
            {
                // PARA CARGAR DATOS TEMPORALMENTE Y MOSTRARLOS
                GameData.CargarPersonajeDesdeJson(archivoSeleccionado);

                // CON ESTO MUESTRO LOS STATS Y SELECCIONES DE UNA PARTIDA GUARDADA ANTERIORMENTE ( ESTO SE GUARDO EN EL GAME DATA CUANDO ALGN CREA UNA PARTIDA ) 
                lblDetalle.Text =
                    $"Nombre de partida: {lstPartidas.SelectedItem}\n" +
                    $"Raza: {GameData.PersonajeRaza}\n" +
                    $"Subraza: {GameData.PersonajeSubraza}\n" +
                    $"Clase: {GameData.PersonajeClase}\n" +
                    $"Trasfondo: {GameData.PersonajeTrasfondo}\n" +
                    $"Alineamiento: {GameData.PersonajeAlineamiento}\n\n" +
                    $"Fuerza: {GameData.StatsFuerza}\n" +
                    $"Destreza: {GameData.StatsDestreza}\n" +
                    $"Constitución: {GameData.StatsConstitucion}\n" +
                    $"Inteligencia: {GameData.StatsInteligencia}\n" +
                    $"Sabiduria: {GameData.StatsSabiduria}\n" +
                    $"Carisma: {GameData.StatsCarisma}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar partida: " + ex.Message);
            }
        }

        private void BtnAbrir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(archivoSeleccionado))
            {
                MessageBox.Show("Selecciona una partida primero.");
                return;
            }

            // AQUI GUARDO TODOS LOS DATOS EN LO DE GAME DATA DE FORMA PERMANENTE
            GameData.CargarPersonajeDesdeJson(archivoSeleccionado);



            var pantallaPrincipal = new PantallaPrincipal();
            pantallaPrincipal.Show();
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
