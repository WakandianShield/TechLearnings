using System;
using System.Drawing;
using System.Windows.Forms;

namespace proyecto
{
    public class PantallaPrincipal : Form
    {
        private Panel panelMapa;
        private Panel panelMenu;
        private Label lblEstadisticas;
        private PictureBox imagenPersonaje;
        private Button btnInventario;
        private Button btnHabilidades;
        private Button btnSalir;

        private int filas = 30;
        private int columnas = 50;
        private Button[,] casillas; 
        private float tamX = 0;      
        private float tamY = 0;   

        private PointF personajePos;
        private PointF destinoPos;
        private System.Windows.Forms.Timer timerMovimiento;

        public PantallaPrincipal()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.DoubleBuffered = true;

            InicializarComponentes();

        
            this.Shown += (s, e) =>
            {
                tamX = panelMapa.Width / (float)columnas;
                tamY = panelMapa.Height / (float)filas;

                CrearMapa();

                personajePos = new PointF(0, (filas - 1) * tamY);
                imagenPersonaje.Location = Point.Round(personajePos);
                imagenPersonaje.Size = new Size((int)tamX, (int)tamY);
            };
        }

        private void InicializarComponentes()
        {
            // mapa
            panelMapa = new Panel();
            panelMapa.Dock = DockStyle.Fill;
            panelMapa.BackColor = Color.DimGray;
            this.Controls.Add(panelMapa);

            // menu
            panelMenu = new Panel();
            panelMenu.Dock = DockStyle.Bottom;
            panelMenu.Height = 160;
            panelMenu.BackColor = Color.FromArgb(40, 40, 40);
            this.Controls.Add(panelMenu);

            // estads
            lblEstadisticas = new Label
            {
                Text = $"Raza: {GameData.Raza} | Subraza: {GameData.SubRaza}\n" +
                       $"Trasfondo: {GameData.Trasfondo} | Alineamiento: {GameData.Alineamiento} | Habilidad: {GameData.Habilidad}",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            panelMenu.Controls.Add(lblEstadisticas);

            // iamgen del personaje
            imagenPersonaje = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            panelMapa.Controls.Add(imagenPersonaje);
            imagenPersonaje.BringToFront();


            // botons de menuy
            btnInventario = CrearBotonMenu("Inventario");
            btnHabilidades = CrearBotonMenu("Habilidades");
            btnSalir = CrearBotonMenu("Salir");
            btnSalir.Click += (s, e) => this.Close();

            panelMenu.Controls.Add(btnInventario);
            panelMenu.Controls.Add(btnHabilidades);
            panelMenu.Controls.Add(btnSalir);

            panelMenu.Resize += (s, e) =>
            {
                btnInventario.Location = new Point(panelMenu.Width - 400, 60);
                btnHabilidades.Location = new Point(panelMenu.Width - 260, 60);
                btnSalir.Location = new Point(panelMenu.Width - 130, 60);
            };

            // movimiento
            timerMovimiento = new System.Windows.Forms.Timer();
            timerMovimiento.Interval = 10;
            timerMovimiento.Tick += TimerMovimiento_Tick;
        }

        private Button CrearBotonMenu(string texto)
        {
            Button btn = new Button
            {
                Text = texto,
                Size = new Size(120, 40),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.DarkOliveGreen
            };
            btn.MouseEnter += (s, e) => btn.BackColor = Color.Green;
            btn.MouseLeave += (s, e) => btn.BackColor = Color.DarkOliveGreen;
            return btn;
        }

        private void CrearMapa()
        {
            panelMapa.SuspendLayout();

            casillas = new Button[filas, columnas];

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    var casilla = new Button
                    {
                        Size = new Size((int)tamX + 1, (int)tamY + 1),
                        Location = new Point((int)(j * tamX), (int)(i * tamY)),
                        BackColor = Color.Green,
                        FlatStyle = FlatStyle.Flat
                    };
                    casilla.FlatAppearance.BorderSize = 0;
                    casilla.Click += Casilla_Click;

                    casillas[i, j] = casilla;
                    panelMapa.Controls.Add(casilla);
                }
            }

            panelMapa.ResumeLayout();
        }

        private void Casilla_Click(object sender, EventArgs e)
        {
            var casilla = sender as Button;
            destinoPos = casilla.Location;
            timerMovimiento.Start();
        }

        private void TimerMovimiento_Tick(object sender, EventArgs e)
        {
            float dx = destinoPos.X - personajePos.X;
            float dy = destinoPos.Y - personajePos.Y;
            float velocidad = 4f;

            if (Math.Abs(dx) < velocidad && Math.Abs(dy) < velocidad)
            {
                personajePos = destinoPos;
                timerMovimiento.Stop();
            }
            else
            {
                float distancia = (float)Math.Sqrt(dx * dx + dy * dy);
                personajePos.X += dx / distancia * velocidad;
                personajePos.Y += dy / distancia * velocidad;
            }

            imagenPersonaje.Location = Point.Round(personajePos);
        }
    }
}
