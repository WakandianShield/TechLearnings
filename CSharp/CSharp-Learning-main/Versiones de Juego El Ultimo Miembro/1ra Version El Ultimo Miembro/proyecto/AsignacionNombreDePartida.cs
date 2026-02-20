using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace proyecto
{
    public class ResumenPartidaForm : Form
    {
        private Label lblTitulo;
        private Label lblRaza, lblSubraza, lblClase, lblTrasfondo, lblAlineamiento;
        private Label lblStats;
        private TextBox txtNombrePartida;
        private Button btnConfirmar;
        private readonly int anchoPantalla = Screen.PrimaryScreen.Bounds.Width;
        private readonly int altoPantalla = Screen.PrimaryScreen.Bounds.Height;

        private SeleccionDeAlineamientoForm formAlineamientoAnterior;

        public ResumenPartidaForm(SeleccionDeAlineamientoForm alineamientoForm)
        {
            formAlineamientoAnterior = alineamientoForm;

            InicializarFormulario();
            Fuente.CargarFuente();
            ConfigurarFondo();
            CrearTitulo();
            MostrarResumen();
            CrearTextBoxNombre();
            CrearBotonConfirmar();
            CrearBotonVolver();
        }

        #region Inicialización
        private void InicializarFormulario()
        {
            Text = "RESUMEN DE PARTIDA";
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
                Text = "RESUMEN DE TU PERSONAJE",
                Font = Fuente.ObtenerFont(40),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true
            };
            lblTitulo.Left = (anchoPantalla - lblTitulo.PreferredWidth) / 2;
            lblTitulo.Top = 20;
            Controls.Add(lblTitulo);
        }

        private void MostrarResumen()
        {
            int topBase = 100;
            int spacing = 60;

            lblRaza = CrearLabel("Raza: " + GameData.PersonajeRaza, topBase);
            lblSubraza = CrearLabel("Subraza: " + GameData.PersonajeSubraza, topBase + spacing);
            lblClase = CrearLabel("Clase: " + GameData.PersonajeClase, topBase + spacing * 2);
            lblTrasfondo = CrearLabel("Trasfondo: " + GameData.PersonajeTrasfondo, topBase + spacing * 3);
            lblAlineamiento = CrearLabel("Alineamiento: " + GameData.PersonajeAlineamiento, topBase + spacing * 4);

            lblStats = CrearLabel("Estadísticas: \n" +
                $"Fuerza: {GameData.StatsFuerza}\n" +
                $"Destreza: {GameData.StatsDestreza}\n" +
                $"Constitución: {GameData.StatsConstitucion}\n" +
                $"Inteligencia: {GameData.StatsInteligencia}\n" +
                $"Sabiduría: {GameData.StatsSabiduria}\n" +
                $"Carisma: {GameData.StatsCarisma}",
                topBase + spacing * 5
            );
        }

        private Label CrearLabel(string texto, int top)
        {
            Label lbl = new Label()
            {
                Text = texto,
                Font = Fuente.ObtenerFont(25),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true
            };
            lbl.Left = 50;
            lbl.Top = top;
            Controls.Add(lbl);
            return lbl;
        }

        private void CrearTextBoxNombre()
        {
            Label lbl = new Label()
            {
                Text = "Nombre de la partida:",
                Font = Fuente.ObtenerFont(20),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true
            };
            lbl.Left = 50;
            lbl.Top = 600;
            Controls.Add(lbl);

            txtNombrePartida = new TextBox()
            {
                Width = 300,
                Font = Fuente.ObtenerFont(16),
                Left = 50,
                Top = 650
            };
            Controls.Add(txtNombrePartida);
        }

        private void CrearBotonConfirmar()
        {
            btnConfirmar = new Button()
            {
                Text = "GUARDAR PARTIDA",
                Width = 300,
                Height = 100,
                Left = (anchoPantalla - 300) / 2,
                Top = 750,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = Fuente.ObtenerFont(16)
            };
            btnConfirmar.FlatAppearance.BorderSize = 0;
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
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnVolver.FlatAppearance.BorderSize = 0;
            AplicarDegradadoOscuroHover(btnVolver, Color.Purple, Color.MediumPurple);

            btnVolver.Click += (s, e) =>
            {
                formAlineamientoAnterior.Show();
                this.Close();
            };

            Controls.Add(btnVolver);
        }
        #endregion

        #region Confirmar
        private void Confirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombrePartida.Text))
            {
                MessageBox.Show("Debes escribir un nombre para la partida.");
                return;
            }

            string nombreArchivo = Path.Combine(Application.StartupPath, "Saves", txtNombrePartida.Text + ".json");
            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Saves"));

            string json = GameData.GuardarPersonajeComoJson();
            File.WriteAllText(nombreArchivo, json);

            MessageBox.Show("Partida guardada correctamente!");

            var pantallaPrincipal = new PantallaPrincipal();
            pantallaPrincipal.Show();
            this.Close();
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
