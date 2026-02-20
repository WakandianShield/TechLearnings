using proyecto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PROYECTO_5TO___TOTЯ
{
    public class COMBATE : Form
    {
        private Personaje jugador;
        private ENEMIGOS enemigo;
        private Point posicionRegreso;

        // Sistema de combate por turnos y casillas - optimizado para 1920x1080
        private const int TAMAÑO_CASILLA = 70;
        private const int FILAS_TABLERO = 8;
        private const int COLUMNAS_TABLERO = 12;

        private int[,] tablero;
        private Point posicionJugador;
        private Point posicionEnemigo;
        private List<Point> obstaculos;

        private int hpJugador;
        private int hpEnemigo;
        private int hpMaxJugador;
        private int hpMaxEnemigo;

        private bool turnoJugador = true;
        private int movimientoRestante;
        private int accionDisponible = 1;

        // UI Components con estilo similar a PANTALLA_JUEGO
        private Panel panelTablero;
        private Panel panelEstadisticas;
        private Panel panelAcciones;
        private RichTextBox txtLog;
        private Label lblTurno;
        private Label lblMovimiento;
        private Label lblAccion;

        private Button[,] casillas;
        private List<Point> casillasAlcance;
        private List<Point> casillasMovimiento;

        private string accionSeleccionada = "";
        private bool esperandoSeleccion = false;

        public bool JugadorGano { get; private set; }
        public int HPRestanteJugador { get; private set; }

        public COMBATE(Personaje personaje, ENEMIGOS enemigoActual, Point posicionActual)
        {
            jugador = personaje;
            enemigo = enemigoActual;
            posicionRegreso = posicionActual;

            // Inicializar stats de combate
            hpMaxJugador = jugador.HP;
            hpJugador = hpMaxJugador;
            hpMaxEnemigo = CalcularHPEnemigo();
            hpEnemigo = hpMaxEnemigo;

            movimientoRestante = CalcularMovimientoJugador();

            InicializarTablero();
            InicializarFormulario();
            CrearInterfaz();
            IniciarCombate();
        }

        private void InicializarFormulario()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(18, 18, 22); // Mismo fondo que PANTALLA_JUEGO
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Size = new Size(1920, 1080);
        }

        private void InicializarTablero()
        {
            tablero = new int[FILAS_TABLERO, COLUMNAS_TABLERO];
            obstaculos = new List<Point>();
            casillasAlcance = new List<Point>();
            casillasMovimiento = new List<Point>();

            // Posiciones iniciales
            posicionJugador = new Point(2, FILAS_TABLERO / 2);
            posicionEnemigo = new Point(COLUMNAS_TABLERO - 3, FILAS_TABLERO / 2);

            tablero[posicionJugador.Y, posicionJugador.X] = 1;
            tablero[posicionEnemigo.Y, posicionEnemigo.X] = 2;

            // Generar obstáculos
            Random rand = new Random();
            for (int i = 0; i < 15; i++)
            {
                int x = rand.Next(2, COLUMNAS_TABLERO - 2);
                int y = rand.Next(1, FILAS_TABLERO - 1);

                if (tablero[y, x] == 0 &&
                    CalcularDistancia(new Point(x, y), posicionJugador) > 2 &&
                    CalcularDistancia(new Point(x, y), posicionEnemigo) > 2)
                {
                    tablero[y, x] = 3;
                    obstaculos.Add(new Point(x, y));
                }
            }
        }

        private int CalcularHPEnemigo()
        {
            return 25 + (jugador.LVL * 8);
        }

        private int CalcularMovimientoJugador()
        {
            return 5 + Math.Max(0, (jugador.DEX - 10) / 2);
        }

        private void CrearInterfaz()
        {
            // TÍTULO CON ESTILO SIMILAR A PANTALLA_JUEGO
            Label lblTitulo = new Label
            {
                Text = "⚔ ENCUENTRO DE COMBATE ⚔",
                Font = FUENTE.ObtenerFont(28),
                ForeColor = Color.FromArgb(255, 100, 100), // Rojo similar a sección COMBATE
                AutoSize = true,
                Location = new Point(700, 20),
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblTitulo);

            // PANEL TABLERO CENTRAL
            int anchoTablero = COLUMNAS_TABLERO * TAMAÑO_CASILLA;
            int altoTablero = FILAS_TABLERO * TAMAÑO_CASILLA;
            panelTablero = new Panel
            {
                Size = new Size(anchoTablero, altoTablero),
                Location = new Point(400, 80),
                BackColor = Color.FromArgb(28, 30, 35), // Mismo color de paneles
                BorderStyle = BorderStyle.FixedSingle
            };
            RedondearPanel(panelTablero, 15);
            this.Controls.Add(panelTablero);

            CrearTablero();

            // PANEL ESTADÍSTICAS IZQUIERDO - Estilo similar a PANTALLA_JUEGO
            panelEstadisticas = new Panel
            {
                Size = new Size(350, 400),
                Location = new Point(30, 80),
                BackColor = Color.FromArgb(28, 30, 35)
            };
            RedondearPanel(panelEstadisticas, 15);
            this.Controls.Add(panelEstadisticas);

            CrearPanelEstadisticas();

            // PANEL ACCIONES DERECHO - Estilo similar a PANTALLA_JUEGO
            panelAcciones = new Panel
            {
                Size = new Size(350, 400),
                Location = new Point(1520, 80),
                BackColor = Color.FromArgb(28, 30, 35)
            };
            RedondearPanel(panelAcciones, 15);
            this.Controls.Add(panelAcciones);

            CrearPanelAcciones();

            // PANEL LOG INFERIOR
            Panel panelLog = new Panel
            {
                Size = new Size(1200, 200),
                Location = new Point(360, 820),
                BackColor = Color.FromArgb(28, 30, 35)
            };
            RedondearPanel(panelLog, 15);
            this.Controls.Add(panelLog);

            txtLog = new RichTextBox
            {
                Size = new Size(1180, 180),
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(35, 38, 45), // Color de cajas internas
                ForeColor = Color.FromArgb(220, 220, 200),
                Font = FUENTE.ObtenerFont(10),
                ReadOnly = true,
                BorderStyle = BorderStyle.None
            };
            RedondearControl(txtLog, 10);
            panelLog.Controls.Add(txtLog);

            // INFO DE TURNO Y ESTADO
            CrearPanelEstado();
        }

        private void CrearPanelEstado()
        {
            Panel panelEstado = new Panel
            {
                Size = new Size(800, 60),
                Location = new Point(560, 500),
                BackColor = Color.FromArgb(28, 30, 35)
            };
            RedondearPanel(panelEstado, 12);
            this.Controls.Add(panelEstado);

            lblTurno = new Label
            {
                Text = "TURNO: JUGADOR",
                Font = FUENTE.ObtenerFont(18),
                ForeColor = Color.FromArgb(100, 200, 255), // Azul similar a INFORMACION
                AutoSize = true,
                Location = new Point(20, 18),
                BackColor = Color.Transparent
            };
            panelEstado.Controls.Add(lblTurno);

            lblMovimiento = new Label
            {
                Text = $"MOVIMIENTO: {movimientoRestante}",
                Font = FUENTE.ObtenerFont(14),
                ForeColor = Color.FromArgb(150, 255, 150), // Verde similar a VEL
                AutoSize = true,
                Location = new Point(250, 20),
                BackColor = Color.Transparent
            };
            panelEstado.Controls.Add(lblMovimiento);

            lblAccion = new Label
            {
                Text = $"ACCIÓN: {accionDisponible}",
                Font = FUENTE.ObtenerFont(14),
                ForeColor = Color.FromArgb(255, 200, 100), // Naranja similar a INI
                AutoSize = true,
                Location = new Point(450, 20),
                BackColor = Color.Transparent
            };
            panelEstado.Controls.Add(lblAccion);
        }

        private void CrearTablero()
        {
            casillas = new Button[FILAS_TABLERO, COLUMNAS_TABLERO];

            for (int fila = 0; fila < FILAS_TABLERO; fila++)
            {
                for (int col = 0; col < COLUMNAS_TABLERO; col++)
                {
                    Button casilla = new Button
                    {
                        Size = new Size(TAMAÑO_CASILLA, TAMAÑO_CASILLA),
                        Location = new Point(col * TAMAÑO_CASILLA, fila * TAMAÑO_CASILLA),
                        FlatStyle = FlatStyle.Flat,
                        Tag = new Point(col, fila),
                        Font = FUENTE.ObtenerFont(14),
                        Cursor = Cursors.Hand
                    };

                    casilla.FlatAppearance.BorderSize = 1;
                    casilla.FlatAppearance.BorderColor = Color.FromArgb(50, 50, 60);

                    // Estilo de tablero similar al diseño general
                    if ((col + fila) % 2 == 0)
                        casilla.BackColor = Color.FromArgb(45, 48, 55);
                    else
                        casilla.BackColor = Color.FromArgb(35, 38, 45);

                    casilla.Click += Casilla_Click;
                    casillas[fila, col] = casilla;
                    panelTablero.Controls.Add(casilla);
                }
            }

            ActualizarTablero();
        }

        private void CrearPanelEstadisticas()
        {
            // TÍTULO
            Label lblTituloStats = new Label
            {
                Text = "ESTADÍSTICAS",
                Font = FUENTE.ObtenerFont(16),
                ForeColor = Color.FromArgb(255, 180, 100), // Naranja similar a ATRIBUTOS
                Location = new Point(20, 15),
                AutoSize = true
            };
            panelEstadisticas.Controls.Add(lblTituloStats);

            // JUGADOR
            Panel panelJugador = new Panel
            {
                Size = new Size(310, 160),
                Location = new Point(20, 50),
                BackColor = Color.FromArgb(35, 38, 45)
            };
            RedondearPanel(panelJugador, 10);
            panelEstadisticas.Controls.Add(panelJugador);

            Label lblJugador = new Label
            {
                Text = jugador.NOMBRE.ToUpper(),
                Font = FUENTE.ObtenerFont(14),
                ForeColor = Color.White,
                Location = new Point(10, 10),
                AutoSize = true
            };
            panelJugador.Controls.Add(lblJugador);

            Label lblClaseJugador = new Label
            {
                Text = $"{jugador.CLASE} • Nivel {jugador.LVL}",
                Font = FUENTE.ObtenerFont(11),
                ForeColor = Color.LightGray,
                Location = new Point(10, 35),
                AutoSize = true
            };
            panelJugador.Controls.Add(lblClaseJugador);

            // HP JUGADOR
            Label lblHPJugador = new Label
            {
                Text = $"PUNTOS DE GOLPE: {hpJugador}/{hpMaxJugador}",
                Font = FUENTE.ObtenerFont(12),
                ForeColor = Color.FromArgb(255, 80, 80), // Rojo similar a HP
                Location = new Point(10, 65),
                AutoSize = true
            };
            panelJugador.Controls.Add(lblHPJugador);

            ProgressBar barraHPJugador = new ProgressBar
            {
                Size = new Size(290, 20),
                Location = new Point(10, 90),
                Maximum = hpMaxJugador,
                Value = hpJugador,
                ForeColor = Color.FromArgb(255, 80, 80),
                BackColor = Color.FromArgb(50, 30, 30)
            };
            panelJugador.Controls.Add(barraHPJugador);

            // STATS JUGADOR
            Label lblStatsJugador = new Label
            {
                Text = $"CA: {jugador.CA} | INI: {jugador.INI}",
                Font = FUENTE.ObtenerFont(10),
                ForeColor = Color.LightBlue,
                Location = new Point(10, 120),
                AutoSize = true
            };
            panelJugador.Controls.Add(lblStatsJugador);

            // ENEMIGO
            Panel panelEnemigo = new Panel
            {
                Size = new Size(310, 160),
                Location = new Point(20, 230),
                BackColor = Color.FromArgb(35, 38, 45)
            };
            RedondearPanel(panelEnemigo, 10);
            panelEstadisticas.Controls.Add(panelEnemigo);

            Label lblEnemigo = new Label
            {
                Text = "CRIATURA HOSTIL",
                Font = FUENTE.ObtenerFont(14),
                ForeColor = Color.White,
                Location = new Point(10, 10),
                AutoSize = true
            };
            panelEnemigo.Controls.Add(lblEnemigo);

            Label lblNivelEnemigo = new Label
            {
                Text = $"Amenaza Nvl {jugador.LVL + 1}",
                Font = FUENTE.ObtenerFont(11),
                ForeColor = Color.LightGray,
                Location = new Point(10, 35),
                AutoSize = true
            };
            panelEnemigo.Controls.Add(lblNivelEnemigo);

            // HP ENEMIGO
            Label lblHPEnemigo = new Label
            {
                Text = $"PUNTOS DE GOLPE: {hpEnemigo}/{hpMaxEnemigo}",
                Font = FUENTE.ObtenerFont(12),
                ForeColor = Color.FromArgb(255, 80, 80),
                Location = new Point(10, 65),
                AutoSize = true
            };
            panelEnemigo.Controls.Add(lblHPEnemigo);

            ProgressBar barraHPEnemigo = new ProgressBar
            {
                Size = new Size(290, 20),
                Location = new Point(10, 90),
                Maximum = hpMaxEnemigo,
                Value = hpEnemigo,
                ForeColor = Color.FromArgb(255, 80, 80),
                BackColor = Color.FromArgb(50, 30, 30)
            };
            panelEnemigo.Controls.Add(barraHPEnemigo);

            // STATS ENEMIGO
            Label lblStatsEnemigo = new Label
            {
                Text = $"CA: 12 | Movimiento: 4\nAlcance: Melee | Daño: 1d6+2",
                Font = FUENTE.ObtenerFont(10),
                ForeColor = Color.LightBlue,
                Location = new Point(10, 120),
                AutoSize = false,
                Size = new Size(290, 35)
            };
            panelEnemigo.Controls.Add(lblStatsEnemigo);
        }

        private void CrearPanelAcciones()
        {
            Label lblTituloAcciones = new Label
            {
                Text = "ACCIONES",
                Font = FUENTE.ObtenerFont(16),
                ForeColor = Color.FromArgb(180, 180, 180), // Gris similar a ACCIONES
                Location = new Point(20, 15),
                AutoSize = true
            };
            panelAcciones.Controls.Add(lblTituloAcciones);

            // BOTONES DE ACCIÓN CON ESTILO SIMILAR A PANTALLA_JUEGO
            int y = 50;
            string[] acciones = {
                "🏃 MOVIMIENTO", "⚔ ATAQUE MELEE", "🏹 ATAQUE DISTANCIA",
                "✨ HABILIDAD", "🛡 DEFENDER", "⏹ TERMINAR TURNO"
            };

            Color[] colores = {
                Color.FromArgb(70, 130, 180),    // Azul
                Color.FromArgb(178, 34, 34),     // Rojo
                Color.FromArgb(65, 105, 225),    // Azul oscuro
                Color.FromArgb(147, 112, 219),   // Púrpura
                Color.FromArgb(46, 139, 87),     // Verde
                Color.FromArgb(169, 169, 169)    // Gris
            };

            for (int i = 0; i < acciones.Length; i++)
            {
                Button btnAccion = new Button
                {
                    Text = acciones[i],
                    Size = new Size(310, 50),
                    Location = new Point(20, y),
                    BackColor = colores[i],
                    ForeColor = Color.White,
                    Font = FUENTE.ObtenerFont(12),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Tag = acciones[i].Substring(2).Trim(),
                    Margin = new Padding(0, 0, 0, 10)
                };

                btnAccion.FlatAppearance.BorderSize = 0;
                btnAccion.Click += Accion_Click;
                RedondearBoton(btnAccion, 8);

                // Efectos hover
                Color colorOriginal = colores[i];
                btnAccion.MouseEnter += (s, e) =>
                {
                    btnAccion.BackColor = Color.FromArgb(
                        Math.Min(255, colorOriginal.R + 30),
                        Math.Min(255, colorOriginal.G + 30),
                        Math.Min(255, colorOriginal.B + 30)
                    );
                };
                btnAccion.MouseLeave += (s, e) => btnAccion.BackColor = colorOriginal;

                panelAcciones.Controls.Add(btnAccion);
                y += 60;
            }
        }

        // MÉTODOS DE COMBATE CORREGIDOS
        private void Casilla_Click(object sender, EventArgs e)
        {
            if (!turnoJugador) return;

            Button casilla = (Button)sender;
            Point pos = (Point)casilla.Tag;

            if (esperandoSeleccion)
            {
                ProcesarSeleccion(pos);
            }
            else if (casillasMovimiento.Contains(pos) && movimientoRestante > 0)
            {
                MoverJugador(pos);
            }
        }

        private void Accion_Click(object sender, EventArgs e)
        {
            if (!turnoJugador) return;

            Button btn = (Button)sender;
            string accion = (string)btn.Tag;

            // Limpiar selecciones anteriores
            LimpiarCasillasResaltadas();
            esperandoSeleccion = false;

            switch (accion)
            {
                case "MOVIMIENTO":
                    if (movimientoRestante > 0)
                    {
                        accionSeleccionada = "MOVIMIENTO";
                        CalcularCasillasMovimiento();
                        AgregarLog("Selecciona una casilla para moverte", Color.Cyan);
                    }
                    else
                    {
                        AgregarLog("No te queda movimiento este turno", Color.Orange);
                    }
                    break;

                case "ATAQUE MELEE":
                    if (accionDisponible > 0)
                    {
                        accionSeleccionada = "ATAQUE_MELEE";
                        esperandoSeleccion = true;
                        CalcularCasillasAlcanceMelee();
                        AgregarLog("Selecciona al enemigo para ataque cuerpo a cuerpo", Color.Red);
                    }
                    else
                    {
                        AgregarLog("No te quedan acciones este turno", Color.Orange);
                    }
                    break;

                case "ATAQUE DISTANCIA":
                    if (accionDisponible > 0)
                    {
                        accionSeleccionada = "ATAQUE_DISTANCIA";
                        esperandoSeleccion = true;
                        CalcularCasillasAlcanceDistancia();
                        AgregarLog("Selecciona al enemigo para ataque a distancia", Color.Blue);
                    }
                    else
                    {
                        AgregarLog("No te quedan acciones este turno", Color.Orange);
                    }
                    break;

                case "HABILIDAD":
                    if (accionDisponible > 0)
                    {
                        UsarHabilidadEspecial();
                    }
                    else
                    {
                        AgregarLog("No te quedan acciones este turno", Color.Orange);
                    }
                    break;

                case "DEFENDER":
                    Defender();
                    break;

                case "TERMINAR TURNO":
                    TerminarTurnoJugador();
                    break;
            }
        }

        private void ProcesarSeleccion(Point pos)
        {
            if (!esperandoSeleccion) return;

            switch (accionSeleccionada)
            {
                case "ATAQUE_MELEE":
                    if (pos == posicionEnemigo && EstaEnAlcanceMelee(pos))
                    {
                        RealizarAtaqueMelee();
                    }
                    else
                    {
                        AgregarLog("El enemigo no está al alcance para ataque cuerpo a cuerpo", Color.Orange);
                    }
                    break;

                case "ATAQUE_DISTANCIA":
                    if (pos == posicionEnemigo && EstaEnAlcanceDistancia(pos))
                    {
                        RealizarAtaqueDistancia();
                    }
                    else
                    {
                        AgregarLog("El enemigo está fuera del alcance", Color.Orange);
                    }
                    break;
            }

            esperandoSeleccion = false;
            accionSeleccionada = "";
            LimpiarCasillasResaltadas();
            ActualizarTablero();
        }

        private void MoverJugador(Point nuevaPos)
        {
            int costoMovimiento = CalcularCostoMovimiento(posicionJugador, nuevaPos);

            if (costoMovimiento <= movimientoRestante && EsPosicionValida(nuevaPos) && tablero[nuevaPos.Y, nuevaPos.X] == 0)
            {
                tablero[posicionJugador.Y, posicionJugador.X] = 0;
                tablero[nuevaPos.Y, nuevaPos.X] = 1;
                posicionJugador = nuevaPos;

                movimientoRestante -= costoMovimiento;

                AgregarLog($"Te mueves {costoMovimiento} casilla(s). Movimiento restante: {movimientoRestante}", Color.Cyan);

                LimpiarCasillasResaltadas();
                ActualizarTablero();
                ActualizarUI();
            }
        }

        private void RealizarAtaqueMelee()
        {
            int distancia = CalcularDistancia(posicionJugador, posicionEnemigo);
            if (distancia > 1)
            {
                AgregarLog("El enemigo no está al alcance para ataque cuerpo a cuerpo", Color.Orange);
                return;
            }

            Random rand = new Random();
            int tiradaAtaque = rand.Next(1, 21);
            int modificadorAtaque = (jugador.STR - 10) / 2;
            int totalAtaque = tiradaAtaque + modificadorAtaque;

            AgregarLog($"Tirada de ataque cuerpo a cuerpo: {tiradaAtaque} + {modificadorAtaque} (FUE) = {totalAtaque}", Color.Yellow);

            if (totalAtaque >= 12) // CA del enemigo
            {
                int daño = rand.Next(1, 8) + modificadorAtaque;
                daño = Math.Max(1, daño);
                hpEnemigo -= daño;

                AgregarLog($"¡Golpeas! Infliges {daño} puntos de daño", Color.LimeGreen);

                if (hpEnemigo <= 0)
                {
                    hpEnemigo = 0;
                    Victoria();
                    return;
                }
            }
            else
            {
                AgregarLog("Fallaste el ataque", Color.OrangeRed);
            }

            accionDisponible--;
            ActualizarUI();
        }

        private void RealizarAtaqueDistancia()
        {
            int distancia = CalcularDistancia(posicionJugador, posicionEnemigo);
            int alcanceMaximo = 6; // Alcance base para ataques a distancia

            if (distancia > alcanceMaximo)
            {
                AgregarLog("El enemigo está fuera del alcance", Color.Orange);
                return;
            }

            Random rand = new Random();
            int tiradaAtaque = rand.Next(1, 21);
            int modificadorAtaque = (jugador.DEX - 10) / 2;
            int totalAtaque = tiradaAtaque + modificadorAtaque;

            AgregarLog($"Tirada de ataque a distancia: {tiradaAtaque} + {modificadorAtaque} (DES) = {totalAtaque}", Color.Yellow);

            if (totalAtaque >= 13) // CA del enemigo +1 por distancia
            {
                int daño = rand.Next(1, 6) + modificadorAtaque;
                daño = Math.Max(1, daño);
                hpEnemigo -= daño;

                AgregarLog($"¡Impacto a distancia! Infliges {daño} puntos de daño", Color.LightBlue);

                if (hpEnemigo <= 0)
                {
                    hpEnemigo = 0;
                    Victoria();
                    return;
                }
            }
            else
            {
                AgregarLog("Fallaste el ataque a distancia", Color.OrangeRed);
            }

            accionDisponible--;
            ActualizarUI();
        }

        private void UsarHabilidadEspecial()
        {
            Random rand = new Random();
            int daño = rand.Next(3, 10) + Math.Max(0, (jugador.INT - 10) / 2);
            hpEnemigo -= daño;

            AgregarLog($"¡Usas {ObtenerHabilidadClase()}! Infliges {daño} puntos de daño mágico", Color.MediumPurple);

            if (hpEnemigo <= 0)
            {
                hpEnemigo = 0;
                Victoria();
                return;
            }

            accionDisponible--;
            ActualizarUI();
        }

        private void Defender()
        {
            AgregarLog("Te pones en guardia. +2 a CA hasta tu próximo turno", Color.Green);
            accionDisponible = 0;
            ActualizarUI();
        }

        private void TerminarTurnoJugador()
        {
            AgregarLog("Terminas tu turno", Color.Gray);
            turnoJugador = false;
            Application.DoEvents(); // Actualizar UI
            System.Threading.Thread.Sleep(500);
            TurnoEnemigo();
        }

        private void TurnoEnemigo()
        {
            lblTurno.Text = "TURNO: ENEMIGO";
            lblTurno.ForeColor = Color.Red;
            AgregarLog("--- TURNO DEL ENEMIGO ---", Color.OrangeRed);

            int distancia = CalcularDistancia(posicionJugador, posicionEnemigo);

            // Mover enemigo si está lejos
            if (distancia > 1)
            {
                MoverEnemigoHaciaJugador();
                distancia = CalcularDistancia(posicionJugador, posicionEnemigo);
            }

            // Atacar si está en rango
            if (distancia <= 2) // Alcance aumentado para que el enemigo pueda atacar más fácilmente
            {
                RealizarAtaqueEnemigo();
            }
            else
            {
                AgregarLog("La criatura no puede alcanzarte este turno", Color.Orange);
            }

            Application.DoEvents();
            System.Threading.Thread.Sleep(1500);

            // Preparar siguiente turno del jugador
            turnoJugador = true;
            movimientoRestante = CalcularMovimientoJugador();
            accionDisponible = 1;

            lblTurno.Text = "TURNO: JUGADOR";
            lblTurno.ForeColor = Color.LimeGreen;
            AgregarLog("--- TU TURNO ---", Color.LimeGreen);

            CalcularCasillasMovimiento();
            ActualizarUI();
        }

        private void MoverEnemigoHaciaJugador()
        {
            Point mejorMovimiento = posicionEnemigo;
            int mejorDistancia = CalcularDistancia(posicionEnemigo, posicionJugador);

            Point[] direcciones = {
                new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1),
                new Point(1, 1), new Point(-1, 1), new Point(1, -1), new Point(-1, -1)
            };

            foreach (Point dir in direcciones)
            {
                Point nuevaPos = new Point(posicionEnemigo.X + dir.X, posicionEnemigo.Y + dir.Y);

                if (EsPosicionValida(nuevaPos) && tablero[nuevaPos.Y, nuevaPos.X] == 0)
                {
                    int nuevaDistancia = CalcularDistancia(nuevaPos, posicionJugador);
                    if (nuevaDistancia < mejorDistancia)
                    {
                        mejorDistancia = nuevaDistancia;
                        mejorMovimiento = nuevaPos;
                    }
                }
            }

            if (mejorMovimiento != posicionEnemigo)
            {
                tablero[posicionEnemigo.Y, posicionEnemigo.X] = 0;
                tablero[mejorMovimiento.Y, mejorMovimiento.X] = 2;
                posicionEnemigo = mejorMovimiento;
                AgregarLog("La criatura se mueve hacia ti", Color.Orange);
            }

            ActualizarTablero();
        }

        private void RealizarAtaqueEnemigo()
        {
            Random rand = new Random();
            int tiradaAtaque = rand.Next(1, 21);
            int totalAtaque = tiradaAtaque + 3; // Bonificación base del enemigo

            AgregarLog($"La criatura ataca: {tiradaAtaque} + 3 = {totalAtaque}", Color.Orange);

            if (totalAtaque >= jugador.CA)
            {
                int daño = rand.Next(1, 8) + 2; // Daño del enemigo
                hpJugador -= daño;
                AgregarLog($"¡Te golpea! Recibes {daño} puntos de daño", Color.Red);

                if (hpJugador <= 0)
                {
                    hpJugador = 0;
                    Derrota();
                    return;
                }
            }
            else
            {
                AgregarLog("Esquivas el ataque enemigo", Color.LightGreen);
            }
        }

        // MÉTODOS UTILITARIOS
        private int CalcularDistancia(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private int CalcularCostoMovimiento(Point desde, Point hasta)
        {
            return CalcularDistancia(desde, hasta);
        }

        private bool EsPosicionValida(Point pos)
        {
            return pos.X >= 0 && pos.X < COLUMNAS_TABLERO &&
                   pos.Y >= 0 && pos.Y < FILAS_TABLERO;
        }

        private bool EstaEnAlcanceMelee(Point objetivo)
        {
            return CalcularDistancia(posicionJugador, objetivo) <= 1;
        }

        private bool EstaEnAlcanceDistancia(Point objetivo)
        {
            int alcance = 6; // Alcance base para ataques a distancia
            return CalcularDistancia(posicionJugador, objetivo) <= alcance;
        }

        private string ObtenerHabilidadClase()
        {
            return jugador.CLASE?.ToUpper() switch
            {
                "GUERRERO" => "Golpe Poderoso",
                "MAGO" => "Bola de Fuego",
                "PÍCARO" => "Ataque Furtivo",
                "CLÉRIGO" => "Rayo Sagrado",
                _ => "Habilidad Especial"
            };
        }

        private void CalcularCasillasMovimiento()
        {
            casillasMovimiento.Clear();

            for (int x = 0; x < COLUMNAS_TABLERO; x++)
            {
                for (int y = 0; y < FILAS_TABLERO; y++)
                {
                    Point pos = new Point(x, y);
                    int costo = CalcularCostoMovimiento(posicionJugador, pos);

                    if (costo <= movimientoRestante && EsPosicionValida(pos) &&
                        tablero[y, x] == 0 && !obstaculos.Contains(pos))
                    {
                        casillasMovimiento.Add(pos);
                    }
                }
            }

            ResaltarCasillasMovimiento();
        }

        private void CalcularCasillasAlcanceMelee()
        {
            casillasAlcance.Clear();
            Point[] direcciones = {
                new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1)
            };

            foreach (Point dir in direcciones)
            {
                Point pos = new Point(posicionJugador.X + dir.X, posicionJugador.Y + dir.Y);
                if (EsPosicionValida(pos) && tablero[pos.Y, pos.X] == 2)
                {
                    casillasAlcance.Add(pos);
                }
            }

            ResaltarCasillasAlcance();
        }

        private void CalcularCasillasAlcanceDistancia()
        {
            casillasAlcance.Clear();
            int alcance = 6;

            for (int x = 0; x < COLUMNAS_TABLERO; x++)
            {
                for (int y = 0; y < FILAS_TABLERO; y++)
                {
                    Point pos = new Point(x, y);
                    if (tablero[y, x] == 2 && CalcularDistancia(posicionJugador, pos) <= alcance)
                    {
                        casillasAlcance.Add(pos);
                    }
                }
            }

            ResaltarCasillasAlcance();
        }

        private void ResaltarCasillasMovimiento()
        {
            foreach (Point pos in casillasMovimiento)
            {
                casillas[pos.Y, pos.X].BackColor = Color.FromArgb(80, 160, 255);
                casillas[pos.Y, pos.X].FlatAppearance.BorderColor = Color.Cyan;
                casillas[pos.Y, pos.X].FlatAppearance.BorderSize = 2;
            }
        }

        private void ResaltarCasillasAlcance()
        {
            foreach (Point pos in casillasAlcance)
            {
                casillas[pos.Y, pos.X].BackColor = Color.FromArgb(255, 100, 100);
                casillas[pos.Y, pos.X].FlatAppearance.BorderColor = Color.Red;
                casillas[pos.Y, pos.X].FlatAppearance.BorderSize = 3;
            }
        }

        private void LimpiarCasillasResaltadas()
        {
            for (int fila = 0; fila < FILAS_TABLERO; fila++)
            {
                for (int col = 0; col < COLUMNAS_TABLERO; col++)
                {
                    if ((col + fila) % 2 == 0)
                        casillas[fila, col].BackColor = Color.FromArgb(45, 48, 55);
                    else
                        casillas[fila, col].BackColor = Color.FromArgb(35, 38, 45);

                    casillas[fila, col].FlatAppearance.BorderSize = 1;
                    casillas[fila, col].FlatAppearance.BorderColor = Color.FromArgb(50, 50, 60);
                }
            }
        }

        private void ActualizarTablero()
        {
            for (int fila = 0; fila < FILAS_TABLERO; fila++)
            {
                for (int col = 0; col < COLUMNAS_TABLERO; col++)
                {
                    Button casilla = casillas[fila, col];
                    casilla.Text = "";
                    casilla.ForeColor = Color.White;

                    switch (tablero[fila, col])
                    {
                        case 1: // Jugador
                            casilla.BackColor = Color.FromArgb(70, 130, 180);
                            casilla.Text = "J";
                            casilla.ForeColor = Color.White;
                            break;
                        case 2: // Enemigo
                            casilla.BackColor = Color.FromArgb(178, 34, 34);
                            casilla.Text = "E";
                            casilla.ForeColor = Color.White;
                            break;
                        case 3: // Obstáculo
                            casilla.BackColor = Color.FromArgb(101, 67, 33);
                            casilla.Text = "▓";
                            break;
                    }
                }
            }
        }

        private void ActualizarUI()
        {
            lblMovimiento.Text = $"MOVIMIENTO: {movimientoRestante}";
            lblAccion.Text = $"ACCIÓN: {accionDisponible}";
        }

        private void Victoria()
        {
            AgregarLog($"¡HAS DERROTADO A LA CRIATURA HOSTIL!", Color.Gold);
            AgregarLog("La victoria es tuya, valiente aventurero.", Color.LimeGreen);

            int oroGanado = new Random().Next(30, 80);
            jugador.DIN += oroGanado;
            AgregarLog($"¡Obtienes {oroGanado} piezas de oro del botín!", Color.Yellow);

            JugadorGano = true;
            HPRestanteJugador = hpJugador;

            // Actualizar estadísticas en la base de datos
            BASE_DE_DATOS.ActualizarDinero(jugador.ID, jugador.DIN);

            System.Threading.Thread.Sleep(3000);
            this.Close();
        }

        private void Derrota()
        {
            AgregarLog("HAS CAÍDO EN COMBATE...", Color.Red);
            AgregarLog("Los dioses te conceden otra oportunidad. Serás transportado a un santuario seguro.", Color.Gray);

            JugadorGano = false;
            HPRestanteJugador = Math.Max(1, hpMaxJugador / 2); // Revive con al menos 1 HP
            jugador.DIN = Math.Max(0, jugador.DIN - 25);

            // Actualizar estadísticas en la base de datos
            BASE_DE_DATOS.ActualizarDinero(jugador.ID, jugador.DIN);

            System.Threading.Thread.Sleep(3000);
            this.Close();
        }

        private void AgregarLog(string mensaje, Color color)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action<string, Color>(AgregarLog), mensaje, color);
                return;
            }

            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.SelectionLength = 0;
            txtLog.SelectionColor = color;
            txtLog.AppendText($"> {mensaje}\n");
            txtLog.SelectionColor = txtLog.ForeColor;
            txtLog.ScrollToCaret();
        }

        private void IniciarCombate()
        {
            AgregarLog("¡EL ENCUENTRO TÁCTICO HA COMENZADO!", Color.Yellow);
            AgregarLog($"{jugador.NOMBRE} se enfrenta a una criatura hostil.", Color.White);
            AgregarLog("¡Es tu turno! Usa MOVIMIENTO para acercarte y ATAQUE MELEE para atacar.", Color.LimeGreen);

            CalcularCasillasMovimiento();
            ActualizarUI();
        }

        // MÉTODOS PARA REDONDEO (similares a PANTALLA_JUEGO)
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

        private void RedondearControl(Control control, int radio)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(control.Width - radio, 0, radio, radio, 270, 90);
            path.AddArc(control.Width - radio, control.Height - radio, radio, radio, 0, 90);
            path.AddArc(0, control.Height - radio, radio, radio, 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                var resultado = MessageBox.Show("¿Abandonar el combate? Esto se considerará una derrota.",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    JugadorGano = false;
                    HPRestanteJugador = Math.Max(1, hpJugador);
                    this.Close();
                }
            }
            base.OnKeyDown(e);
        }
    }
}