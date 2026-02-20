using proyecto;
using System.Data.Entity;
using System.Data.SQLite;
using static Mysqlx.Notice.Warning.Types;

namespace PROYECTO_5TO___TOTЯ
{
    public class PANTALLA_JUEGO : Form
    {
        private Personaje pj;
        private Panel panelMapa;
        private TableLayoutPanel bottomLayout;
        private PictureBox picPersonaje;
        private int[,] mapa;
        private Point posicionJugador;

        private Point camaraPos = new Point(0, 0);
        private int tamañoCelda = 40;
        private int filasVisibles, columnasVisibles;

        private BufferedGraphicsContext contextoBuffer;
        private BufferedGraphics buffer;
        private Bitmap mapaBuffer;
        private Point posicionAntesDeTienda = Point.Empty;
        private Panel panelRecompensaCofre;
        private System.Windows.Forms.Timer timerCofre;

        private static Dictionary<string, Image> cacheImagenes = new Dictionary<string, Image>();
        private Dictionary<int, Image> spritesTerreno = new Dictionary<int, Image>();
        private Dictionary<string, Label> statLabels = new Dictionary<string, Label>();
        private int[,] mapaBase;
        private int[,] mapaObjetos;
        private Dictionary<int, Image> spritesObjetos = new Dictionary<int, Image>();

        private List<ENEMIGOS> enemigos = new List<ENEMIGOS>();
        private System.Windows.Forms.Timer timerEnemigos = new System.Windows.Forms.Timer();

        private Label lblHP;
        private Label lblCA;
        private Label lblVEL;
        private Label lblINI;
        private Label[,] lblStats;


        public PANTALLA_JUEGO(Personaje personaje)
        {
            pj = personaje;

            InicializarFormulario();
            CrearInterfaz();
            panelMapa.DoubleBuffered(true);
            InicializarMapa();
            InicializarEnemigos();
            CargarSpritesTerreno();
            MostrarPersonaje();
            GenerarMapaBuffer();
        }

        private void InicializarFormulario()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
            Text = "Pantalla de Juego";
            BackColor = Color.Black;
            contextoBuffer = BufferedGraphicsManager.Current;

            timerEnemigos.Interval = 1000;
            timerEnemigos.Tick += (s, e) => MoverEnemigos();
            timerEnemigos.Start();
        }




        private void CrearInterfaz()
        {
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            Controls.Add(mainLayout);

            panelMapa = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black
            };
            panelMapa.Paint += DibujarMapa;
            panelMapa.Resize += (s, e) => CalcularCeldasVisibles();
            mainLayout.Controls.Add(panelMapa, 0, 0);

            bottomLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 1,
                BackColor = Color.FromArgb(18, 18, 22),
                Padding = new Padding(8)
            };
            mainLayout.Controls.Add(bottomLayout, 0, 1);

            CrearMenuInferior();
        }


        private void CalcularCeldasVisibles()
        {
            columnasVisibles = (int)Math.Ceiling((double)panelMapa.Width / tamañoCelda);
            filasVisibles = (int)Math.Ceiling((double)panelMapa.Height / tamañoCelda);

            if (panelMapa.Width > 0 && panelMapa.Height > 0)
            {
                buffer?.Dispose();
                buffer = contextoBuffer.Allocate(panelMapa.CreateGraphics(), panelMapa.DisplayRectangle);
            }
            panelMapa.Invalidate();
        }




        private void CrearMenuInferior()
        {
            bottomLayout.Controls.Clear();
            TableLayoutPanel layoutPrincipal = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };

            layoutPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8F));
            layoutPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            layoutPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            layoutPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            layoutPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));

            layoutPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bottomLayout.Controls.Add(layoutPrincipal);

            Panel panelPersonaje = new Panel
            {
                BackColor = Color.FromArgb(28, 30, 35),
                Margin = new Padding(4),
                Width = 140,
                Height = 175,
            };
            RedondearMenu(panelPersonaje, 30);
            layoutPrincipal.Controls.Add(panelPersonaje, 0, 0);
            AgregarSeccionPersonaje(panelPersonaje);

            Panel panelInfo = new Panel
            {
                BackColor = Color.FromArgb(28, 30, 35),
                Margin = new Padding(4),
                Width = 785,
                Height = 175,
            };
            RedondearMenu(panelInfo, 30);
            layoutPrincipal.Controls.Add(panelInfo, 1, 0);
            AgregarSeccionInfo(panelInfo);

            Panel panelAtributos = new Panel
            {
                BackColor = Color.FromArgb(28, 30, 35),
                Margin = new Padding(4),
                Width = 370,
                Height = 175,
            };
            RedondearMenu(panelAtributos, 30);
            layoutPrincipal.Controls.Add(panelAtributos, 2, 0);
            AgregarSeccionAtributos(panelAtributos);

            Panel panelCombate = new Panel
            {
                BackColor = Color.FromArgb(28, 30, 35),
                Margin = new Padding(4),
                Width = 275,
                Height = 175,
            };
            RedondearMenu(panelCombate, 30);
            layoutPrincipal.Controls.Add(panelCombate, 3, 0);
            AgregarSeccionCombate(panelCombate);

            Panel panelAcciones = new Panel
            {
                BackColor = Color.FromArgb(28, 30, 35),
                Margin = new Padding(4),
                Width = 277,
                Height = 175,
            };
            RedondearMenu(panelAcciones, 30);
            layoutPrincipal.Controls.Add(panelAcciones, 4, 0);
            AgregarSeccionAcciones(panelAcciones);
        }


        private Panel CrearPanelSeccion()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(28, 30, 35),
                Margin = new Padding(4),

            };
            RedondearMenu(panel, 12);
            return panel;
        }

        private void AgregarSeccionPersonaje(Panel panel)
        {
            picPersonaje = new PictureBox
            {
                Size = new Size(100, 160),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.None
            };
            panel.Controls.Add(picPersonaje);
            panel.Resize += (s, e) =>
            {
                picPersonaje.Location = new Point(
                    (panel.Width - picPersonaje.Width) / 2,
                    (panel.Height - picPersonaje.Height) / 2
                );
            };
            CargarImagenPersonajeSegura();
        }

        private void AgregarSeccionInfo(Panel panel)
        {
            Label titulo = CrearTituloSeccion("INFORMACION", Color.FromArgb(120, 200, 255));
            panel.Controls.Add(titulo);

            TableLayoutPanel grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 3,
                Padding = new Padding(10, 35, 10, 10)
            };

            for (int i = 0; i < 3; i++)
                grid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            for (int i = 0; i < 3; i++)
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            panel.Controls.Add(grid);

            string[,] etiquetas = {
                { $"NMB: {pj.NOMBRE}", $"CLS: {pj.CLASE}", $"LVL: {pj.LVL}" },
                { $"RZA: {pj.RAZA}", $"TRF: {pj.TRASFONDO}", $"DIN: {pj.DIN}" },
                { $"SBZ: {pj.SUBRAZA}", $"ALN: {pj.ALINEAMIENTO}", "DAY: 67" }
                };

                    Color[,] colores = {
                { Color.Gold, Color.LightBlue, Color.LightPink },
                { Color.LightGray, Color.LimeGreen, Color.Orange },
                { Color.LightCyan, Color.LightSalmon, Color.Transparent }
                };

            lblStats = new Label[3, 3];

            for (int fila = 0; fila < 3; fila++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (string.IsNullOrEmpty(etiquetas[fila, col])) continue;

                    Panel itemInfo = new Panel
                    {
                        Dock = DockStyle.Fill,
                        BackColor = Color.Transparent,
                        Padding = new Padding(5)
                    };

                    Label lblEtiqueta = new Label
                    {
                        Text = etiquetas[fila, col],
                        Font = FUENTE.ObtenerFont(10),
                        ForeColor = Color.Gray,
                        AutoSize = false,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(5, 2)
                    };

                    itemInfo.Controls.Add(lblEtiqueta);
                    grid.Controls.Add(itemInfo, col, fila);

                    lblStats[fila, col] = lblEtiqueta; 
                }
            }
        }

        private void ActualizarDineroLabel()
        {
            if (lblStats != null && lblStats[1, 2] != null)
            {
                lblStats[1, 2].Text = $"DIN: {pj.DIN}";
            }


        }

        private void AgregarSeccionAtributos(Panel panel)
        {
            Label titulo = CrearTituloSeccion("ATRIBUTOS", Color.FromArgb(255, 180, 100));
            panel.Controls.Add(titulo);

            TableLayoutPanel grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 3,
                Padding = new Padding(8, 35, 8, 8)
            };

            for (int i = 0; i < 3; i++)
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            for (int i = 0; i < 2; i++)
                grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            panel.Controls.Add(grid);

            string[] nombres = { "STR", "DEX", "CON", "INT", "WIS", "CHA" };
            int[] valores = { pj.STR, pj.DEX, pj.CON, pj.INT, pj.WIS, pj.CHA };

            for (int i = 0; i < 6; i++)
            {
                Panel cajaAtrib = CrearCajaAtributo(nombres[i], valores[i]);
                grid.Controls.Add(cajaAtrib, i % 3, i / 3);
            }
        }

        private Panel CrearCajaAtributo(string nombre, int valor)
        {
            Panel caja = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(35, 38, 45),
                Margin = new Padding(3)
            };
            RedondearMenu(caja, 8);

            Label lblNombre = new Label
            {
                Text = nombre,
                Font = FUENTE.ObtenerFont(10),
                ForeColor = Color.LightGray,
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 5, 0, 0)
            };

            Label lblValor = new Label
            {
                Text = valor.ToString(),
                Font = FUENTE.ObtenerFont(20),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            int mod = (valor - 10) / 2;
            Label lblMod = new Label
            {
                Text = (mod >= 0 ? "+" : "") + mod.ToString(),
                Font = FUENTE.ObtenerFont(9),
                ForeColor = mod >= 0 ? Color.LimeGreen : Color.OrangeRed,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            caja.Resize += (s, e) =>
            {
                lblMod.Location = new Point(caja.Width - lblMod.Width - 8, 5);
            };

            caja.Controls.AddRange(new Control[] { lblValor, lblNombre, lblMod });
            lblMod.BringToFront();
            return caja;
        }

        private void AgregarSeccionCombate(Panel panel)
        {
            Label titulo = CrearTituloSeccion("COMBATE", Color.FromArgb(255, 100, 100));
            panel.Controls.Add(titulo);

            TableLayoutPanel grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2,
                Padding = new Padding(8, 35, 8, 8)
            };

            for (int i = 0; i < 2; i++)
            {
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            }

            panel.Controls.Add(grid);

            string[] nombres = { "HP", "CA", "VEL", "INI" };
            int[] valores = { pj.HP, pj.CA, pj.VEL, pj.INI };
            Color[] colores = {
                Color.FromArgb(255, 80, 80),
                Color.FromArgb(100, 150, 255),
                Color.FromArgb(150, 255, 150),
                Color.FromArgb(255, 200, 100)
            };

            for (int i = 0; i < 4; i++)
            {
                Panel cajaStat = CrearCajaCombate(nombres[i], valores[i], colores[i]);

                Label lblValor = cajaStat.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblValor");

                if (lblValor != null)
                {
                    statLabels[nombres[i]] = lblValor;
                    System.Diagnostics.Debug.WriteLine($"✓ Label '{nombres[i]}' guardado correctamente");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"✗ ERROR: No se encontró lblValor para '{nombres[i]}'");
                }
                grid.Controls.Add(cajaStat, i % 2, i / 2);
            }
        }

        private Panel CrearCajaCombate(string nombre, int valor, Color colorAccent)
        {
            Panel caja = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(35, 38, 45),
                Margin = new Padding(3)
            };
            RedondearMenu(caja, 8);

            Panel barra = new Panel
            {
                Width = 4,
                Dock = DockStyle.Left,
                BackColor = colorAccent
            };
            caja.Controls.Add(barra);

            Label lblNombre = new Label
            {
                Text = nombre,
                Font = FUENTE.ObtenerFont(10),
                ForeColor = Color.LightGray,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 5, 0, 0)
            };

            Label lblValor = new Label
            {
                Name = "lblValor",
                Text = valor.ToString(),
                Font = FUENTE.ObtenerFont(22),
                ForeColor = colorAccent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(0, 0, 0, 8)
            };

            caja.Controls.AddRange(new Control[] { lblValor, lblNombre });
            return caja;
        }

        private void AgregarSeccionAcciones(Panel panel)
        {
            Label titulo = CrearTituloSeccion("ACCIONES", Color.FromArgb(180, 180, 180));
            panel.Controls.Add(titulo);

            TableLayoutPanel grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2,
                Padding = new Padding(8, 35, 8, 8)
            };

            for (int i = 0; i < 2; i++)
            {
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            }

            panel.Controls.Add(grid);

            string[] iconos = { "MENU.png", "INVENTARIO.png", "MAPA.png", "MISIONES.png" };
            EventHandler[] eventos = {
                (s, e) => MostrarMenuPausa(),
                (s, e) => MostrarMenuInventario(),
                (s, e) => MostrarMenuMapa(),
                (s, e) => MostrarMenuMisiones()
            };

            for (int i = 0; i < 4; i++)
            {
                Button btn = CrearBotonAccion(iconos[i], eventos[i]);
                grid.Controls.Add(btn, i % 2, i / 2);
            }
        }

        private Button CrearBotonAccion(string nombreIcono, EventHandler evento)
        {
            Button btn = new Button
            {
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(45, 48, 55),
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Margin = new Padding(3),
                Width = 150,
                Height = 150
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 63, 70);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 53, 60);
            RedondearBoton(btn, 8);

            string ruta = Path.Combine(Application.StartupPath, "Resources", nombreIcono);
            if (File.Exists(ruta))
            {
                try
                {
                    PictureBox pic = new PictureBox
                    {
                        Image = Image.FromFile(ruta),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Size = new Size(35, 35),
                        BackColor = Color.Transparent,
                        Enabled = false
                    };
                    btn.Controls.Add(pic);
                    btn.Resize += (s, e) => pic.Location = new Point((btn.Width - pic.Width) / 2, (btn.Height - pic.Height) / 2);
                }
                catch { }
            }

            btn.Click += evento;
            return btn;
        }

        private Label CrearTituloSeccion(string texto, Color color)
        {
            return new Label
            {
                Text = texto,
                Font = FUENTE.ObtenerFont(11),
                ForeColor = color,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 28,
                BackColor = Color.Transparent
            };
        }

        private void CargarImagenPersonajeSegura()
        {
            string rutaBase = Path.Combine(Application.StartupPath, "Resources");
            string ruta = Path.Combine(rutaBase, "DEFAULT.PNG");

            string raza = pj.RAZA?.ToUpper() ?? "";
            string subraza = pj.SUBRAZA?.ToUpper() ?? "";
            string clase = pj.CLASE?.ToUpper() ?? "";

            if (raza == "HUMANO") ruta = Path.Combine(rutaBase, $"HUMANO {clase}.PNG");
            else if (raza == "ORCO") ruta = Path.Combine(rutaBase, $"ORCO {clase}.PNG");
            else if (raza == "ELFO") ruta = Path.Combine(rutaBase, $"{subraza} {clase}.PNG");
            else if (raza == "ENANO") ruta = Path.Combine(rutaBase, $"{subraza} {clase}.PNG");

            if (!File.Exists(ruta)) ruta = Path.Combine(rutaBase, "DEFAULT.PNG");
            if (!File.Exists(ruta)) return;

            int ancho = picPersonaje.Width > 0 ? picPersonaje.Width : 120;
            int alto = picPersonaje.Height > 0 ? picPersonaje.Height : 180;

            if (picPersonaje.Image != null)
            {
                picPersonaje.Image.Dispose();
                picPersonaje.Image = null;
            }

            picPersonaje.Image = CargarImagenOpt(ruta, ancho, alto);
        }

        private Image CargarImagenOpt(string ruta, int ancho, int alto)
        {
            try
            {
                using (var fs = new FileStream(ruta, FileMode.Open, FileAccess.Read))
                using (var imgTemp = Image.FromStream(fs))
                {
                    Bitmap bmp = new Bitmap(ancho, alto);
                    using (Graphics g = Graphics.FromImage(bmp))
                        g.DrawImage(imgTemp, 0, 0, ancho, alto);
                    return bmp;
                }
            }
            catch { return new Bitmap(ancho, alto); }
        }











































        private void InicializarMapa()
        {
            int filas = 100;
            int columnas = 100;

            mapaBase = new int[filas, columnas];
            mapaObjetos = new int[filas, columnas];
            mapa = new int[filas, columnas];

            // INICIALIZAR TODO CON PASTO
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    mapaBase[i, j] = 0;
                    mapaObjetos[i, j] = -1;
                    mapa[i, j] = 0;
                }
            }

            // CREAR ZONAS ESTILO ZELDA
            CrearPuebloKakariko();
            CrearBosquePerdido();
            CrearLagoHylia();
            CrearMontañaDeLaMuerte();
            CrearTemploDelTiempo();
            CrearCastilloHyrule();
            CrearDesierto();
            CrearCementerio();
            CrearCuevasSecretas();
            CrearRioYPuentes();
            CrearCaminosConectores();
            CrearZonaSecretaNorte();
            CrearIslaFlotante();
            AgregarElementosDecorativos();

            posicionJugador = new Point(48, 48); // INICIO EN EL CENTRO
            CalcularCeldasVisibles();
        }

        // ==========================================
        // PUEBLO KAKARIKO (ZONA INICIAL)
        // ==========================================
        private void CrearPuebloKakariko()
        {
            // PERÍMETRO DEL PUEBLO CON MUROS
            for (int i = 42; i <= 54; i++)
            {
                for (int j = 42; j <= 54; j++)
                {
                    if (i == 42 || i == 54 || j == 42 || j == 54)
                    {
                        mapaBase[i, j] = 4;
                        mapaObjetos[i, j] = 1; // MUROS
                    }
                }
            }

            // ENTRADAS DEL PUEBLO (4 ENTRADAS)
            mapaObjetos[42, 48] = -1; // NORTE
            mapaObjetos[54, 48] = -1; // SUR
            mapaObjetos[48, 42] = -1; // OESTE
            mapaObjetos[48, 54] = -1; // ESTE

            // PLAZA CENTRAL CON CAMINOS
            for (int i = 47; i <= 49; i++)
            {
                for (int j = 43; j <= 53; j++)
                {
                    mapaBase[i, j] = 4;
                }
            }
            for (int i = 43; i <= 53; i++)
            {
                for (int j = 47; j <= 49; j++)
                {
                    mapaBase[i, j] = 4;
                }
            }

            // EDIFICIOS DEL PUEBLO
            mapaObjetos[45, 45] = 8; // TIENDA
            mapaObjetos[45, 51] = 8; // TIENDA 2
            mapaObjetos[51, 45] = 7; // NPC SABIO
            mapaObjetos[51, 51] = 9; // CASA CON PUERTA

            // DECORACIONES
            mapaObjetos[44, 44] = 5; // ÁRBOL
            mapaObjetos[44, 52] = 5;
            mapaObjetos[52, 44] = 5;
            mapaObjetos[52, 52] = 5;

            // COFRES Y NPCS
            mapaObjetos[48, 48] = 6; // COFRE CENTRAL
            mapaObjetos[46, 48] = 7; // NPC
            mapaObjetos[50, 48] = 7; // NPC

            // FAROLAS
            mapaObjetos[47, 45] = 25;
            mapaObjetos[47, 51] = 25;
            mapaObjetos[49, 45] = 25;
            mapaObjetos[49, 51] = 25;
        }

        // ==========================================
        // BOSQUE PERDIDO (LABERINTO)
        // ==========================================
        private void CrearBosquePerdido()
        {
            // ZONA DEL BOSQUE
            for (int i = 10; i <= 35; i++)
            {
                for (int j = 10; j <= 35; j++)
                {
                    mapaBase[i, j] = 0;
                    if ((i + j) % 3 == 0)
                        mapaObjetos[i, j] = 5; // ÁRBOLES DENSOS
                }
            }

            // CREAR LABERINTO DE CAMINOS
            int[] caminoX = { 10, 15, 15, 20, 20, 25, 25, 30, 30, 35 };
            int[] caminoY = { 23, 23, 18, 18, 25, 25, 15, 15, 20, 20 };

            for (int k = 0; k < caminoX.Length - 1; k++)
            {
                int x1 = caminoX[k], y1 = caminoY[k];
                int x2 = caminoX[k + 1], y2 = caminoY[k + 1];

                while (x1 != x2 || y1 != y2)
                {
                    mapaBase[x1, y1] = 4;
                    mapaObjetos[x1, y1] = -1;

                    if (x1 < x2) x1++;
                    else if (x1 > x2) x1--;
                    if (y1 < y2) y1++;
                    else if (y1 > y2) y1--;
                }
            }

            // CLARO EN EL CENTRO DEL BOSQUE
            for (int i = 20; i <= 25; i++)
            {
                for (int j = 20; j <= 25; j++)
                {
                    mapaBase[i, j] = 0;
                    mapaObjetos[i, j] = -1;
                }
            }

            // MASTER SWORD (PUERTA ESPECIAL)
            mapaObjetos[23, 23] = 9;
            mapaObjetos[22, 23] = 7; // NPC GUARDIÁN
            mapaObjetos[24, 23] = 6; // COFRE

            // FLORES ALREDEDOR
            mapaObjetos[21, 21] = 22;
            mapaObjetos[21, 24] = 22;
            mapaObjetos[24, 21] = 22;
            mapaObjetos[24, 24] = 22;
        }

        // ==========================================
        // LAGO HYLIA
        // ==========================================
        private void CrearLagoHylia()
        {
            // GRAN LAGO CIRCULAR
            for (int i = 60; i <= 85; i++)
            {
                for (int j = 15; j <= 40; j++)
                {
                    double distX = j - 27.5;
                    double distY = i - 72.5;
                    double dist = Math.Sqrt(distX * distX + distY * distY);

                    if (dist < 12)
                        mapaBase[i, j] = 3; // AGUA
                    else if (dist < 14)
                        mapaBase[i, j] = 2; // ARENA/PLAYA
                }
            }

            // ISLA EN EL CENTRO
            for (int i = 70; i <= 75; i++)
            {
                for (int j = 25; j <= 30; j++)
                {
                    mapaBase[i, j] = 0;
                    if (i == 70 || i == 75 || j == 25 || j == 30)
                        mapaObjetos[i, j] = 1; // MUROS
                }
            }
            mapaObjetos[73, 28] = 9; // TEMPLO DEL AGUA
            mapaObjetos[72, 27] = 6; // COFRE
            mapaObjetos[72, 29] = 6; // COFRE

            // PUENTE HACIA LA ISLA
            for (int i = 72; i >= 65; i--)
            {
                mapaBase[i, 27] = 4;
                mapaBase[i, 28] = 4;
            }

            // DECORACIONES EN LA PLAYA
            mapaObjetos[62, 20] = 7; // NPC PESCADOR
            mapaObjetos[63, 21] = 23; // ROCA
            mapaObjetos[64, 35] = 8; // TIENDA
        }

        // ==========================================
        // MONTAÑA DE LA MUERTE
        // ==========================================
        private void CrearMontañaDeLaMuerte()
        {
            // ZONA ROCOSA
            for (int i = 8; i <= 40; i++)
            {
                for (int j = 65; j <= 92; j++)
                {
                    if ((i + j) % 2 == 0)
                        mapaBase[i, j] = 1; // PIEDRA
                }
            }

            // CAMINO SERPENTEANTE
            int[] montX = { 35, 32, 29, 26, 23, 20, 17, 14, 11, 8 };
            int[] montY = { 65, 68, 71, 74, 77, 80, 83, 86, 89, 92 };

            for (int k = 0; k < montX.Length; k++)
            {
                for (int d = -1; d <= 1; d++)
                {
                    if (montX[k] + d >= 0 && montX[k] + d < 100)
                    {
                        mapaBase[montX[k] + d, montY[k]] = 4;
                        mapaObjetos[montX[k] + d, montY[k]] = -1;
                    }
                }
            }

            // ENTRADA A LA CUEVA DEL DRAGÓN
            mapaObjetos[8, 92] = 9;
            mapaObjetos[7, 92] = 1;
            mapaObjetos[9, 92] = 1;
            mapaObjetos[8, 91] = 1;
            mapaObjetos[8, 93] = 1;

            // COFRES Y NPCS EN EL CAMINO
            mapaObjetos[26, 74] = 6;
            mapaObjetos[20, 80] = 6;
            mapaObjetos[14, 86] = 7; // NPC ESCALADOR
            mapaObjetos[32, 68] = 23; // ROCA DECORATIVA
        }

        // ==========================================
        // TEMPLO DEL TIEMPO
        // ==========================================
        private void CrearTemploDelTiempo()
        {
            // ESTRUCTURA DEL TEMPLO
            for (int i = 15; i <= 30; i++)
            {
                for (int j = 45; j <= 60; j++)
                {
                    if (i == 15 || i == 30 || j == 45 || j == 60)
                    {
                        mapaBase[i, j] = 1;
                        mapaObjetos[i, j] = 1; // MUROS DE PIEDRA
                    }
                    else
                    {
                        mapaBase[i, j] = 4; // PISO DEL TEMPLO
                    }
                }
            }

            // ENTRADA PRINCIPAL
            mapaObjetos[30, 52] = -1;
            mapaObjetos[30, 53] = -1;

            // COLUMNAS INTERNAS
            mapaObjetos[18, 48] = 1;
            mapaObjetos[18, 57] = 1;
            mapaObjetos[27, 48] = 1;
            mapaObjetos[27, 57] = 1;

            // ALTAR CENTRAL
            mapaObjetos[22, 52] = 9; // PUERTA DEL TIEMPO
            mapaObjetos[23, 51] = 6; // COFRE
            mapaObjetos[23, 53] = 6; // COFRE
            mapaObjetos[21, 52] = 7; // SHEIK/ZELDA

            // DECORACIONES
            mapaObjetos[20, 50] = 25; // FAROLA
            mapaObjetos[20, 54] = 25;
            mapaObjetos[25, 50] = 25;
            mapaObjetos[25, 54] = 25;
        }

        // ==========================================
        // CASTILLO DE HYRULE
        // ==========================================
        private void CrearCastilloHyrule()
        {
            // ESTRUCTURA PRINCIPAL
            for (int i = 62; i <= 85; i++)
            {
                for (int j = 60; j <= 83; j++)
                {
                    if (i == 62 || i == 85 || j == 60 || j == 83)
                    {
                        mapaBase[i, j] = 1;
                        mapaObjetos[i, j] = 1; // MUROS EXTERIORES
                    }
                    else
                    {
                        mapaBase[i, j] = 0; // JARDINES
                    }
                }
            }

            // CAMINOS DEL CASTILLO
            for (int i = 63; i <= 84; i++)
            {
                mapaBase[i, 71] = 4;
                mapaBase[i, 72] = 4;
            }
            for (int j = 61; j <= 82; j++)
            {
                mapaBase[73, j] = 4;
                mapaBase[74, j] = 4;
            }

            // TORRE CENTRAL
            for (int i = 68; i <= 78; i++)
            {
                for (int j = 66; j <= 76; j++)
                {
                    if (i == 68 || i == 78 || j == 66 || j == 76)
                    {
                        mapaBase[i, j] = 1;
                        mapaObjetos[i, j] = 1;
                    }
                    else
                    {
                        mapaBase[i, j] = 4;
                    }
                }
            }

            // ENTRADA AL CASTILLO
            mapaObjetos[85, 71] = -1;
            mapaObjetos[85, 72] = -1;

            // TRONO/GANON
            mapaObjetos[73, 71] = 9; // PUERTA DEL TRONO
            mapaObjetos[70, 71] = 7; // GUARDIA
            mapaObjetos[70, 72] = 7; // GUARDIA

            // JARDINES Y DECORACIONES
            mapaObjetos[65, 65] = 5; // ÁRBOLES
            mapaObjetos[65, 78] = 5;
            mapaObjetos[82, 65] = 5;
            mapaObjetos[82, 78] = 5;

            // COFRES EN LOS JARDINES
            mapaObjetos[67, 63] = 6;
            mapaObjetos[67, 80] = 6;
            mapaObjetos[80, 63] = 6;
            mapaObjetos[80, 80] = 6;

            // FUENTES
            mapaObjetos[73, 65] = 3;
            mapaObjetos[73, 78] = 3;
        }

        // ==========================================
        // DESIERTO GERUDO
        // ==========================================
        private void CrearDesierto()
        {
            // ARENA INFINITA
            for (int i = 60; i <= 95; i++)
            {
                for (int j = 85; j <= 99; j++)
                {
                    mapaBase[i, j] = 2; // ARENA
                }
            }

            // OASIS
            for (int i = 70; i <= 76; i++)
            {
                for (int j = 90; j <= 96; j++)
                {
                    double distX = j - 93;
                    double distY = i - 73;
                    double dist = Math.Sqrt(distX * distX + distY * distY);

                    if (dist < 3)
                        mapaBase[i, j] = 3; // AGUA
                }
            }

            // PALMERAS ALREDEDOR DEL OASIS
            mapaObjetos[68, 91] = 5;
            mapaObjetos[68, 95] = 5;
            mapaObjetos[78, 91] = 5;
            mapaObjetos[78, 95] = 5;

            // FORTALEZA GERUDO
            for (int i = 82; i <= 92; i++)
            {
                for (int j = 88; j <= 96; j++)
                {
                    if (i == 82 || i == 92 || j == 88 || j == 96)
                    {
                        mapaBase[i, j] = 1;
                        mapaObjetos[i, j] = 1;
                    }
                }
            }

            mapaObjetos[92, 92] = 9; // ENTRADA
            mapaObjetos[87, 92] = 7; // GUARDIA GERUDO
            mapaObjetos[85, 90] = 6; // COFRE
            mapaObjetos[85, 94] = 6; // COFRE
        }

        // ==========================================
        // CEMENTERIO
        // ==========================================
        private void CrearCementerio()
        {
            // ZONA DEL CEMENTERIO
            for (int i = 35; i <= 55; i++)
            {
                for (int j = 8; j <= 25; j++)
                {
                    mapaBase[i, j] = 0;
                }
            }

            // TUMBAS (PIEDRAS PEQUEÑAS)
            for (int i = 37; i <= 53; i += 4)
            {
                for (int j = 10; j <= 23; j += 3)
                {
                    mapaObjetos[i, j] = 23;
                }
            }

            // CRIPTA PRINCIPAL
            for (int i = 43; i <= 48; i++)
            {
                for (int j = 14; j <= 19; j++)
                {
                    if (i == 43 || i == 48 || j == 14 || j == 19)
                    {
                        mapaBase[i, j] = 1;
                        mapaObjetos[i, j] = 1;
                    }
                    else
                    {
                        mapaBase[i, j] = 4;
                    }
                }
            }

            mapaObjetos[48, 16] = 9; // ENTRADA
            mapaObjetos[45, 16] = 6; // COFRE ESPECIAL
            mapaObjetos[46, 17] = 7; // FANTASMA

            // ÁRBOLES MUERTOS
            mapaObjetos[36, 10] = 5;
            mapaObjetos[36, 22] = 5;
            mapaObjetos[54, 10] = 5;
            mapaObjetos[54, 22] = 5;
        }

        // ==========================================
        // CUEVAS SECRETAS
        // ==========================================
        private void CrearCuevasSecretas()
        {
            // CUEVA 1 (NORESTE)
            for (int i = 5; i <= 10; i++)
            {
                for (int j = 5; j <= 10; j++)
                {
                    if (i == 5 || i == 10 || j == 5 || j == 10)
                    {
                        mapaBase[i, j] = 1;
                        mapaObjetos[i, j] = 1;
                    }
                    else
                    {
                        mapaBase[i, j] = 4;
                    }
                }
            }
            mapaObjetos[10, 7] = 9; // ENTRADA
            mapaObjetos[7, 7] = 6; // COFRE LEGENDARIO
            mapaObjetos[8, 8] = 7; // ANCIANO

            // CUEVA 2 (SURESTE)
            for (int i = 90; i <= 95; i++)
            {
                for (int j = 5; j <= 10; j++)
                {
                    if (i == 90 || i == 95 || j == 5 || j == 10)
                    {
                        mapaBase[i, j] = 1;
                        mapaObjetos[i, j] = 1;
                    }
                    else
                    {
                        mapaBase[i, j] = 4;
                    }
                }
            }
            mapaObjetos[90, 7] = 9;
            mapaObjetos[93, 7] = 6;
        }

        // ==========================================
        // RÍO Y PUENTES
        // ==========================================
        private void CrearRioYPuentes()
        {
            // RÍO VERTICAL (DIVIDE EL MAPA)
            for (int i = 0; i <= 99; i++)
            {
                if (i < 42 || i > 54) // NO PASAR POR EL PUEBLO
                {
                    mapaBase[i, 38] = 3; // AGUA
                    mapaBase[i, 39] = 3;
                    mapaBase[i, 40] = 3;
                }
            }

            // PUENTES
            int[] puentesY = { 12, 35, 58, 78, 95 };
            foreach (int y in puentesY)
            {
                mapaBase[y, 38] = 4;
                mapaBase[y, 39] = 4;
                mapaBase[y, 40] = 4;
                mapaObjetos[y, 37] = 21; // VALLA
                mapaObjetos[y, 41] = 21; // VALLA
            }
        }

        // ==========================================
        // CAMINOS CONECTORES
        // ==========================================
        private void CrearCaminosConectores()
        {
            // PUEBLO -> TEMPLO DEL TIEMPO
            for (int j = 41; j >= 32; j--)
            {
                mapaBase[42, j] = 4;
            }
            for (int i = 30; i <= 42; i++)
            {
                mapaBase[i, 32] = 4;
            }

            // PUEBLO -> LAGO
            for (int i = 54; i <= 62; i++)
            {
                mapaBase[i, 48] = 4;
            }
            for (int j = 40; j >= 28; j--)
            {
                mapaBase[62, j] = 4;
            }

            // PUEBLO -> CASTILLO
            for (int i = 54; i <= 62; i++)
            {
                mapaBase[i, 48] = 4;
            }
            for (int j = 48; j <= 60; j++)
            {
                mapaBase[62, j] = 4;
            }

            // BOSQUE -> CEMENTERIO
            for (int i = 35; i <= 42; i++)
            {
                mapaBase[i, 25] = 4;
            }
        }

        // ==========================================
        // ZONA SECRETA DEL NORTE
        // ==========================================
        private void CrearZonaSecretaNorte()
        {
            // SANTUARIO OCULTO
            for (int i = 2; i <= 8; i++)
            {
                for (int j = 45; j <= 55; j++)
                {
                    if (i == 2 || i == 8 || j == 45 || j == 55)
                    {
                        mapaBase[i, j] = 1;
                        mapaObjetos[i, j] = 1;
                    }
                    else
                    {
                        mapaBase[i, j] = 4;
                    }
                }
            }

            mapaObjetos[8, 50] = 9; // ENTRADA SECRETA
            mapaObjetos[5, 50] = 9; // TELETRANSPORTE
            mapaObjetos[4, 48] = 6; // COFRE MÁSTER
            mapaObjetos[4, 52] = 6; // COFRE MÁSTER
            mapaObjetos[5, 47] = 7; // GRAN HADA
        }

        // ==========================================
        // ISLA FLOTANTE
        // ==========================================
        private void CrearIslaFlotante()
        {
            // ISLA EN EL CIELO (ESQUINA SUROESTE)
            for (int i = 88; i <= 96; i++)
            {
                for (int j = 45; j <= 53; j++)
                {
                    double distX = j - 49;
                    double distY = i - 92;
                    double dist = Math.Sqrt(distX * distX + distY * distY);

                    if (dist < 5)
                    {
                        mapaBase[i, j] = 0;
                        if (dist < 2)
                            mapaObjetos[i, j] = 9; // PORTAL
                    }
                }
            }

            mapaObjetos[92, 49] = 6; // COFRE CELESTIAL
            mapaObjetos[91, 48] = 7; // ÁNGEL
        }

        // ==========================================
        // ELEMENTOS DECORATIVOS
        // ==========================================
        private void AgregarElementosDecorativos()
        {
            Random rnd = new Random(42); // SEED FIJA PARA CONSISTENCIA

            // FLORES ALEATORIAS EN ZONAS DE PASTO
            for (int intentos = 0; intentos < 200; intentos++)
            {
                int i = rnd.Next(0, 100);
                int j = rnd.Next(0, 100);

                if (mapaBase[i, j] == 0 && mapaObjetos[i, j] == -1)
                {
                    mapaObjetos[i, j] = 22; // FLORES
                }
            }

            // ROCAS DECORATIVAS
            for (int intentos = 0; intentos < 100; intentos++)
            {
                int i = rnd.Next(0, 100);
                int j = rnd.Next(0, 100);

                if (mapaBase[i, j] == 0 && mapaObjetos[i, j] == -1)
                {
                    mapaObjetos[i, j] = 23; // ROCAS
                }
            }

            // SEÑALES EN CRUCES DE CAMINOS
            mapaObjetos[42, 32] = 24;
            mapaObjetos[62, 48] = 24;
            mapaObjetos[35, 25] = 24;

            // FAROLAS EN CAMINOS PRINCIPALES
            for (int i = 43; i <= 53; i += 5)
            {
                mapaObjetos[i, 38] = 25;
                mapaObjetos[i, 40] = 25;
            }
        }


        private void AgregarObjetosDecorativos()
        {

            for (int j = 10; j <= 25; j++)
            {
                if (mapaBase[26, j] == 0)
                {
                    mapaObjetos[26, j] = 20;
                }
            }

            for (int i = 10; i <= 25; i++)
            {
                if (mapaBase[i, 9] == 0)
                {
                    mapaObjetos[i, 9] = 21; 
                }
            }

            for (int i = 10; i <= 25; i++)
            {
                if (mapaBase[i, 26] == 0)
                {
                    mapaObjetos[i, 26] = 21;
                }
            }

            // FLORES Y DECORACIONES EN EL PASTO
            mapaObjetos[12, 12] = 22; 
            mapaObjetos[14, 14] = 22;
            mapaObjetos[20, 20] = 22;

            // ROCAS DECORATIVAS
            mapaObjetos[18, 12] = 23; 
            mapaObjetos[22, 18] = 23;

            // SEÑALES DE CAMINO
            mapaObjetos[17, 10] = 24; 

            // FAROLAS
            mapaObjetos[13, 17] = 25; 
            mapaObjetos[21, 17] = 25;
        }



        private void CrearPuebloInicial()
        {
            // PERÍMETRO DEL PUEBLO
            for (int i = 10; i <= 25; i++)
            {
                mapaBase[10, i] = 4;
                mapaBase[25, i] = 4;
                mapaBase[i, 10] = 4;
                mapaBase[i, 25] = 4;

                // MUROS SOBRE EL CAMINO
                mapaObjetos[10, i] = 1;
                mapaObjetos[25, i] = 1;
                mapaObjetos[i, 10] = 1;
                mapaObjetos[i, 25] = 1;
            }

            // ENTRADA DEL PUEBLO
            mapaObjetos[10, 17] = -1;
            mapaObjetos[10, 18] = -1;
            mapaObjetos[25, 17] = -1;
            mapaObjetos[25, 18] = -1;

            // CAMINOS INTERNOS (solo terreno)
            for (int j = 11; j <= 24; j++)
            {
                mapaBase[17, j] = 4; 
            }
            for (int i = 11; i <= 24; i++)
            {
                mapaBase[i, 17] = 4; 
            }

            // EDIFICIOS Y TIENDAS
            mapaObjetos[13, 13] = 8; 
            mapaObjetos[13, 21] = 8;
            mapaObjetos[21, 13] = 8;
            mapaObjetos[21, 21] = 7;

            // ÁRBOLES DECORATIVOS
            mapaObjetos[12, 12] = 5;
            mapaObjetos[12, 22] = 5;
            mapaObjetos[22, 12] = 5;
            mapaObjetos[22, 22] = 5;

            // COFRES
            mapaObjetos[15, 15] = 6;
        }


        private void AgregarPuntosDeInteres()
        {
            for (int i = 5; i <= 15; i++)
            {
                for (int j = 85; j <= 95; j++)
                {
                    if (i == 5 || i == 15 || j == 85 || j == 95)
                        mapa[i, j] = 1; 
                }
            }
            mapa[10, 90] = 9; 
            mapa[8, 92] = 7; 
            mapa[12, 92] = 6;

            for (int i = 80; i < 90; i++)
            {
                for (int j = 10; j < 25; j++)
                {
                    if ((i + j) % 5 == 0)
                        mapa[i, j] = 1; 
                }
            }
            mapa[85, 17] = 7; 
            mapa[82, 15] = 6; 

            for (int i = 28; i <= 32; i++)
            {
                mapa[i, 50] = 3; 
                mapa[i, 51] = 3;
            }
            for (int i = 28; i <= 32; i++)
                mapa[i, 52] = 4; 

            for (int i = 45; i <= 55; i++)
            {
                for (int j = 70; j <= 80; j++)
                {
                    if (i == 45 || i == 55 || j == 70 || j == 80)
                        mapa[i, j] = 1;
                }
            }
            mapa[50, 70] = 9; 
            mapa[50, 75] = 6; 
            mapa[48, 75] = 7; 
        }

        private void ManejareInteraccion(Point pos, int tipo)
        {
            string zona = ObtenerNombreZona(pos);

            switch (tipo)
            {
                case 3: // AGUA
                    MessageBox.Show($"TE VAS A AHOGAR CARNALITO ANDAS EN {zona}.", ""
                                , MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case 6: // COFRE
                    int dineroGanado = new Random().Next(50, 200);
                    pj.DIN += dineroGanado;
                    mapa[pos.Y, pos.X] = 0;
                    GenerarMapaBuffer();
                    ActualizarDineroLabel();
                    BASE_DE_DATOS.ActualizarDinero(pj.ID, pj.DIN);

                    // GENERAR ITEMS ALEATORIOS
                    List<string> itemsObtenidos = new List<string>();
                    Random rnd = new Random();

                    // 50% de probabilidad de obtener un item
                    if (rnd.Next(0, 100) < 50)
                    {
                        string[] posiblesItems = {
                            "Poción de Curación",
                            "Poción de Maná",
                            "Antídoto",
                            "Pergamino Mágico",
                            "Gema Rara"
                        };
                        itemsObtenidos.Add(posiblesItems[rnd.Next(posiblesItems.Length)]);
                    }

                    // MOSTRAR PANEL DE RECOMPENSA
                    MostrarRecompensaCofre(dineroGanado, itemsObtenidos);
                    break;

                case 7: // NPC
                    MostrarDialogoSegunZona(pos);
                    break;


                case 8: // TIENDA
                    AbrirTienda();
                    break;

                case 9: // PUERTA
                    var resultado = MessageBox.Show(
                        $"ESTO VA A SER UN DESAFIO PERO TODAVIA NO EXISTE {zona}.\n¿QUIERES ENTRAR A LA NADA?",
                        "OK",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                    {
                        MessageBox.Show("NO EXISTE PDJO",
                                      "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
            }
        }



        private string ObtenerNombreZona(Point pos)
        {
            if (pos.X >= 10 && pos.X <= 25 && pos.Y >= 10 && pos.Y <= 25)
                return "el Pueblo Inicial";

            if (pos.X >= 5 && pos.X <= 30 && pos.Y >= 30 && pos.Y <= 50)
                return "el Bosque Oscuro";

            if (pos.X >= 40 && pos.X <= 60 && pos.Y >= 40 && pos.Y <= 55)
                return "el Lago Cristalino";

            if (pos.X >= 70 && pos.X <= 85 && pos.Y >= 10 && pos.Y <= 60)
                return "las Montañas del Norte";

            if (pos.X >= 65 && pos.X <= 95 && pos.Y >= 65 && pos.Y <= 90)
                return "el Desierto Ardiente";

            if (pos.X >= 85 && pos.X <= 95 && pos.Y >= 5 && pos.Y <= 15)
                return "la Torre Misteriosa";

            if (pos.X >= 10 && pos.X <= 25 && pos.Y >= 80 && pos.Y <= 90)
                return "el Cementerio Embrujado";

            if (pos.X >= 70 && pos.X <= 80 && pos.Y >= 45 && pos.Y <= 55)
                return "la Fortaleza Abandonada";

            return "tierras desconocidas";
        }

        private void GenerarMapaBuffer()
        {
            int filas = mapaBase.GetLength(0);
            int columnas = mapaBase.GetLength(1);
            mapaBuffer = new Bitmap(columnas * tamañoCelda, filas * tamañoCelda);

            using (Graphics g = Graphics.FromImage(mapaBuffer))
            {
                for (int i = 0; i < filas; i++)
                {
                    for (int j = 0; j < columnas; j++)
                    {
                        int tipoTerreno = mapaBase[i, j];
                        if (!spritesTerreno.ContainsKey(tipoTerreno))
                            tipoTerreno = 0;

                        g.DrawImage(spritesTerreno[tipoTerreno],
                                   j * tamañoCelda,
                                   i * tamañoCelda,
                                   tamañoCelda,
                                   tamañoCelda);
                    }
                }

                for (int i = 0; i < filas; i++)
                {
                    for (int j = 0; j < columnas; j++)
                    {
                        int tipoObjeto = mapaObjetos[i, j];

                        if (tipoObjeto != -1 && spritesObjetos.ContainsKey(tipoObjeto))
                        {
                            g.DrawImage(spritesObjetos[tipoObjeto],
                                       j * tamañoCelda,
                                       i * tamañoCelda,
                                       tamañoCelda,
                                       tamañoCelda);
                        }
                    }
                }
            }
        }

        private void DibujarMapa(object sender, PaintEventArgs e)
        {
            if (mapaBuffer == null) return;

            int anchoVista = panelMapa.Width;
            int altoVista = panelMapa.Height;

            Rectangle origen = new Rectangle(
                camaraPos.X * tamañoCelda,
                camaraPos.Y * tamañoCelda,
                anchoVista,
                altoVista
            );

            e.Graphics.DrawImage(mapaBuffer, new Rectangle(0, 0, anchoVista, altoVista), origen, GraphicsUnit.Pixel);

            // --------------------------------------------------------------------------------------------------------------- DIBUJA AL JUGADOR POR ENCIMA
            int jugadorX = (posicionJugador.X - camaraPos.X) * tamañoCelda;
            int jugadorY = (posicionJugador.Y - camaraPos.Y) * tamañoCelda;
            e.Graphics.FillEllipse(Brushes.Blue, jugadorX, jugadorY, tamañoCelda, tamañoCelda);

            // --------------------------------------------------------------------------------------------------------------- DIBUJA ENEMIGOS POR ENCIMA
            foreach (var enemigo in enemigos)
                enemigo.Dibujar(e.Graphics, camaraPos, tamañoCelda);
        }

        private string ObtenerDialogoNPC(string zona)
        {
            if (zona.Contains("Pueblo"))
                return "¡ME LA PELAS.";

            if (zona.Contains("Bosque"))
                return "PUTO.";

            if (zona.Contains("Lago"))
                return "UN PDJO?";

            if (zona.Contains("Montañas"))
                return "PITO.";

            if (zona.Contains("Desierto"))
                return "VERGA.";

            if (zona.Contains("Torre"))
                return "PENE.";

            if (zona.Contains("Cementerio"))
                return "MIEMBRO.";

            if (zona.Contains("Fortaleza"))
                return "APARATO REPRODUCTOR?";

            return "SEXO TILIN";
        }


        private void MostrarDialogoNPCSimple()
        {
            DialogoNPC dialogo = new DialogoNPC
            {
                NombreNPC = "Aldeano Juan",
                ImagenNPC = "NPC_ALDEANO.png",
                TipoNPC = "info",
                Mensajes = new List<string>
        {
            "¡Hola aventurero! Bienvenido a nuestro humilde pueblo.",
            "Si buscas provisiones, el mercader está justo al norte de aquí.",
            "Ten cuidado con los lobos que rondan por el bosque al este."
        }
            };

            using (DIALOGO_NPC dialogoForm = new DIALOGO_NPC(dialogo))
            {
                dialogoForm.ShowDialog(this);
            }
        }

        private void MostrarDialogoNPCMision()
        {
            DialogoNPC dialogo = new DialogoNPC
            {
                NombreNPC = "Capitán Marcus",
                ImagenNPC = "NPC_CAPITAN.png",
                TipoNPC = "quest",
                Mensajes = new List<string>
        {
            "¡Aventurero! Necesito tu ayuda urgentemente.",
            "Un dragón ha estado aterrorizando las aldeas cercanas.",
            "Si logras derrotarlo, te recompensaré generosamente."
        },
                Opciones = new List<OpcionDialogo>
        {
            new OpcionDialogo("Acepto la misión", () => {
                MessageBox.Show("¡Misión aceptada! Ve al norte para encontrar al dragón.");
            }, Color.FromArgb(50, 180, 80)),

            new OpcionDialogo("Necesito más tiempo", () => {
                MessageBox.Show("Entiendo. Vuelve cuando estés listo.");
            }, Color.FromArgb(200, 140, 50)),

            new OpcionDialogo("No estoy interesado", () => {
                MessageBox.Show("Qué lástima... esperaba más de ti.");
            }, Color.FromArgb(180, 60, 60))
        }
            };

            using (DIALOGO_NPC dialogoForm = new DIALOGO_NPC(dialogo))
            {
                dialogoForm.ShowDialog(this);
            }
        }

        private void MostrarDialogoComerciant()
        {
            DialogoNPC dialogo = new DialogoNPC
            {
                NombreNPC = "Comerciante Roderick",
                ImagenNPC = "NPC_COMERCIANTE.png",
                TipoNPC = "tienda",
                Mensajes = new List<string>
                {
                "¡Bienvenido a mi tienda, viajero!",
                "Tengo las mejores armas y pociones de toda la región."
                },
                Opciones = new List<OpcionDialogo>
                {
                new OpcionDialogo("Ver tienda", () => {
                using (TIENDA tienda = new TIENDA(pj))
                {
                    tienda.ShowDialog();
                }
                }, Color.FromArgb(100, 220, 100)),

                new OpcionDialogo("Solo estoy mirando", () => {
                MessageBox.Show("No hay problema, vuelve cuando quieras comprar algo.");
                }, Color.FromArgb(100, 150, 220))
                }
            };

            using (DIALOGO_NPC dialogoForm = new DIALOGO_NPC(dialogo))
            {
                dialogoForm.ShowDialog(this);
            }
        }

        private void MostrarDialogoSegunZona(Point pos)
        {
            string zona = ObtenerNombreZona(pos);

            DialogoNPC dialogo = new DialogoNPC
            {
                NombreNPC = "Habitante Local",
                ImagenNPC = "NPC_ALDEANO.png",
                TipoNPC = "info",
                Mensajes = new List<string>
                {
                    $"Hola viajero, estás en {zona}.",
                    "¿Puedo ayudarte en algo?"
                },
                Opciones = new List<OpcionDialogo>
                {
                new OpcionDialogo("Cuéntame sobre este lugar", () => {
                    MessageBox.Show($"Información sobre {zona}");
                }, Color.FromArgb(100, 150, 220)),

                    new OpcionDialogo("Adiós", () => { }, Color.FromArgb(80, 80, 100))
                }
            };

            using (DIALOGO_NPC dialogoForm = new DIALOGO_NPC(dialogo))
            {
                dialogoForm.ShowDialog(this);
            }
        }

        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------- MENU DEL MAPA
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------


        private Panel panelMenuMapa;
        private Panel picContainerMenu;
        private PictureBox picMapaMenu;
        private List<Point> marcasMapa = new List<Point>();
        private Bitmap mapaBaseBitmapMenu;
        private float mapaZoom = 1.0f;
        private float mapaFitZoom = 1.0f;
        private float mapaZoomMin = 0.2f;
        private float mapaZoomMax = 5.0f;
        private bool mapaDragging = false;
        private Point mapaDragStartPoint;
        private Point mapaDragStartScroll;
        private int mapaCellBaseSize = 20;

        private void MostrarMenuMapa()
        {
            indiceMenuActual = 2;
            if (panelMenuMapa != null && panelMenuMapa.Visible) return;
            juegoPausado = true;

            panelMenuMapa = new Panel
            {
                Dock = DockStyle.Fill,
                BackgroundImage = CapturarMapa(),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            this.Controls.Add(panelMenuMapa);
            panelMenuMapa.BringToFront();

            Panel overlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(160, 0, 0, 0)
            };
            panelMenuMapa.Controls.Add(overlay);

            // ------------------------------------------------------------------------------------------ TAMAÑO DE LA VENTANA
            int tamañoVentana = Math.Min(this.ClientSize.Width - 20, this.ClientSize.Height) - 80;

            Panel ventanaMapa = new Panel
            {
                Size = new Size(tamañoVentana, tamañoVentana),
                BackColor = Color.FromArgb(230, 20, 20, 20),
                Location = new Point((overlay.Width - tamañoVentana) / 2, (overlay.Height - tamañoVentana) / 2)
            };
            overlay.Controls.Add(ventanaMapa);
            RedondearMenu(ventanaMapa, 26);

            // ------------------------------------------------------------------------------------------ BOTON CERRAR
            Button btnCerrar = new Button
            {
                Text = "X",
                Font = FUENTE.ObtenerFont(18),
                Size = new Size(30, 30),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Location = new Point(5, 5)
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnCerrar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnCerrar.MouseEnter += (s, e) => btnCerrar.ForeColor = Color.Red;
            btnCerrar.MouseLeave += (s, e) => btnCerrar.ForeColor = Color.White;
            RedondearBoton(btnCerrar, 26);
            btnCerrar.Click += (s, e) => CerrarMenuMapa();
            ventanaMapa.Controls.Add(btnCerrar);

            // ------------------------------------------------------------------------------------------ FLECHAS DEL CARRUSEL
            Button flechaIzq = new Button
            {
                Text = "<",
                Width = 50,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = FUENTE.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Location = new Point(10, (this.ClientSize.Height - 50) / 2)
            };
            flechaIzq.FlatAppearance.BorderSize = 0;
            flechaIzq.FlatAppearance.MouseOverBackColor = Color.Transparent;
            flechaIzq.FlatAppearance.MouseDownBackColor = Color.Transparent;
            flechaIzq.MouseEnter += (s, e) => flechaIzq.ForeColor = Color.DarkGray;
            flechaIzq.MouseLeave += (s, e) => flechaIzq.ForeColor = Color.White;
            flechaIzq.Click += (s, e) => Navegar_FlechaIzquierda();
            overlay.Controls.Add(flechaIzq);

            Button flechaDer = new Button
            {
                Text = ">",
                Width = 50,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = FUENTE.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Location = new Point(this.ClientSize.Width - 50 - 10, (this.ClientSize.Height - 50) / 2)
            };
            flechaDer.FlatAppearance.BorderSize = 0;
            flechaDer.FlatAppearance.MouseOverBackColor = Color.Transparent;
            flechaDer.FlatAppearance.MouseDownBackColor = Color.Transparent;
            flechaDer.MouseEnter += (s, e) => flechaDer.ForeColor = Color.DarkGray;
            flechaDer.MouseLeave += (s, e) => flechaDer.ForeColor = Color.White;

            flechaDer.Click += (s, e) => Navegar_FlechaDerecha();
            overlay.Controls.Add(flechaDer);

            // ------------------------------------------------------------------------------------------ PANEL CONTENEDOR DEL MAPA
            picContainerMenu = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(ventanaMapa.Width - 20, ventanaMapa.Height - 20),
                AutoScroll = false,
                BackColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };
            ventanaMapa.Controls.Add(picContainerMenu);

            // ------------------------------------------------------------------------------------------ PICTUREBOX DEL MAPA
            picMapaMenu = new PictureBox
            {
                Location = new Point(0, 0),
                BackColor = Color.Black
            };
            picContainerMenu.Controls.Add(picMapaMenu);
            btnCerrar.BringToFront();

            GenerarBaseBitmap();

            float ratioX = (float)picContainerMenu.ClientSize.Width / mapaBaseBitmapMenu.Width;
            float ratioY = (float)picContainerMenu.ClientSize.Height / mapaBaseBitmapMenu.Height;
            mapaZoom = mapaFitZoom = Math.Min(ratioX, ratioY);
            mapaZoomMin = mapaFitZoom;
            mapaZoomMax = mapaFitZoom * 20f;

            UpdateScaledImage(null);

            picMapaMenu.MouseDown += PicMapaMenu_MouseDown;
            picMapaMenu.MouseMove += PicMapaMenu_MouseMove;
            picMapaMenu.MouseUp += PicMapaMenu_MouseUp;
            picMapaMenu.MouseClick += PicMapaMenu_MouseClick;
        }


        // --------------------------------------------------------------------------------------------- GENERAR TERRENO MARCAS JUGADOR
        private void GenerarBaseBitmap()
        {
            try { mapaBaseBitmapMenu?.Dispose(); } catch { }

            int filas = (mapaBase != null) ? mapaBase.GetLength(0) : 100;
            int columnas = (mapaBase != null) ? mapaBase.GetLength(1) : 100;

            int ancho = columnas * mapaCellBaseSize;
            int alto = filas * mapaCellBaseSize;
            if (ancho <= 0) ancho = 1;
            if (alto <= 0) alto = 1;

            mapaBaseBitmapMenu = new Bitmap(ancho, alto);
            using (Graphics g = Graphics.FromImage(mapaBaseBitmapMenu))
            {
                g.Clear(Color.Black);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // ====================================================================
                // DIBUJAR TERRENO (mapaBase) CON COLORES MEJORADOS
                // ====================================================================
                for (int y = 0; y < filas; y++)
                {
                    for (int x = 0; x < columnas; x++)
                    {
                        int tipoTerreno = mapaBase[y, x];
                        Color c = tipoTerreno switch
                        {
                            0 => Color.FromArgb(34, 160, 34),   // PASTO (MÁS BRILLANTE)
                            1 => Color.FromArgb(100, 100, 110), // PIEDRA
                            2 => Color.FromArgb(230, 210, 160), // ARENA
                            3 => Color.FromArgb(46, 150, 220),  // AGUA (MÁS AZUL)
                            4 => Color.FromArgb(150, 120, 70),  // CAMINO
                            _ => Color.DarkGreen
                        };

                        using (Brush b = new SolidBrush(c))
                        {
                            g.FillRectangle(b, x * mapaCellBaseSize, y * mapaCellBaseSize,
                                           mapaCellBaseSize, mapaCellBaseSize);
                        }

                        // BORDE SUTIL PARA MEJOR DEFINICIÓN (solo si el tamaño es suficiente)
                        if (mapaCellBaseSize >= 8)
                        {
                            using (Pen borderPen = new Pen(Color.FromArgb(30, 0, 0, 0), 1))
                            {
                                g.DrawRectangle(borderPen, x * mapaCellBaseSize, y * mapaCellBaseSize,
                                              mapaCellBaseSize - 1, mapaCellBaseSize - 1);
                            }
                        }
                    }
                }

                // ====================================================================
                // DIBUJAR OBJETOS (mapaObjetos) CON ICONOS MEJORADOS
                // ====================================================================
                for (int y = 0; y < filas; y++)
                {
                    for (int x = 0; x < columnas; x++)
                    {
                        int tipoObjeto = mapaObjetos[y, x];
                        if (tipoObjeto == -1) continue;

                        int cx = x * mapaCellBaseSize;
                        int cy = y * mapaCellBaseSize;
                        int size = Math.Max(4, mapaCellBaseSize - 2);

                        Color objetoColor = tipoObjeto switch
                        {
                            1 => Color.FromArgb(80, 80, 80),    // MURO - GRIS OSCURO
                            3 => Color.FromArgb(46, 150, 220),  // FUENTE - AGUA
                            5 => Color.FromArgb(20, 100, 20),   // ÁRBOL - VERDE OSCURO
                            6 => Color.Gold,                     // COFRE - DORADO
                            7 => Color.Orange,                   // NPC - NARANJA
                            8 => Color.Violet,                   // TIENDA - VIOLETA
                            9 => Color.SaddleBrown,             // PUERTA - CAFÉ
                            20 => Color.FromArgb(139, 90, 43),  // VALLA H - CAFÉ
                            21 => Color.FromArgb(139, 90, 43),  // VALLA V - CAFÉ
                            22 => Color.HotPink,                 // FLORES - ROSA
                            23 => Color.Gray,                    // ROCA - GRIS
                            24 => Color.FromArgb(160, 120, 60), // SEÑAL - MADERA
                            25 => Color.Yellow,                  // FAROLA - AMARILLO
                            _ => Color.White
                        };

                        using (Brush b = new SolidBrush(objetoColor))
                        {
                            // DIBUJAR SEGÚN EL TIPO DE OBJETO
                            if (tipoObjeto == 1) // MURO - CUADRADO SÓLIDO
                            {
                                g.FillRectangle(b, cx, cy, mapaCellBaseSize, mapaCellBaseSize);
                                using (Pen borderPen = new Pen(Color.FromArgb(60, 60, 60), 1))
                                {
                                    g.DrawRectangle(borderPen, cx, cy, mapaCellBaseSize - 1, mapaCellBaseSize - 1);
                                }
                            }
                            else if (tipoObjeto == 6) // COFRE - CUADRADO CON BORDE
                            {
                                g.FillRectangle(b, cx + 1, cy + 1, size, size);
                                g.DrawRectangle(Pens.DarkGoldenrod, cx + 1, cy + 1, size, size);

                                // AGREGAR BRILLO
                                using (Brush highlight = new SolidBrush(Color.FromArgb(150, 255, 255, 200)))
                                {
                                    g.FillRectangle(highlight, cx + 2, cy + 2, size / 3, size / 3);
                                }
                            }
                            else if (tipoObjeto == 9) // PUERTA - RECTÁNGULO VERTICAL
                            {
                                int doorWidth = Math.Max(2, size / 2);
                                int doorHeight = Math.Max(4, size);
                                g.FillRectangle(b, cx + (mapaCellBaseSize - doorWidth) / 2, cy, doorWidth, doorHeight);
                                g.DrawRectangle(Pens.Black, cx + (mapaCellBaseSize - doorWidth) / 2, cy, doorWidth, doorHeight);
                            }
                            else if (tipoObjeto == 5) // ÁRBOL - CÍRCULO VERDE
                            {
                                g.FillEllipse(b, cx + 1, cy + 1, size, size);
                                using (Pen treeBorder = new Pen(Color.FromArgb(15, 70, 15), 1))
                                {
                                    g.DrawEllipse(treeBorder, cx + 1, cy + 1, size, size);
                                }
                            }
                            else if (tipoObjeto == 25) // FAROLA - CRUZ BRILLANTE
                            {
                                int crossSize = Math.Max(2, size);
                                // VERTICAL
                                g.FillRectangle(b, cx + mapaCellBaseSize / 2 - 1, cy, 2, crossSize);
                                // HORIZONTAL
                                g.FillRectangle(b, cx, cy + mapaCellBaseSize / 2 - 1, crossSize, 2);

                                // BRILLO
                                using (Brush glow = new SolidBrush(Color.FromArgb(100, 255, 255, 150)))
                                {
                                    g.FillEllipse(glow, cx, cy, crossSize, crossSize);
                                }
                            }
                            else if (tipoObjeto == 8) // TIENDA - CUADRADO CON TECHO
                            {
                                g.FillRectangle(b, cx + 1, cy + size / 3, size - 1, size * 2 / 3);
                                g.DrawRectangle(Pens.Purple, cx + 1, cy + size / 3, size - 1, size * 2 / 3);

                                // TECHO TRIANGULAR
                                Point[] roof = {
                            new Point(cx, cy + size/3),
                            new Point(cx + size/2, cy),
                            new Point(cx + size, cy + size/3)
                        };
                                g.FillPolygon(Brushes.DarkViolet, roof);
                            }
                            else if (tipoObjeto == 7) // NPC - CÍRCULO CON CARA
                            {
                                g.FillEllipse(b, cx + 2, cy + 2, size - 2, size - 2);
                                g.DrawEllipse(Pens.DarkOrange, cx + 2, cy + 2, size - 2, size - 2);

                                // OJOS (si hay espacio)
                                if (size > 6)
                                {
                                    g.FillEllipse(Brushes.Black, cx + size / 3, cy + size / 3, 2, 2);
                                    g.FillEllipse(Brushes.Black, cx + size * 2 / 3, cy + size / 3, 2, 2);
                                }
                            }
                            else if (tipoObjeto == 22) // FLORES - MÚLTIPLES CÍRCULOS
                            {
                                int flowerSize = Math.Max(2, size / 3);
                                g.FillEllipse(b, cx, cy, flowerSize, flowerSize);
                                g.FillEllipse(b, cx + size / 2, cy, flowerSize, flowerSize);
                                g.FillEllipse(b, cx, cy + size / 2, flowerSize, flowerSize);
                                g.FillEllipse(b, cx + size / 2, cy + size / 2, flowerSize, flowerSize);
                            }
                            else if (tipoObjeto == 23) // ROCA - CÍRCULO IRREGULAR
                            {
                                g.FillEllipse(b, cx + 2, cy + 2, size - 2, size - 2);
                                using (Pen rockPen = new Pen(Color.DarkGray, 1))
                                {
                                    g.DrawEllipse(rockPen, cx + 2, cy + 2, size - 2, size - 2);
                                }
                            }
                            else if (tipoObjeto == 24) // SEÑAL - POSTE CON TABLERO
                            {
                                // POSTE
                                g.FillRectangle(Brushes.SaddleBrown, cx + mapaCellBaseSize / 2 - 1, cy + size / 3, 2, size);
                                // TABLERO
                                g.FillRectangle(b, cx + 1, cy, size, size / 3);
                                g.DrawRectangle(Pens.Brown, cx + 1, cy, size, size / 3);
                            }
                            else if (tipoObjeto == 20 || tipoObjeto == 21) // VALLAS
                            {
                                if (tipoObjeto == 20) // HORIZONTAL
                                {
                                    g.FillRectangle(b, cx, cy + mapaCellBaseSize / 2 - 1, mapaCellBaseSize, 2);
                                }
                                else // VERTICAL
                                {
                                    g.FillRectangle(b, cx + mapaCellBaseSize / 2 - 1, cy, 2, mapaCellBaseSize);
                                }
                            }
                            else if (tipoObjeto == 3) // FUENTE (AGUA DECORATIVA)
                            {
                                g.FillEllipse(b, cx + 1, cy + 1, size, size);
                                using (Pen waterPen = new Pen(Color.FromArgb(100, 200, 255), 1))
                                {
                                    g.DrawEllipse(waterPen, cx + 1, cy + 1, size, size);
                                }
                            }
                            else // OTROS - CÍRCULO GENÉRICO
                            {
                                g.FillEllipse(b, cx + 2, cy + 2, size - 2, size - 2);
                            }
                        }
                    }
                }

                // ====================================================================
                // DIBUJAR MARCAS CON ETIQUETAS MEJORADAS
                // ====================================================================
                using (Brush bm = new SolidBrush(Color.Black))
                using (Pen pm = new Pen(Color.White, 2))
                using (Font f = FUENTE.ObtenerFont(8))
                {
                    int marcaIndex = 1;
                    foreach (var m in marcasMapa)
                    {
                        int cx = m.X * mapaCellBaseSize;
                        int cy = m.Y * mapaCellBaseSize;
                        int size = Math.Max(8, mapaCellBaseSize);

                        // SOMBRA
                        g.FillEllipse(Brushes.Black, cx - size / 2 + 2, cy - size / 2 + 2, size, size);

                        // MARCA
                        g.FillEllipse(bm, cx - size / 2, cy - size / 2, size, size);
                        g.DrawEllipse(pm, cx - size / 2, cy - size / 2, size, size);

                        // NÚMERO DE MARCA
                        string txt = marcaIndex.ToString();
                        SizeF ts = g.MeasureString(txt, f);
                        g.DrawString(txt, f, Brushes.White,
                                   cx - ts.Width / 2, cy - ts.Height / 2);

                        marcaIndex++;
                    }
                }

                // ====================================================================
                // DIBUJAR JUGADOR CON EFECTO BRILLANTE
                // ====================================================================
                if (posicionJugador != Point.Empty)
                {
                    int jx = posicionJugador.X * mapaCellBaseSize;
                    int jy = posicionJugador.Y * mapaCellBaseSize;
                    int psize = Math.Max(mapaCellBaseSize + 4, 12);

                    // RESPLANDOR EXTERIOR
                    using (Brush glowBrush = new SolidBrush(Color.FromArgb(100, 255, 100, 100)))
                    {
                        g.FillEllipse(glowBrush, jx - psize / 2 - 2, jy - psize / 2 - 2,
                                    psize + 4, psize + 4);
                    }

                    // JUGADOR (ROJO)
                    using (Brush bp = new SolidBrush(Color.FromArgb(255, 80, 80)))
                    {
                        g.FillEllipse(bp, jx - psize / 2, jy - psize / 2, psize, psize);
                        g.DrawEllipse(Pens.White, jx - psize / 2, jy - psize / 2, psize, psize);
                    }

                    // PUNTO CENTRAL
                    using (Brush centerBrush = new SolidBrush(Color.White))
                    {
                        g.FillEllipse(centerBrush, jx - 2, jy - 2, 4, 4);
                    }

                    // DIRECCIÓN (PEQUEÑA FLECHA)
                    if (mapaCellBaseSize >= 8)
                    {
                        Point[] arrow = {
                    new Point(jx, jy - psize/2 - 3),
                    new Point(jx - 3, jy - psize/2 + 2),
                    new Point(jx + 3, jy - psize/2 + 2)
                };
                        g.FillPolygon(Brushes.White, arrow);
                    }
                }
            }
        }

        private void UpdateScaledImage(Point? mouseInContainer)
        {
            if (mapaBaseBitmapMenu == null || picMapaMenu == null || picContainerMenu == null) return;

            int newW = (int)(mapaBaseBitmapMenu.Width * mapaZoom);
            int newH = (int)(mapaBaseBitmapMenu.Height * mapaZoom);

            Bitmap scaled = new Bitmap(newW, newH);
            using (Graphics g = Graphics.FromImage(scaled))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(mapaBaseBitmapMenu, new Rectangle(0, 0, newW, newH));
            }

            var prev = picMapaMenu.Image;
            picMapaMenu.Image = scaled;
            picMapaMenu.Size = scaled.Size;
            try { prev?.Dispose(); } catch { }

            if (Math.Abs(mapaZoom - mapaFitZoom) < 0.0001f)
            {
                picMapaMenu.Location = new Point(0, 0);
            }
            else
            {

                if (mouseInContainer.HasValue)
                {
                    Point mouse = mouseInContainer.Value;
                    float relX = (mouse.X - picMapaMenu.Left) / (float)picMapaMenu.Width;
                    float relY = (mouse.Y - picMapaMenu.Top) / (float)picMapaMenu.Height;

                    int newLeft = mouse.X - (int)(relX * newW);
                    int newTop = mouse.Y - (int)(relY * newH);

                    if (picMapaMenu.Width > picContainerMenu.ClientSize.Width)
                    {
                        if (newLeft > 0) newLeft = 0;
                        if (newLeft + newW < picContainerMenu.ClientSize.Width) newLeft = picContainerMenu.ClientSize.Width - newW;
                    }
                    else
                    {
                        newLeft = (picContainerMenu.ClientSize.Width - newW) / 2;
                    }

                    if (picMapaMenu.Height > picContainerMenu.ClientSize.Height)
                    {
                        if (newTop > 0) newTop = 0;
                        if (newTop + newH < picContainerMenu.ClientSize.Height) newTop = picContainerMenu.ClientSize.Height - newH;
                    }
                    else
                    {
                        newTop = (picContainerMenu.ClientSize.Height - newH) / 2;
                    }

                    picMapaMenu.Location = new Point(newLeft, newTop);
                }
                else
                {
                    int cx = (int)(posicionJugador.X * mapaCellBaseSize * mapaZoom - picContainerMenu.ClientSize.Width / 2);
                    int cy = (int)(posicionJugador.Y * mapaCellBaseSize * mapaZoom - picContainerMenu.ClientSize.Height / 2);
                    picMapaMenu.Location = new Point(-cx, -cy);
                }
            }
        }

        private void PicMapaMenu_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
            {
                mapaDragging = true;
                mapaDragStartPoint = Cursor.Position;
                mapaDragStartScroll = picMapaMenu.Location;
                picMapaMenu.Cursor = Cursors.Hand;
            }
        }

        private void PicMapaMenu_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mapaDragging) return;

            Point cur = Cursor.Position;
            int dx = cur.X - mapaDragStartPoint.X;
            int dy = cur.Y - mapaDragStartPoint.Y;

            int newX = mapaDragStartScroll.X + dx;
            int newY = mapaDragStartScroll.Y + dy;

            if (picMapaMenu.Width > picContainerMenu.ClientSize.Width)
            {
                if (newX > 0) newX = 0;
                if (newX + picMapaMenu.Width < picContainerMenu.ClientSize.Width)
                    newX = picContainerMenu.ClientSize.Width - picMapaMenu.Width;
            }
            else
            {
                newX = (picContainerMenu.ClientSize.Width - picMapaMenu.Width) / 2;
            }

            if (picMapaMenu.Height > picContainerMenu.ClientSize.Height)
            {
                if (newY > 0) newY = 0;
                if (newY + picMapaMenu.Height < picContainerMenu.ClientSize.Height)
                    newY = picContainerMenu.ClientSize.Height - picMapaMenu.Height;
            }
            else
            {
                newY = (picContainerMenu.ClientSize.Height - picMapaMenu.Height) / 2;
            }

            picMapaMenu.Location = new Point(newX, newY);
        }

        private void PicMapaMenu_MouseUp(object sender, MouseEventArgs e)
        {
            mapaDragging = false;
            picMapaMenu.Cursor = Cursors.Default;
        }

        private void PicMapaMenu_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            Point imgPoint = picMapaMenu.PointToClient(Cursor.Position);
            int imgX = (int)((imgPoint.X - picMapaMenu.Left) / mapaZoom);
            int imgY = (int)((imgPoint.Y - picMapaMenu.Top) / mapaZoom);

            int cellX = imgX / mapaCellBaseSize;
            int cellY = imgY / mapaCellBaseSize;

            if (cellX < 0 || cellY < 0 || cellX >= mapa.GetLength(1) || cellY >= mapa.GetLength(0)) return;

            Point nueva = new Point(cellX, cellY);
            if (marcasMapa.Contains(nueva)) marcasMapa.Remove(nueva);
            else marcasMapa.Add(nueva);

            GenerarBaseBitmap();
            UpdateScaledImage(picContainerMenu.PointToClient(Cursor.Position));
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (panelMenuMapa == null || picContainerMenu == null)
            {
                base.OnMouseWheel(e);
                return;
            }

            Point mouseInContainer = picContainerMenu.PointToClient(Cursor.Position);
            if (picContainerMenu.ClientRectangle.Contains(mouseInContainer))
            {
                float factor = e.Delta > 0 ? 1.5f : 0.7f; // ------------------------------------------ ZOOM IN / OUT
                mapaZoom *= factor;
                if (mapaZoom < mapaZoomMin) mapaZoom = mapaZoomMin;
                if (mapaZoom > mapaZoomMax) mapaZoom = mapaZoomMax;

                UpdateScaledImage(mouseInContainer);
                return;
            }

            base.OnMouseWheel(e);
        }

        private void CerrarMenuMapa()
        {
            try
            {
                if (picMapaMenu != null)
                {
                    picMapaMenu.MouseDown -= PicMapaMenu_MouseDown;
                    picMapaMenu.MouseMove -= PicMapaMenu_MouseMove;
                    picMapaMenu.MouseUp -= PicMapaMenu_MouseUp;
                    picMapaMenu.MouseClick -= PicMapaMenu_MouseClick;
                }
                try { picContainerMenu.MouseEnter -= (s, e) => picContainerMenu.Focus(); } catch { }

                if (picMapaMenu != null)
                {
                    try { picMapaMenu.Image?.Dispose(); } catch { }
                    picMapaMenu.Image = null;
                }

                try { mapaBaseBitmapMenu?.Dispose(); } catch { mapaBaseBitmapMenu = null; }

                if (panelMenuMapa != null)
                {
                    this.Controls.Remove(panelMenuMapa);
                    try { panelMenuMapa.Dispose(); } catch { }
                    panelMenuMapa = null;
                }
            }
            finally
            {
                juegoPausado = false;
            }
        }





















        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------- SPRITES
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------


        private void CargarSpritesTerreno()
        {
            string rutaBase = Path.Combine(Application.StartupPath, "Resources");

            // SPRITES DE TERRENO 
            spritesTerreno[0] = CargarImagenOpt(Path.Combine(rutaBase, "grass.png"), tamañoCelda, tamañoCelda);
            spritesTerreno[1] = CargarImagenOpt(Path.Combine(rutaBase, "rock.png"), tamañoCelda, tamañoCelda);
            spritesTerreno[2] = CargarImagenOpt(Path.Combine(rutaBase, "sand.png"), tamañoCelda, tamañoCelda);
            spritesTerreno[3] = CargarImagenOpt(Path.Combine(rutaBase, "water.png"), tamañoCelda, tamañoCelda);
            spritesTerreno[4] = CargarImagenOpt(Path.Combine(rutaBase, "path.png"), tamañoCelda, tamañoCelda);

            // SPRITES DE OBJETOS 
            spritesObjetos[1] = CargarImagenOpt(Path.Combine(rutaBase, "wall.png"), tamañoCelda, tamañoCelda);
            spritesObjetos[5] = CargarImagenOpt(Path.Combine(rutaBase, "tree.png"), tamañoCelda, tamañoCelda);
            spritesObjetos[6] = CargarImagenOpt(Path.Combine(rutaBase, "chest.png"), tamañoCelda, tamañoCelda);
            spritesObjetos[7] = CargarImagenOpt(Path.Combine(rutaBase, "npc.png"), tamañoCelda, tamañoCelda);
            spritesObjetos[8] = CargarImagenOpt(Path.Combine(rutaBase, "shop.png"), tamañoCelda, tamañoCelda);
            spritesObjetos[9] = CargarImagenOpt(Path.Combine(rutaBase, "door.png"), tamañoCelda, tamañoCelda);

            // NUEVOS SPRITES DECORATIVOS
            spritesObjetos[20] = CargarImagenOpt(Path.Combine(rutaBase, "fence_h.png"), tamañoCelda, tamañoCelda); // VALLA HORIZONTAL
            spritesObjetos[21] = CargarImagenOpt(Path.Combine(rutaBase, "fence_v.png"), tamañoCelda, tamañoCelda); // VALLA VERTICA
            spritesObjetos[22] = CargarImagenOpt(Path.Combine(rutaBase, "flowers.png"), tamañoCelda, tamañoCelda); // FLORES
            spritesObjetos[23] = CargarImagenOpt(Path.Combine(rutaBase, "rock_small.png"), tamañoCelda, tamañoCelda); // PIEDRA
            spritesObjetos[24] = CargarImagenOpt(Path.Combine(rutaBase, "sign.png"), tamañoCelda, tamañoCelda); // SEÑAL
            spritesObjetos[25] = CargarImagenOpt(Path.Combine(rutaBase, "lamp.png"), tamañoCelda, tamañoCelda); // FARO
        }

        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------- ENEMIGOS
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------

        private void InicializarEnemigos()
        {
            enemigos.Clear();
            enemigos.Add(new ENEMIGOS(new Point(10, 10)));
            enemigos.Add(new ENEMIGOS(new Point(15, 20)));
            enemigos.Add(new ENEMIGOS(new Point(30, 35)));
            enemigos.Add(new ENEMIGOS(new Point(20, 12)));
            enemigos.Add(new ENEMIGOS(new Point(45, 30)));

        }


        private void MoverEnemigos()
        {
            if (juegoPausado) return;

            foreach (var e in enemigos)
            {
                e.Mover(mapa, posicionJugador);

                if (e.HaAtrapadoJugador(posicionJugador))
                {
                    juegoPausado = true;
                    timerEnemigos.Stop();

                    using (COMBATE pantallaCombate = new COMBATE(pj, e, posicionJugador))
                    {
                        pantallaCombate.ShowDialog(this);
                        ProcesarResultadoCombate(pantallaCombate, e); 
                    }

                    juegoPausado = false;
                    timerEnemigos.Start();

                    break;
                }
            }

            panelMapa.Invalidate();
        }

        private void ProcesarResultadoCombate(COMBATE combate, ENEMIGOS enemigoCombate)
        {
            System.Diagnostics.Debug.WriteLine("=== PROCESANDO RESULTADO COMBATE ===");
            System.Diagnostics.Debug.WriteLine($"Jugador ganó: {combate.JugadorGano}, HP restante jugador: {combate.HPRestanteJugador}");

            if (combate.JugadorGano)
            {
                enemigos.Remove(enemigoCombate);

                pj.HP = combate.HPRestanteJugador;

                MessageBox.Show($"¡Victoria! Has derrotado al enemigo.\nHP restante: {pj.HP}",
                                "Combate Terminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (combate.HPRestanteJugador <= 0)
                {
                    MessageBox.Show("Has sido derrotado. Serás transportado a un lugar seguro.",
                                    "Derrota", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    posicionJugador = new Point(15, 15); 
                    pj.HP = pj.HP / 2;

                    camaraPos.X = Math.Max(0, Math.Min(posicionJugador.X - columnasVisibles / 2,
                                                       mapa.GetLength(1) - columnasVisibles));
                    camaraPos.Y = Math.Max(0, Math.Min(posicionJugador.Y - filasVisibles / 2,
                                                       mapa.GetLength(0) - filasVisibles));
                }
                else
                {
                    pj.HP = combate.HPRestanteJugador;
                    MessageBox.Show($"Lograste huir. HP restante: {pj.HP}",
                                    "Huida Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            ActualizarInterfazDespuesCombate();
            System.Diagnostics.Debug.WriteLine("=== RESULTADO COMBATE PROCESADO ===");
        }

        private void ActualizarInterfazDespuesCombate()
        {
            System.Diagnostics.Debug.WriteLine("=== ACTUALIZANDO INTERFAZ ===");
            System.Diagnostics.Debug.WriteLine($"pj.HP actual: {pj.HP}");
            System.Diagnostics.Debug.WriteLine($"statLabels contiene {statLabels.Count} elementos");

            foreach (var kvp in statLabels)
            {
                switch (kvp.Key)
                {
                    case "HP":
                        kvp.Value.Text = pj.HP.ToString();
                        System.Diagnostics.Debug.WriteLine($"✓ HP actualizado a: {pj.HP}"); // ⭐ NUEVO
                        break;
                    case "CA":
                        kvp.Value.Text = pj.CA.ToString();
                        break;
                    case "VEL":
                        kvp.Value.Text = pj.VEL.ToString();
                        break;
                    case "INI":
                        kvp.Value.Text = pj.INI.ToString();
                        break;
                }

                kvp.Value.Invalidate();
                kvp.Value.Update();
                kvp.Value.Refresh();
            }

            ActualizarDineroLabel();
            panelMapa.Invalidate();
            GuardarProgresoCombate();
            System.Diagnostics.Debug.WriteLine("=== INTERFAZ ACTUALIZADA ===");
        }

        private void GuardarProgresoCombate()
        {
            try
            {
                using var conn = new SQLiteConnection(BASE_DE_DATOS.cadenaConexion());
                conn.Open();

                string query = @"UPDATE Personajes SET 
                        HP = @HP, 
                        DIN = @DIN 
                        WHERE ID = @ID";

                using var cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@HP", pj.HP);
                cmd.Parameters.AddWithValue("@DIN", pj.DIN);
                cmd.Parameters.AddWithValue("@ID", pj.ID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar progreso: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------ CONTROLES Y TECLAS
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (juegoPausado) return base.ProcessCmdKey(ref msg, keyData);

            Point nuevaPos = posicionJugador;
            if (keyData == Keys.W) nuevaPos.Y--;
            else if (keyData == Keys.S) nuevaPos.Y++;
            else if (keyData == Keys.A) nuevaPos.X--;
            else if (keyData == Keys.D) nuevaPos.X++;
            else if (keyData == Keys.Escape) { MostrarMenuPausa(); return true; }
            else if (keyData == Keys.E) { MostrarMenuInventario(); return true; }
            else if (keyData == Keys.M) { MostrarMenuMapa(); return true; }
            else if (keyData == Keys.Q) { MostrarMenuMisiones(); return true; }
            else return base.ProcessCmdKey(ref msg, keyData);

            if (nuevaPos.X < 0 || nuevaPos.Y < 0 ||
                nuevaPos.X >= mapaBase.GetLength(1) || nuevaPos.Y >= mapaBase.GetLength(0))
                return base.ProcessCmdKey(ref msg, keyData);

            int tipoTerreno = mapaBase[nuevaPos.Y, nuevaPos.X];
            int tipoObjeto = mapaObjetos[nuevaPos.Y, nuevaPos.X];

            if (tipoTerreno == 1) 
                return base.ProcessCmdKey(ref msg, keyData);

            int[] objetosBloqueantes = {1, 5, 20, 21 }; 
            if (objetosBloqueantes.Contains(tipoObjeto))
                return base.ProcessCmdKey(ref msg, keyData);

            posicionJugador = nuevaPos;

            camaraPos.X = Math.Max(0, Math.Min(posicionJugador.X - columnasVisibles / 2,
                                               mapaBase.GetLength(1) - columnasVisibles));
            camaraPos.Y = Math.Max(0, Math.Min(posicionJugador.Y - filasVisibles / 2,
                                               mapaBase.GetLength(0) - filasVisibles));

            panelMapa.Invalidate();

            if (tipoObjeto != -1)
            {
                int[] objetosInteractuables = { 6, 7, 8, 9 }; 
                if (objetosInteractuables.Contains(tipoObjeto))
                {
                    ManejareInteraccion(nuevaPos, tipoObjeto);
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void EstablecerObjeto(int fila, int columna, int tipoObjeto)
        {
            if (fila >= 0 && fila < mapaObjetos.GetLength(0) &&
                columna >= 0 && columna < mapaObjetos.GetLength(1))
            {
                mapaObjetos[fila, columna] = tipoObjeto;
            }
        }

        private void EliminarObjeto(int fila, int columna)
        {
            if (fila >= 0 && fila < mapaObjetos.GetLength(0) &&
                columna >= 0 && columna < mapaObjetos.GetLength(1))
            {
                mapaObjetos[fila, columna] = -1;
            }
        }


        private void MostrarPersonaje()
        {
            Label lblNombre = new Label
            {
                Text = pj.NOMBRE,
                Location = new Point(20, 20),
                AutoSize = true,
                ForeColor = Color.White,
                Font = FUENTE.ObtenerFont(20)
            };
            Controls.Add(lblNombre);
        }

        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------------------- COFRE
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------

        private void MostrarRecompensaCofre(int dineroGanado, List<string> itemsObtenidos = null)
        {
            if (panelRecompensaCofre != null && panelRecompensaCofre.Visible)
            {
                CerrarPanelCofre();
            }

            juegoPausado = true;
            timerEnemigos.Stop();

            panelRecompensaCofre = new Panel
            {
                Size = new Size(600, 450),
                BackColor = Color.FromArgb(180, 0, 0, 0),
                Location = new Point((this.Width - 600) / 2, (this.Height - 450) / 2)
            };
            this.Controls.Add(panelRecompensaCofre);
            RedondearMenu(panelRecompensaCofre, 30);
            panelRecompensaCofre.BringToFront();

            Panel ventanaRecompensa = new Panel
            {
                Size = new Size(600, 450),
                BackColor = Color.FromArgb(240, 20, 25, 35)

            };
            panelRecompensaCofre.Controls.Add(ventanaRecompensa);
            RedondearMenu(ventanaRecompensa, 30);

            ventanaRecompensa.Location = new Point(
                (panelRecompensaCofre.Width - ventanaRecompensa.Width) / 2,
                (panelRecompensaCofre.Height - ventanaRecompensa.Height) / 2
            );

            ventanaRecompensa.BackColor = Color.FromArgb(0, 20, 25, 35);
            System.Windows.Forms.Timer animTimer = new System.Windows.Forms.Timer { Interval = 10 };
            float opacity = 0f;
            animTimer.Tick += (s, e) =>
            {
                opacity += 0.1f;
                if (opacity >= 1f)
                {
                    opacity = 1f;
                    animTimer.Stop();
                }
                int offsetY = (int)((1f - opacity) * 50);
                ventanaRecompensa.Location = new Point(
                    (panelRecompensaCofre.Width - ventanaRecompensa.Width) / 2,
                    (panelRecompensaCofre.Height - ventanaRecompensa.Height) / 2 - offsetY
                );
            };
            animTimer.Start();

            // BORDE DE COLOR
            Panel bordeSuperior = new Panel
            {
                Size = new Size(600, 8),
                Location = new Point(0, 0),
                BackColor = Color.Gold
            };
            ventanaRecompensa.Controls.Add(bordeSuperior);

            // ICONO DEL COFRE 
            Label lblIconoCofre = new Label
            {
                Text = "📦",
                Font = FUENTE.ObtenerFont(80),
                ForeColor = Color.Gold,
                AutoSize = false,
                Size = new Size(600, 120),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 30)
            };
            ventanaRecompensa.Controls.Add(lblIconoCofre);

            // ANIMACIÓN DEL COFRE 
            System.Windows.Forms.Timer parpadeoTimer = new System.Windows.Forms.Timer { Interval = 300 };
            bool brillando = false;
            parpadeoTimer.Tick += (s, e) =>
            {
                brillando = !brillando;
                lblIconoCofre.ForeColor = brillando ? Color.FromArgb(255, 223, 0) : Color.Gold;
            };
            parpadeoTimer.Start();

            // TÍTULO
            Label lblTitulo = new Label
            {
                Text = "¡COFRE ABIERTO!",
                Font = FUENTE.ObtenerFont(32),
                ForeColor = Color.FromArgb(255, 215, 0),
                AutoSize = false,
                Size = new Size(600, 50),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 150)
            };
            ventanaRecompensa.Controls.Add(lblTitulo);

            // LÍNEA DECORATIVA
            Panel lineaDecorativa = new Panel
            {
                Size = new Size(400, 3),
                Location = new Point(100, 210),
                BackColor = Color.FromArgb(100, 255, 215, 0)
            };
            ventanaRecompensa.Controls.Add(lineaDecorativa);

            // PANEL DE RECOMPENSAS
            Panel panelRecompensas = new Panel
            {
                Size = new Size(550, 150),
                Location = new Point(25, 230),
                BackColor = Color.FromArgb(200, 15, 20, 30)
            };
            ventanaRecompensa.Controls.Add(panelRecompensas);
            RedondearMenu(panelRecompensas, 15);

            int yPos = 20;

            // MOSTRAR DINERO
            if (dineroGanado > 0)
            {
                Panel panelDinero = new Panel
                {
                    Size = new Size(500, 50),
                    Location = new Point(25, yPos),
                    BackColor = Color.FromArgb(180, 40, 45, 35)
                };
                RedondearMenu(panelDinero, 10);

                Label lblDineroIcon = new Label
                {
                    Text = "🪙",
                    Font = FUENTE.ObtenerFont(28),
                    ForeColor = Color.Gold,
                    AutoSize = false,
                    Size = new Size(60, 50),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(10, 0)
                };
                panelDinero.Controls.Add(lblDineroIcon);

                Label lblDineroTexto = new Label
                {
                    Text = $"+{dineroGanado} Monedas de Oro",
                    Font = FUENTE.ObtenerFont(18),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(80, 12)
                };
                panelDinero.Controls.Add(lblDineroTexto);

                panelRecompensas.Controls.Add(panelDinero);
                yPos += 60;
            }

            // MOSTRAR ITEMS
            if (itemsObtenidos != null && itemsObtenidos.Count > 0)
            {
                foreach (var item in itemsObtenidos)
                {
                    Panel panelItem = new Panel
                    {
                        Size = new Size(500, 40),
                        Location = new Point(25, yPos),
                        BackColor = Color.FromArgb(180, 30, 40, 50)
                    };
                    RedondearMenu(panelItem, 8);

                    Label lblItemIcon = new Label
                    {
                        Text = "⭐",
                        Font = FUENTE.ObtenerFont(20),
                        ForeColor = Color.FromArgb(150, 255, 200),
                        AutoSize = false,
                        Size = new Size(40, 40),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(5, 0)
                    };
                    panelItem.Controls.Add(lblItemIcon);

                    Label lblItemTexto = new Label
                    {
                        Text = item,
                        Font = FUENTE.ObtenerFont(14),
                        ForeColor = Color.LightGray,
                        AutoSize = true,
                        Location = new Point(50, 10)
                    };
                    panelItem.Controls.Add(lblItemTexto);

                    panelRecompensas.Controls.Add(panelItem);
                    yPos += 45;
                }
            }

            // TEXTO
            Label lblAutoCerrar = new Label
            {
                Text = "Se cerrará automáticamente en 2 segundos...",
                Font = FUENTE.ObtenerFont(12),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(600, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 400)
            };
            ventanaRecompensa.Controls.Add(lblAutoCerrar);

            // TIMER PARA CERRAR AUTOMATICAMENTE
            timerCofre = new System.Windows.Forms.Timer { Interval = 2000 };
            timerCofre.Tick += (s, e) =>
            {
                timerCofre.Stop();
                parpadeoTimer.Stop();
                animTimer.Stop();
                CerrarPanelCofre();
            };
            timerCofre.Start();

            // PERMITIR CERRAR CON CLICK
            panelRecompensaCofre.Click += (s, e) =>
            {
                timerCofre.Stop();
                parpadeoTimer.Stop();
                animTimer.Stop();
                CerrarPanelCofre();
            };
            ventanaRecompensa.Click += (s, e) =>
            {
                timerCofre.Stop();
                parpadeoTimer.Stop();
                animTimer.Stop();
                CerrarPanelCofre();
            };
        }

        private void CerrarPanelCofre()
        {
            if (panelRecompensaCofre != null)
            {
                this.Controls.Remove(panelRecompensaCofre);
                panelRecompensaCofre.Dispose();
                panelRecompensaCofre = null;
            }
            juegoPausado = false;
            timerEnemigos.Start();
        }



        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------ TIENDA
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------

        private void AbrirTienda()
        {
            posicionAntesDeTienda = new Point(posicionJugador.X, posicionJugador.Y);
            juegoPausado = true;
            timerEnemigos.Stop();

            using (TIENDA tienda = new TIENDA(pj))
            {
                tienda.StartPosition = FormStartPosition.CenterParent;
                tienda.ShowDialog(this);
            }

            posicionJugador = posicionAntesDeTienda;
            camaraPos.X = Math.Max(0, Math.Min(posicionJugador.X - columnasVisibles / 2,
                                               mapa.GetLength(1) - columnasVisibles));
            camaraPos.Y = Math.Max(0, Math.Min(posicionJugador.Y - filasVisibles / 2,
                                               mapa.GetLength(0) - filasVisibles));
            juegoPausado = false;
            timerEnemigos.Start();
            ActualizarDineroLabel();
            panelMapa.Invalidate();
        }

        private void ActualizarPanelInferior()
        {
            CrearMenuInferior();
        }


        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------ MENU DE PAUSA
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------


        private bool juegoPausado = false;
        private Panel panelMenuPausa;
        private int indiceMenuActual = 0; 
        private List<string> subMenus = new List<string> { "MENÚ", "INVENTARIO", "MISIONES", "MAPA" };
        private PictureBox picPersonajeMenu;
        private Label lblStatsMenu;

        private Bitmap CapturarMapa()
        {
            Bitmap bmp = new Bitmap(panelMapa.Width, panelMapa.Height);
            panelMapa.DrawToBitmap(bmp, new Rectangle(0, 0, panelMapa.Width, panelMapa.Height));
            return bmp;

        }

        private void MostrarMenuPausa()
        {
            if (panelMenuPausa != null && panelMenuPausa.Visible) return;

            juegoPausado = true;
            indiceMenuActual = 0;

            // FONDO CONGELADO
            panelMenuPausa = new Panel
            {
                Dock = DockStyle.Fill,
                BackgroundImage = CapturarMapa(),
                BackgroundImageLayout = ImageLayout.Stretch,
            };

            // OVERLAY
            Panel overlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(150, 0, 0, 0)
            };
            panelMenuPausa.Controls.Add(overlay);

            // VENTANA PRINCIPAL
            Panel ventanaMenu = new Panel
            {
                Size = new Size(800, 600),
                BackColor = Color.FromArgb(235, 20, 20, 25),
                Location = new Point((overlay.Width - 800) / 2, (overlay.Height - 600) / 2)
            };
            overlay.Controls.Add(ventanaMenu);
            ventanaMenu.Anchor = AnchorStyles.None;
            RedondearMenu(ventanaMenu, 20);

            // BOTÓN CERRAR
            Button btnCerrar = new Button
            {
                Text = "X",
                Font = FUENTE.ObtenerFont(22),
                Size = new Size(45, 45),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Location = new Point(ventanaMenu.Width - 50, 5)
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnCerrar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnCerrar.MouseEnter += (s, e) => btnCerrar.ForeColor = Color.Red;
            btnCerrar.MouseLeave += (s, e) => btnCerrar.ForeColor = Color.White;
            btnCerrar.Click += (s, e) => CerrarMenuPausa();
            ventanaMenu.Controls.Add(btnCerrar);
            btnCerrar.BringToFront();

            // TÍTULO
            Label lblTitulo = new Label
            {
                Text = "JUEGO PAUSADO",
                Font = FUENTE.ObtenerFont(32),
                ForeColor = Color.FromArgb(255, 100, 100),
                AutoSize = true,
                Location = new Point((ventanaMenu.Width - 280) / 2, 25)
            };
            ventanaMenu.Controls.Add(lblTitulo);

            // LÍNEA DECORATIVA
            Panel lineaDecorativa = new Panel
            {
                Size = new Size(760, 2),
                BackColor = Color.FromArgb(100, 255, 255, 255),
                Location = new Point(20, 80)
            };
            ventanaMenu.Controls.Add(lineaDecorativa);

            // ------------------------------------------------------------- PANEL IZQUIERDO
            Panel panelPersonaje = new Panel
            {
                Size = new Size(280, 480),
                Location = new Point(20, 100),
                BackColor = Color.FromArgb(200, 15, 15, 20)
            };
            ventanaMenu.Controls.Add(panelPersonaje);
            RedondearMenu(panelPersonaje, 15);

            // IMAGEN DEL PERSONAJE
            int anchoImg = 160;
            int altoImg = 240;
            Bitmap bmpPj = new Bitmap(anchoImg, altoImg);
            using (Graphics g = Graphics.FromImage(bmpPj))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                g.DrawImage(picPersonaje.Image, 0, 0, anchoImg, altoImg);
            }

            PictureBox picPjMenu = new PictureBox
            {
                Image = bmpPj,
                SizeMode = PictureBoxSizeMode.Normal,
                Size = new Size(anchoImg, altoImg),
                Location = new Point((panelPersonaje.Width - anchoImg) / 2, 20)
            };
            panelPersonaje.Controls.Add(picPjMenu);

            // NOMBRE DEL PERSONAJE
            Label lblNombrePj = new Label
            {
                Text = pj.NOMBRE.ToUpper(),
                Font = FUENTE.ObtenerFont(22),
                ForeColor = Color.Gold,
                AutoSize = false,
                Size = new Size(260, 35),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(10, 270)
            };
            panelPersonaje.Controls.Add(lblNombrePj);

            // INFO BASICA
            Label lblInfoBasica = new Label
            {
                Text = $"{pj.RAZA} {pj.SUBRAZA}\n{pj.CLASE} - Nivel {pj.LVL}\n{pj.ALINEAMIENTO}",
                Font = FUENTE.ObtenerFont(14),
                ForeColor = Color.LightGray,
                AutoSize = false,
                Size = new Size(260, 70),
                TextAlign = ContentAlignment.TopCenter,
                Location = new Point(10, 310)
            };
            panelPersonaje.Controls.Add(lblInfoBasica);

            // STATS
            Panel panelStatsPrincipales = new Panel
            {
                Size = new Size(260, 80),
                Location = new Point(10, 390),
                BackColor = Color.FromArgb(150, 10, 10, 15)
            };
            panelPersonaje.Controls.Add(panelStatsPrincipales);
            RedondearMenu(panelStatsPrincipales, 10);

            string[] statsLabels = { "HP", "CA", "VEL", "INI" };
            int[] statsValues = { pj.HP, pj.CA, pj.VEL, pj.INI };
    
            for (int i = 0; i < 4; i++)
            {
                int x = (i % 2) * 130;
                int y = (i / 2) * 40;

                Label lblStat = new Label
                {
                    Text = $"{statsLabels[i]}: {statsValues[i]}",
                    Font = FUENTE.ObtenerFont(14),
                    ForeColor = Color.White,
                    AutoSize = false,
                    Size = new Size(120, 30),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Location = new Point(10 + x, 5 + y)
                };
                panelStatsPrincipales.Controls.Add(lblStat);
            }

            // -------------------------------------------------------------- PANEL DERECHO
            Panel panelOpciones = new Panel
            {
                Size = new Size(470, 480),
                Location = new Point(310, 100),
                BackColor = Color.FromArgb(200, 15, 15, 20)
            };
            ventanaMenu.Controls.Add(panelOpciones);
            RedondearMenu(panelOpciones, 15);

            // TITULO DEL PANEL
            Label lblTituloOpciones = new Label
            {
                Text = "OPCIONES",
                Font = FUENTE.ObtenerFont(20),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            panelOpciones.Controls.Add(lblTituloOpciones);

            // PANEL DE ATRIBUTOS
            Panel panelAtributos = new Panel
            {
                Size = new Size(430, 140),
                Location = new Point(20, 60),
                BackColor = Color.FromArgb(150, 10, 10, 15)
            };
            panelOpciones.Controls.Add(panelAtributos);
            RedondearMenu(panelAtributos, 10);

            Label lblAtributosTitulo = new Label
            {
                Text = "ATRIBUTOS",
                Font = FUENTE.ObtenerFont(14),
                ForeColor = Color.Cyan,
                AutoSize = true,
                Location = new Point(15, 10)
            };
            panelAtributos.Controls.Add(lblAtributosTitulo);

            string[] atributos = { "STR", "DEX", "CON", "INT", "WIS", "CHA" };
            int[] valores = { pj.STR, pj.DEX, pj.CON, pj.INT, pj.WIS, pj.CHA };

            for (int i = 0; i < 6; i++)
            {
                int col = i % 3;
                int row = i / 3;
                int x = col * 140;
                int y = row * 50;

                Panel atributoBox = new Panel
                {
                    Size = new Size(130, 45),
                    Location = new Point(10 + x, 40 + y),
                    BackColor = Color.FromArgb(100, 20, 20, 30)
                };
                panelAtributos.Controls.Add(atributoBox);
                RedondearMenu(atributoBox, 8);

                Label lblNombre = new Label
                {
                    Text = atributos[i],
                    Font = FUENTE.ObtenerFont(11),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(8, 5)
                };
                atributoBox.Controls.Add(lblNombre);

                Label lblValor = new Label
                {
                    Text = valores[i].ToString(),
                    Font = FUENTE.ObtenerFont(18),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(90, 10)
                };
                atributoBox.Controls.Add(lblValor);
            }

            // BOTONES DE ACCION
            int btnY = 220;
            int btnSpacing = 65;

            Button btnContinuar = CrearBotonMenuPausa("CONTINUAR JUEGO", Color.FromArgb(50, 150, 80), 430);
            btnContinuar.Location = new Point(20, btnY);
            btnContinuar.Click += (s, e) => CerrarMenuPausa();
            panelOpciones.Controls.Add(btnContinuar);

            Button btnGuardar = CrearBotonMenuPausa("GUARDAR PARTIDA", Color.FromArgb(60, 120, 200), 430);
            btnGuardar.Location = new Point(20, btnY + btnSpacing);
            btnGuardar.Click += (s, e) => GuardarPartida();
            panelOpciones.Controls.Add(btnGuardar);


            Button btnGuardarSalir = CrearBotonMenuPausa("GUARDAR Y SALIR", Color.FromArgb(200, 140, 50), 430);
            btnGuardarSalir.Location = new Point(20, btnY + btnSpacing * 2);
            btnGuardarSalir.Click += (s, e) =>
            {
                GuardarPartida();
                var mainForm = new MENU_PRINCIPAL();
                mainForm.Show();
                this.Close();
            };
            panelOpciones.Controls.Add(btnGuardarSalir);

                    Button btnSalir = CrearBotonMenuPausa("SALIR SIN GUARDAR", Color.FromArgb(180, 60, 60), 430);
                    btnSalir.Location = new Point(20, btnY + btnSpacing * 3);
                    btnSalir.Click += (s, e) =>
                    {
                        var resultado = MessageBox.Show(
                            "¿CHAVAL ESTAS SEGURO?\nVAS A PERDER TODO LO QUE NO SE HAYA GUARDADO.",
                            "SI COJONES",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        );

                        if (resultado == DialogResult.Yes)
                        {
                            Application.Exit();
                        }
                    };
                    panelOpciones.Controls.Add(btnSalir);


            Button flechaIzq = new Button
            {
                Text = "<",
                Width = 50,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = FUENTE.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Location = new Point(10, (this.ClientSize.Height - 50) / 2)
            };
            flechaIzq.FlatAppearance.BorderSize = 0;
            flechaIzq.FlatAppearance.MouseOverBackColor = Color.Transparent;
            flechaIzq.FlatAppearance.MouseDownBackColor = Color.Transparent;
            flechaIzq.MouseEnter += (s, e) => flechaIzq.ForeColor = Color.DarkGray;
            flechaIzq.MouseLeave += (s, e) => flechaIzq.ForeColor = Color.White;
            flechaIzq.Click += (s, e) => Navegar_FlechaIzquierda();
            overlay.Controls.Add(flechaIzq);

            Button flechaDer = new Button
            {
                Text = ">",
                Width = 50,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = FUENTE.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Location = new Point(this.ClientSize.Width - 50 - 10, (this.ClientSize.Height - 50) / 2)
            };
            flechaDer.FlatAppearance.BorderSize = 0;
            flechaDer.FlatAppearance.MouseOverBackColor = Color.Transparent;
            flechaDer.FlatAppearance.MouseDownBackColor = Color.Transparent;
            flechaDer.MouseEnter += (s, e) => flechaDer.ForeColor = Color.DarkGray;
            flechaDer.MouseLeave += (s, e) => flechaDer.ForeColor = Color.White;
            flechaDer.Click += (s, e) => Navegar_FlechaDerecha();
            overlay.Controls.Add(flechaDer);

            this.Controls.Add(panelMenuPausa);
            panelMenuPausa.BringToFront();
        }

        private Button CrearBotonMenuPausa(string texto, Color color, int ancho)
        {
            Button btn = new Button
            {
                Text = texto,
                Width = ancho,
                Height = 55,
                FlatStyle = FlatStyle.Flat,
                BackColor = color,
                ForeColor = Color.White,
                Font = FUENTE.ObtenerFont(16),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            RedondearBoton(btn, 12);
    
            Color colorOriginal = color;
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(
                    Math.Min(255, colorOriginal.R + 30),
                    Math.Min(255, colorOriginal.G + 30),
                    Math.Min(255, colorOriginal.B + 30)
                );
            };
            btn.MouseLeave += (s, e) => btn.BackColor = colorOriginal;
    
            return btn;
        }

        private Button CrearBotonMenu(string texto, Color color, EventHandler click)
        {
            Button btn = new Button
            {
                Text = texto,
                Width = 480,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                BackColor = color,
                ForeColor = Color.White,
                Font = FUENTE.ObtenerFont(18),
                Margin = new Padding(0, 10, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            RedondearBoton(btn, 25);
            btn.Click += click;
            return btn;
        }

        private void CerrarMenuPausa()
        {
            if (panelMenuPausa != null)
            {
                this.Controls.Remove(panelMenuPausa);
                panelMenuPausa.Dispose();
                panelMenuPausa = null;
            }
            juegoPausado = false;
        }

        private void Navegar_FlechaIzquierda()
        {
            indiceMenuActual--;
            if (indiceMenuActual < 0) indiceMenuActual = 3;

            CerrarMenuActual();
            MostrarMenuSegunIndice();
        }

        private void Navegar_FlechaDerecha()
        {
            indiceMenuActual++;
            if (indiceMenuActual > 3) indiceMenuActual = 0;

            CerrarMenuActual();
            MostrarMenuSegunIndice();
        }

        private void MostrarMenuSegunIndice()
        {
            switch (indiceMenuActual)
            {
                case 0: MostrarMenuPausa(); break;
                case 1: MostrarMenuInventario(); break;
                case 2: MostrarMenuMapa(); break;
                case 3: MostrarMenuMisiones(); break;
            }
        }

        private void CerrarMenuActual()
        {
            if (panelMenuPausa != null)
            {
                this.Controls.Remove(panelMenuPausa);
                panelMenuPausa.Dispose();
                panelMenuPausa = null;
            }

            if (panelMenuMapa != null)
            {
                CerrarMenuMapa();
            }
        }

        private void GuardarPartida()
        {
            if (pj == null) return;

            try
            {
                using var conn = new SQLiteConnection(BASE_DE_DATOS.cadenaConexion());
                conn.Open();
                using var tran = conn.BeginTransaction();

                string queryPersonaje = @"
                UPDATE Personajes SET 
                DIN = @DIN,
                HP = @HP,
                LVL = @LVL,
                CA = @CA,
                VEL = @VEL,
                INI = @INI
                WHERE ID = @ID;";

                using (var cmd = new SQLiteCommand(queryPersonaje, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@DIN", pj.DIN);
                    cmd.Parameters.AddWithValue("@HP", pj.HP);
                    cmd.Parameters.AddWithValue("@LVL", pj.LVL);
                    cmd.Parameters.AddWithValue("@CA", pj.CA);
                    cmd.Parameters.AddWithValue("@VEL", pj.VEL);
                    cmd.Parameters.AddWithValue("@INI", pj.INI);
                    cmd.Parameters.AddWithValue("@ID", pj.ID);
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                MessageBox.Show("¡PARTIDA GUARDADA CORRECTAMENTE!", "GUARDAR",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR AL GUARDAR: {ex.Message}", "ERROR",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void RedondearMenu(Panel panel, int radio)
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


        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------ MENU DE INVENTARIO
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------

        private void MostrarMenuInventario()
        {
            if (panelMenuPausa != null && panelMenuPausa.Visible) return;

            juegoPausado = true;
            indiceMenuActual = 1;

            // FONDO CONGELADO
            panelMenuPausa = new Panel
            {
                Dock = DockStyle.Fill,
                BackgroundImage = CapturarMapa(),
                BackgroundImageLayout = ImageLayout.Stretch,
            };

            // OVERLAY
            Panel overlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(140, 0, 0, 0)
            };
            panelMenuPausa.Controls.Add(overlay);

            // VENTANA PRINCIPAL
            Panel ventanaMenu = new Panel
            {
                Size = new Size(1075, 600),
                BackColor = Color.FromArgb(230, 25, 25, 35),
                Location = new Point((overlay.Width - 1075) / 2, (overlay.Height - 600) / 2)
            };
            overlay.Controls.Add(ventanaMenu);
            ventanaMenu.Anchor = AnchorStyles.None;
            RedondearMenu(ventanaMenu, 20);

            // BOTON CERRAR
            Button btnCerrar = new Button
            {
                Text = "X",
                Font = FUENTE.ObtenerFont(18),
                Size = new Size(40, 40),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Location = new Point(ventanaMenu.Width - 45, 5)
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnCerrar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnCerrar.MouseEnter += (s, e) => btnCerrar.ForeColor = Color.Red;
            btnCerrar.MouseLeave += (s, e) => btnCerrar.ForeColor = Color.White;
            btnCerrar.Click += (s, e) => CerrarMenuPausa();
            ventanaMenu.Controls.Add(btnCerrar);
            btnCerrar.BringToFront();

            // TITULO
            Label lblTitulo = new Label
            {
                Text = "INVENTARIO",
                Font = FUENTE.ObtenerFont(28),
                ForeColor = Color.Gold,
                AutoSize = true,
                Location = new Point(30, 15)
            };
            ventanaMenu.Controls.Add(lblTitulo);

            // ---------------------------------------------------------------------- PANEL IZQUIERDO
            Panel panelIzquierdo = new Panel
            {
                Size = new Size(250, 520),
                Location = new Point(20, 70),
                BackColor = Color.FromArgb(180, 15, 15, 25)
            };
            ventanaMenu.Controls.Add(panelIzquierdo);
            RedondearMenu(panelIzquierdo, 15);

            // IMAGEN DEL PERSONAJE
            int anchoImg = 120;
            int altoImg = 180;
            Bitmap bmpPj = new Bitmap(anchoImg, altoImg);
            using (Graphics g = Graphics.FromImage(bmpPj))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                g.DrawImage(picPersonaje.Image, 0, 0, anchoImg, altoImg);
            }

            PictureBox picPjInventario = new PictureBox
            {
                Image = bmpPj,
                SizeMode = PictureBoxSizeMode.Normal,
                Size = new Size(anchoImg, altoImg),
                Location = new Point((panelIzquierdo.Width - anchoImg) / 2, 20)
            };
            panelIzquierdo.Controls.Add(picPjInventario);

            // STATS DEL PERSONAJE
            Label lblStats = new Label
            {
                Text = $"{pj.NOMBRE}\n" +
                       $"────────────────\n" +
                       $"Nivel: {pj.LVL}\n" +
                       $"Clase: {pj.CLASE}\n" +
                       $"Raza: {pj.RAZA}\n" +
                       $"────────────────\n" +
                       $"HP: {pj.HP}\n" +
                       $"CA: {pj.CA}\n" +
                       $"Velocidad: {pj.VEL}",
                ForeColor = Color.White,
                Font = FUENTE.ObtenerFont(14),
                AutoSize = false,
                Size = new Size(230, 300),
                Location = new Point(10, 210),
                TextAlign = ContentAlignment.TopCenter
            };
            panelIzquierdo.Controls.Add(lblStats);

            // ---------------------------------------------------------------------- PANEL DERECHO
            Panel panelDerecho = new Panel
            {
                Size = new Size(775, 520),
                Location = new Point(280, 70),
                BackColor = Color.FromArgb(180, 15, 15, 25)
            };
            ventanaMenu.Controls.Add(panelDerecho);
            RedondearMenu(panelDerecho, 15);

            // PESTAÑAS
            FlowLayoutPanel panelPestañas = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Size = new Size(775, 50),
                Location = new Point(10, 10),
                BackColor = Color.Transparent,
                WrapContents = false
            };
            panelDerecho.Controls.Add(panelPestañas);

            // PANEL DE CONTENIDO
            Panel panelContenido = new Panel
            {
                Size = new Size(750, 450),
                Location = new Point(10, 65),
                BackColor = Color.FromArgb(200, 10, 10, 15),
                AutoScroll = true
            };
            panelDerecho.Controls.Add(panelContenido);
            RedondearMenu(panelContenido, 10);

            // BOTONES DE PESTAÑAS
            Button btnArmas = CrearBotonPestaña("ARMAS", Color.Firebrick);
            Button btnHechizos = CrearBotonPestaña("HECHIZOS", Color.MediumPurple);
            Button btnHabilidades = CrearBotonPestaña("HABILIDADES", Color.DodgerBlue);
            Button btnConsumibles = CrearBotonPestaña("CONSUMIBLES", Color.Yellow);

            panelPestañas.Controls.Add(btnArmas);
            panelPestañas.Controls.Add(btnHechizos);
            panelPestañas.Controls.Add(btnHabilidades);
            panelPestañas.Controls.Add(btnConsumibles);

            // CARGAR DATOS DE LA BASE DE DATOS
            List<string> armas = BASE_DE_DATOS.ObtenerArmas(pj.ID);
            List<string> hechizos = BASE_DE_DATOS.ObtenerHechizos(pj.ID);
            List<Habilidad> habilidades = BASE_DE_DATOS.ObtenerHabilidades(pj.ID);

            void MostrarConsumibles()
            {
                panelContenido.Controls.Clear();

                var inventario = BASE_DE_DATOS.ObtenerInventario(pj.ID);

                if (inventario.Count == 0)
                {
                    Label lblVacio = new Label
                    {
                        Text = "Tu inventario está vacío\nVisita la tienda para comprar items",
                        Font = FUENTE.ObtenerFont(16),
                        ForeColor = Color.Gray,
                        AutoSize = false,
                        Size = new Size(730, 400),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(10, 50)
                    };
                    panelContenido.Controls.Add(lblVacio);
                    return;
                }

                int y = 10;
                foreach (var item in inventario)
                {
                    Panel itemPanel = new Panel
                    {
                        Size = new Size(730, 70),
                        Location = new Point(10, y),
                        BackColor = Color.FromArgb(150, 30, 40, 30)
                    };
                    RedondearMenu(itemPanel, 8);

                    // ICONO SEGUN TIPO DE ITEM
                    string icono = item.tipo switch
                    {
                        "arma" => "⚔",
                        "armadura" => "🛡",
                        "consumible" => "⚗",
                        "accesorio" => "💍",
                        _ => "📦"
                    };

                    Label lblIcono = new Label
                    {
                        Text = icono,
                        Font = FUENTE.ObtenerFont(24),
                        ForeColor = Color.White,
                        Size = new Size(50, 50),
                        Location = new Point(10, 10),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    itemPanel.Controls.Add(lblIcono);

                    // NOMBRE
                    Label lblNombre = new Label
                    {
                        Text = item.nombre,
                        Font = FUENTE.ObtenerFont(16),
                        ForeColor = Color.White,
                        AutoSize = true,
                        Location = new Point(70, 15)
                    };
                    itemPanel.Controls.Add(lblNombre);

                    // CANTIDAD
                    Label lblCantidad = new Label
                    {
                        Text = $"x{item.cantidad}",
                        Font = FUENTE.ObtenerFont(18),
                        ForeColor = Color.Gold,
                        AutoSize = true,
                        Location = new Point(480, 20)
                    };
                    itemPanel.Controls.Add(lblCantidad);

                    panelContenido.Controls.Add(itemPanel);
                    y += 80;
                }
            }

            btnConsumibles.Click += (s, e) =>
            {
                ActivarPestaña(btnConsumibles, new[] { btnArmas, btnHechizos, btnHabilidades });
                MostrarConsumibles();
            };

            // FUNCION PARA MOSTRAR ARMAS
            void MostrarArmas()
            {
                panelContenido.Controls.Clear();

                if (armas.Count == 0)
                {
                    Label lblVacio = new Label
                    {
                        Text = "No tienes armas equipadas",
                        Font = FUENTE.ObtenerFont(16),
                        ForeColor = Color.Gray,
                        AutoSize = false,
                        Size = new Size(730, 400),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(10, 50)
                    };
                    panelContenido.Controls.Add(lblVacio);
                    return;
                }

                int y = 10;
                foreach (var arma in armas)
                {
                    Panel itemPanel = new Panel
                    {
                        Size = new Size(730, 60),
                        Location = new Point(10, y),
                        BackColor = Color.FromArgb(150, 40, 20, 20)
                    };
                    RedondearMenu(itemPanel, 8);

                    // ICONO DE ARMA
                    Label icono = new Label
                    {
                        Text = "⚔",
                        Font = FUENTE.ObtenerFont(28),
                        ForeColor = Color.OrangeRed,
                        Size = new Size(50, 50),
                        Location = new Point(10, 5),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    itemPanel.Controls.Add(icono);

                    // NOMBRE DEL ARMA
                    Label nombre = new Label
                    {
                        Text = arma,
                        Font = FUENTE.ObtenerFont(18),
                        ForeColor = Color.White,
                        AutoSize = true,
                        Location = new Point(70, 15)
                    };
                    itemPanel.Controls.Add(nombre);

                    panelContenido.Controls.Add(itemPanel);
                    y += 70;
                }
            }

            // FUNCION PARA MOSTRAR HECHIZOS
            void MostrarHechizos()
            {
                panelContenido.Controls.Clear();

                if (hechizos.Count == 0)
                {
                    Label lblVacio = new Label
                    {
                        Text = "No conoces ningún hechizo",
                        Font = FUENTE.ObtenerFont(16),
                        ForeColor = Color.Gray,
                        AutoSize = false,
                        Size = new Size(730, 400),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(10, 50)
                    };
                    panelContenido.Controls.Add(lblVacio);
                    return;
                }

                int y = 10;
                foreach (var hechizo in hechizos)
                {
                    Panel itemPanel = new Panel
                    {
                        Size = new Size(730, 60),
                        Location = new Point(10, y),
                        BackColor = Color.FromArgb(150, 30, 20, 50)
                    };
                    RedondearMenu(itemPanel, 8);

                    // ICONO DE HECHIZO
                    Label icono = new Label
                    {
                        Text = "✨",
                        Font = FUENTE.ObtenerFont(24),
                        ForeColor = Color.MediumPurple,
                        Size = new Size(50, 50),
                        Location = new Point(10, 5),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    itemPanel.Controls.Add(icono);

                    // NOMBRE DEL HECHIZO
                    Label nombre = new Label
                    {
                        Text = hechizo,
                        Font = FUENTE.ObtenerFont(18),
                        ForeColor = Color.White,
                        AutoSize = true,
                        Location = new Point(70, 15)
                    };
                    itemPanel.Controls.Add(nombre);

                    panelContenido.Controls.Add(itemPanel);
                    y += 70;
                }
            }

            // FUNCION PARA MOSTRAR HABILIDADES
            void MostrarHabilidades()
            {
                panelContenido.Controls.Clear();

                if (habilidades.Count == 0)
                {
                    Label lblVacio = new Label
                    {
                        Text = "No tienes habilidades registradas",
                        Font = FUENTE.ObtenerFont(16),
                        ForeColor = Color.Gray,
                        AutoSize = false,
                        Size = new Size(730, 400),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(10, 50)
                    };
                    panelContenido.Controls.Add(lblVacio);
                    return;
                }

                int y = 10;
                foreach (var hab in habilidades)
                {
                    Panel itemPanel = new Panel
                    {
                        Size = new Size(730, 80),
                        Location = new Point(10, y),
                        BackColor = Color.FromArgb(150, 20, 30, 50)
                    };
                    RedondearMenu(itemPanel, 8);

                    // NOMBRE DE LA HABILIDAD
                    Label nombre = new Label
                    {
                        Text = hab.NOMBRE,
                        Font = FUENTE.ObtenerFont(18),
                        ForeColor = Color.Cyan,
                        AutoSize = true,
                        Location = new Point(15, 10)
                    };
                    itemPanel.Controls.Add(nombre);

                    // DETALLES
                    Label detalles = new Label
                    {
                        Text = $"Stat: {hab.STAT_ASOCIADO} | Mod: {hab.MODIFICADOR_STAT:+0;-#} | Comp: {hab.BONIFICADOR_COMPETENCIA:+0;-#}",
                        Font = FUENTE.ObtenerFont(12),
                        ForeColor = Color.LightGray,
                        AutoSize = true,
                        Location = new Point(15, 38)
                    };
                    itemPanel.Controls.Add(detalles);

                    // TOTAL
                    Label total = new Label
                    {
                        Text = hab.TOTAL.ToString("+0;-#"),
                        Font = FUENTE.ObtenerFont(24),
                        ForeColor = Color.LimeGreen,
                        AutoSize = true,
                        Location = new Point(480, 20)
                    };
                    itemPanel.Controls.Add(total);

                    panelContenido.Controls.Add(itemPanel);
                    y += 90;
                }
            }

            // EVENTOS DE LAS PESTAÑAS
            void ActivarPestaña(Button activo, Button[] otros)
            {
                activo.BackColor = activo.Tag as Color? ?? Color.Gray;
                foreach (var otro in otros)
                {
                    otro.BackColor = Color.FromArgb(100, 30, 30, 30);
                }
            }

            btnArmas.Click += (s, e) =>
            {
                ActivarPestaña(btnArmas, new[] { btnHechizos, btnHabilidades });
                MostrarArmas();
            };

            btnHechizos.Click += (s, e) =>
            {
                ActivarPestaña(btnHechizos, new[] { btnArmas, btnHabilidades });
                MostrarHechizos();
            };

            btnHabilidades.Click += (s, e) =>
            {
                ActivarPestaña(btnHabilidades, new[] { btnArmas, btnHechizos });
                MostrarHabilidades();
            };

            // MOSTRAR ARMAS POR DEFECTO
            btnArmas.BackColor = Color.Firebrick;
            MostrarArmas();

            // FLECHAS DEL CARRUSEL
            Button flechaIzq = new Button
            {
                Text = "<",
                Width = 50,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = FUENTE.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Location = new Point(10, (this.ClientSize.Height - 50) / 2)
            };
            flechaIzq.FlatAppearance.BorderSize = 0;
            flechaIzq.FlatAppearance.MouseOverBackColor = Color.Transparent;
            flechaIzq.FlatAppearance.MouseDownBackColor = Color.Transparent;
            flechaIzq.MouseEnter += (s, e) => flechaIzq.ForeColor = Color.DarkGray;
            flechaIzq.MouseLeave += (s, e) => flechaIzq.ForeColor = Color.White;
            flechaIzq.Click += (s, e) => Navegar_FlechaIzquierda();
            overlay.Controls.Add(flechaIzq);

            Button flechaDer = new Button
            {
                Text = ">",
                Width = 50,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = FUENTE.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Location = new Point(this.ClientSize.Width - 50 - 10, (this.ClientSize.Height - 50) / 2)
            };
            flechaDer.FlatAppearance.BorderSize = 0;
            flechaDer.FlatAppearance.MouseOverBackColor = Color.Transparent;
            flechaDer.FlatAppearance.MouseDownBackColor = Color.Transparent;
            flechaDer.MouseEnter += (s, e) => flechaDer.ForeColor = Color.DarkGray;
            flechaDer.MouseLeave += (s, e) => flechaDer.ForeColor = Color.White;
            flechaDer.Click += (s, e) => Navegar_FlechaDerecha();
            overlay.Controls.Add(flechaDer);

            // AÑADIR TODO
            this.Controls.Add(panelMenuPausa);
            panelMenuPausa.BringToFront();
        }

        // FUNCION AUXILIAR PARA CREAR BOTONES DE PESTAÑA
        private Button CrearBotonPestaña(string texto, Color colorActivo)
        {
            Button btn = new Button
            {
                Text = texto,
                Width = 185,
                Height = 45,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(100, 30, 30, 30),
                ForeColor = Color.White,
                Font = FUENTE.ObtenerFont(14),
                Cursor = Cursors.Hand,
                Margin = new Padding(2),
                Tag = colorActivo
            };
            btn.FlatAppearance.BorderSize = 0;
            RedondearBoton(btn, 10);

            btn.MouseEnter += (s, e) =>
            {
                if (btn.BackColor == Color.FromArgb(100, 30, 30, 30))
                    btn.BackColor = Color.FromArgb(120, 50, 50, 50);
            };
            btn.MouseLeave += (s, e) =>
            {
                if (btn.BackColor == Color.FromArgb(120, 50, 50, 50))
                    btn.BackColor = Color.FromArgb(100, 30, 30, 30);
            };

            return btn;
        }

        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------ MENU DE MISIONES
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------------------

        private void MostrarMenuMisiones()
        {
            if (panelMenuPausa != null && panelMenuPausa.Visible) return;

            juegoPausado = true;
            indiceMenuActual = 3;

            // FONDO CONGELADO
            panelMenuPausa = new Panel
            {
                Dock = DockStyle.Fill,
                BackgroundImage = CapturarMapa(),
                BackgroundImageLayout = ImageLayout.Stretch,
            };

            // OVERLAY
            Panel overlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(140, 0, 0, 0)
            };
            panelMenuPausa.Controls.Add(overlay);

            // VENTANA PRINCIPAL
            Panel ventanaMenu = new Panel
            {
                Size = new Size(950, 650),
                BackColor = Color.FromArgb(230, 20, 25, 30),
                Location = new Point((overlay.Width - 950) / 2, (overlay.Height - 650) / 2)
            };
            overlay.Controls.Add(ventanaMenu);
            ventanaMenu.Anchor = AnchorStyles.None;
            RedondearMenu(ventanaMenu, 20);

            // BOTON CERRAR
            Button btnCerrar = new Button
            {
                Text = "X",
                Font = FUENTE.ObtenerFont(18),
                Size = new Size(40, 40),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Location = new Point(ventanaMenu.Width - 45, 5)
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnCerrar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnCerrar.MouseEnter += (s, e) => btnCerrar.ForeColor = Color.Red;
            btnCerrar.MouseLeave += (s, e) => btnCerrar.ForeColor = Color.White;
            btnCerrar.Click += (s, e) => CerrarMenuPausa();
            ventanaMenu.Controls.Add(btnCerrar);
            btnCerrar.BringToFront();

            // TITULO
            Label lblTitulo = new Label
            {
                Text = "LIBRO DE MISIONES",
                Font = FUENTE.ObtenerFont(28),
                ForeColor = Color.FromArgb(255, 220, 120),
                AutoSize = true,
                Location = new Point(30, 15)
            };
            ventanaMenu.Controls.Add(lblTitulo);

            // SUBTITULO
            Label lblSubtitulo = new Label
            {
                Text = "Aventuras y Tareas Pendientes",
                Font = FUENTE.ObtenerFont(14),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(30, 50)
            };
            ventanaMenu.Controls.Add(lblSubtitulo);

            // PANEL DE PESTAÑAS
            FlowLayoutPanel panelPestañas = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Size = new Size(910, 50),
                Location = new Point(20, 85),
                BackColor = Color.Transparent,
                WrapContents = false
            };
            ventanaMenu.Controls.Add(panelPestañas);

            // PANEL DE CONTENIDO DE MISIONES
            Panel panelMisiones = new Panel
            {
                Size = new Size(910, 490),
                Location = new Point(20, 145),
                BackColor = Color.FromArgb(200, 15, 15, 20),
                AutoScroll = true
            };
            ventanaMenu.Controls.Add(panelMisiones);
            RedondearMenu(panelMisiones, 12);

            // BOTONES DE PESTAÑAS
            Button btnActivas = CrearBotonPestañaMision("ACTIVAS", Color.FromArgb(50, 180, 80));
            Button btnCompletadas = CrearBotonPestañaMision("COMPLETADAS", Color.FromArgb(100, 150, 220));
            Button btnFallidas = CrearBotonPestañaMision("FALLIDAS", Color.FromArgb(200, 60, 60));
            Button btnDisponibles = CrearBotonPestañaMision("DISPONIBLES", Color.FromArgb(220, 170, 50));

            panelPestañas.Controls.Add(btnActivas);
            panelPestañas.Controls.Add(btnCompletadas);
            panelPestañas.Controls.Add(btnFallidas);
            panelPestañas.Controls.Add(btnDisponibles);

            // DATOS DE EJEMPLO (AQUI SE CONECTARA A LA BASE DE DATOS)
            var misionesActivas = new List<dynamic>
            {
                new { Titulo = "El Dragón de las Montañas", Descripcion = "Derrota al dragón que aterroriza la aldea de Thornwood", Recompensa = "500 oro + Espada legendaria", Progreso = 60, Nivel = 15 },
                new { Titulo = "Recuperar el Anillo Perdido", Descripcion = "Encuentra el anillo de la Reina en las Catacumbas Oscuras", Recompensa = "300 oro + Favor real", Progreso = 30, Nivel = 8 },
                new { Titulo = "Entrega Urgente", Descripcion = "Lleva el paquete al comerciante en Ciudad Capital antes del anochecer", Recompensa = "100 oro", Progreso = 80, Nivel = 3 }
            };

                    var misionesCompletadas = new List<dynamic>
            {
                new { Titulo = "Salvar al Aldeano", Descripcion = "Rescataste al aldeano secuestrado por bandidos", Recompensa = "150 oro" },
                new { Titulo = "Recolectar Hierbas", Descripcion = "Recolectaste 20 hierbas medicinales para el curandero", Recompensa = "80 oro + Poción de curación" }
            };

                    var misionesFallidas = new List<dynamic>
            {
                new { Titulo = "Proteger la Caravana", Descripcion = "La caravana fue destruida por orcos antes de llegar", Razon = "Tiempo agotado" }
            };

                    var misionesDisponibles = new List<dynamic>
            {
                new { Titulo = "Limpiar la Cueva", Descripcion = "Elimina a las arañas gigantes de la cueva al norte", Recompensa = "200 oro", Nivel = 5 },
                new { Titulo = "Investigar Rumores", Descripcion = "Investiga los extraños sucesos en el cementerio abandonado", Recompensa = "250 oro + Amuleto mágico", Nivel = 10 },
                new { Titulo = "Torneo de Combate", Descripcion = "Participa en el torneo anual de la ciudad y demuestra tu valía", Recompensa = "1000 oro + Título de Campeón", Nivel = 20 }
            };

            // FUNCIONES PARA MOSTRAR CADA CATEGORIA
            void MostrarMisionesActivas()
            {
                panelMisiones.Controls.Clear();

                if (misionesActivas.Count == 0)
                {
                    MostrarMensajeVacio("No tienes misiones activas", "Explora el mundo para encontrar nuevas aventuras");
                    return;
                }

                int y = 15;
                foreach (var mision in misionesActivas)
                {
                    Panel cardMision = new Panel
                    {
                        Size = new Size(880, 130),
                        Location = new Point(15, y),
                        BackColor = Color.FromArgb(180, 25, 35, 30)
                    };
                    RedondearMenu(cardMision, 10);

                    // BARRA LATERAL DE COLOR
                    Panel barraColor = new Panel
                    {
                        Size = new Size(6, 130),
                        Location = new Point(0, 0),
                        BackColor = Color.FromArgb(50, 180, 80)
                    };
                    cardMision.Controls.Add(barraColor);

                    // NIVEL REQUERIDO
                    Label lblNivel = new Label
                    {
                        Text = $"NIV {mision.Nivel}",
                        Font = FUENTE.ObtenerFont(12),
                        ForeColor = Color.Gold,
                        BackColor = Color.FromArgb(150, 80, 60, 20),
                        AutoSize = false,
                        Size = new Size(70, 25),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(790, 10)
                    };
                    cardMision.Controls.Add(lblNivel);

                    // TITULO
                    Label lblTituloMision = new Label
                    {
                        Text = mision.Titulo,
                        Font = FUENTE.ObtenerFont(18),
                        ForeColor = Color.White,
                        AutoSize = true,
                        Location = new Point(20, 15)
                    };
                    cardMision.Controls.Add(lblTituloMision);

                    // DESCRIPCION
                    Label lblDesc = new Label
                    {
                        Text = mision.Descripcion,
                        Font = FUENTE.ObtenerFont(13),
                        ForeColor = Color.LightGray,
                        AutoSize = false,
                        Size = new Size(750, 40),
                        Location = new Point(20, 42)
                    };
                    cardMision.Controls.Add(lblDesc);

                    // RECOMPENSA
                    Label lblRecompensa = new Label
                    {
                        Text = $"Recompensa: {mision.Recompensa}",
                        Font = FUENTE.ObtenerFont(12),
                        ForeColor = Color.FromArgb(255, 215, 100),
                        AutoSize = true,
                        Location = new Point(20, 85)
                    };
                    cardMision.Controls.Add(lblRecompensa);

                    // BARRA DE PROGRESO
                    Panel barraFondo = new Panel
                    {
                        Size = new Size(300, 20),
                        Location = new Point(560, 85),
                        BackColor = Color.FromArgb(100, 20, 20, 20)
                    };
                    RedondearMenu(barraFondo, 10);
                    cardMision.Controls.Add(barraFondo);

                    Panel barraProgreso = new Panel
                    {
                        Size = new Size((int)(300 * (mision.Progreso / 100.0)), 20),
                        Location = new Point(0, 0),
                        BackColor = Color.FromArgb(50, 180, 80)
                    };
                    RedondearMenu(barraProgreso, 10);
                    barraFondo.Controls.Add(barraProgreso);

                    Label lblProgreso = new Label
                    {
                        Text = $"{mision.Progreso}%",
                        Font = FUENTE.ObtenerFont(11),
                        ForeColor = Color.White,
                        AutoSize = false,
                        Size = new Size(300, 20),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(0, 0),
                        BackColor = Color.Transparent
                    };
                    barraFondo.Controls.Add(lblProgreso);
                    lblProgreso.BringToFront();

                    panelMisiones.Controls.Add(cardMision);
                    y += 145;
                }
            }

            void MostrarMisionesCompletadas()
            {
                panelMisiones.Controls.Clear();

                if (misionesCompletadas.Count == 0)
                {
                    MostrarMensajeVacio("Aún no has completado ninguna misión", "Las misiones completadas aparecerán aquí");
                    return;
                }

                int y = 15;
                foreach (var mision in misionesCompletadas)
                {
                    Panel cardMision = new Panel
                    {
                        Size = new Size(880, 100),
                        Location = new Point(15, y),
                        BackColor = Color.FromArgb(180, 20, 30, 40)
                    };
                    RedondearMenu(cardMision, 10);

                    // BARRA LATERAL
                    Panel barraColor = new Panel
                    {
                        Size = new Size(6, 100),
                        Location = new Point(0, 0),
                        BackColor = Color.FromArgb(100, 150, 220)
                    };
                    cardMision.Controls.Add(barraColor);

                    // ICONO DE COMPLETADO
                    Label icono = new Label
                    {
                        Text = "✓",
                        Font = FUENTE.ObtenerFont(32),
                        ForeColor = Color.LimeGreen,
                        Size = new Size(50, 50),
                        Location = new Point(15, 25),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    cardMision.Controls.Add(icono);

                    // TITULO
                    Label lblTituloMision = new Label
                    {
                        Text = mision.Titulo,
                        Font = FUENTE.ObtenerFont(18),
                        ForeColor = Color.White,
                        AutoSize = true,
                        Location = new Point(75, 15)
                    };
                    cardMision.Controls.Add(lblTituloMision);

                    // DESCRIPCION
                    Label lblDesc = new Label
                    {
                        Text = mision.Descripcion,
                        Font = FUENTE.ObtenerFont(13),
                        ForeColor = Color.LightGray,
                        AutoSize = false,
                        Size = new Size(550, 30),
                        Location = new Point(75, 45)
                    };
                    cardMision.Controls.Add(lblDesc);

                    // RECOMPENSA OBTENIDA
                    Label lblRecompensa = new Label
                    {
                        Text = $"Obtenido: {mision.Recompensa}",
                        Font = FUENTE.ObtenerFont(12),
                        ForeColor = Color.Gold,
                        AutoSize = true,
                        Location = new Point(650, 40)
                    };
                    cardMision.Controls.Add(lblRecompensa);

                    panelMisiones.Controls.Add(cardMision);
                    y += 115;
                }
            }

            void MostrarMisionesFallidas()
            {
                panelMisiones.Controls.Clear();

                if (misionesFallidas.Count == 0)
                {
                    MostrarMensajeVacio("No has fallado ninguna misión", "Mantén el buen trabajo, aventurero");
                    return;
                }

                int y = 15;
                foreach (var mision in misionesFallidas)
                {
                    Panel cardMision = new Panel
                    {
                        Size = new Size(880, 100),
                        Location = new Point(15, y),
                        BackColor = Color.FromArgb(180, 40, 20, 20)
                    };
                    RedondearMenu(cardMision, 10);

                    // BARRA LATERAL
                    Panel barraColor = new Panel
                    {
                        Size = new Size(6, 100),
                        Location = new Point(0, 0),
                        BackColor = Color.FromArgb(200, 60, 60)
                    };
                    cardMision.Controls.Add(barraColor);

                    // ICONO DE FALLIDO
                    Label icono = new Label
                    {
                        Text = "✗",
                        Font = FUENTE.ObtenerFont(32),
                        ForeColor = Color.OrangeRed,
                        Size = new Size(50, 50),
                        Location = new Point(15, 25),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    cardMision.Controls.Add(icono);

                    // TITULO
                    Label lblTituloMision = new Label
                    {
                        Text = mision.Titulo,
                        Font = FUENTE.ObtenerFont(18),
                        ForeColor = Color.LightGray,
                        AutoSize = true,
                        Location = new Point(75, 15)
                    };
                    cardMision.Controls.Add(lblTituloMision);

                    // DESCRIPCION
                    Label lblDesc = new Label
                    {
                        Text = mision.Descripcion,
                        Font = FUENTE.ObtenerFont(13),
                        ForeColor = Color.Gray,
                        AutoSize = false,
                        Size = new Size(550, 30),
                        Location = new Point(75, 45)
                    };
                    cardMision.Controls.Add(lblDesc);

                    // RAZON DEL FALLO
                    Label lblRazon = new Label
                    {
                        Text = $"Razón: {mision.Razon}",
                        Font = FUENTE.ObtenerFont(12),
                        ForeColor = Color.FromArgb(255, 150, 150),
                        AutoSize = true,
                        Location = new Point(650, 40)
                    };
                    cardMision.Controls.Add(lblRazon);

                    panelMisiones.Controls.Add(cardMision);
                    y += 115;
                }
            }

            void MostrarMisionesDisponibles()
            {
                panelMisiones.Controls.Clear();

                if (misionesDisponibles.Count == 0)
                {
                    MostrarMensajeVacio("No hay misiones disponibles", "Vuelve más tarde o explora nuevas áreas");
                    return;
                }

                int y = 15;
                foreach (var mision in misionesDisponibles)
                {
                    Panel cardMision = new Panel
                    {
                        Size = new Size(880, 120),
                        Location = new Point(15, y),
                        BackColor = Color.FromArgb(180, 35, 30, 20)
                    };
                    RedondearMenu(cardMision, 10);

                    // BARRA LATERAL
                    Panel barraColor = new Panel
                    {
                        Size = new Size(6, 120),
                        Location = new Point(0, 0),
                        BackColor = Color.FromArgb(220, 170, 50)
                    };
                    cardMision.Controls.Add(barraColor);

                    // NIVEL REQUERIDO
                    Label lblNivel = new Label
                    {
                        Text = $"NIV {mision.Nivel}",
                        Font = FUENTE.ObtenerFont(12),
                        ForeColor = Color.White,
                        BackColor = Color.FromArgb(150, 80, 60, 20),
                        AutoSize = false,
                        Size = new Size(70, 25),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(790, 10)
                    };
                    cardMision.Controls.Add(lblNivel);

                    // TITULO
                    Label lblTituloMision = new Label
                    {
                        Text = mision.Titulo,
                        Font = FUENTE.ObtenerFont(18),
                        ForeColor = Color.FromArgb(255, 220, 120),
                        AutoSize = true,
                        Location = new Point(20, 15)
                    };
                    cardMision.Controls.Add(lblTituloMision);

                    // DESCRIPCION
                    Label lblDesc = new Label
                    {
                        Text = mision.Descripcion,
                        Font = FUENTE.ObtenerFont(13),
                        ForeColor = Color.LightGray,
                        AutoSize = false,
                        Size = new Size(750, 40),
                        Location = new Point(20, 42)
                    };
                    cardMision.Controls.Add(lblDesc);

                    // RECOMPENSA
                    Label lblRecompensa = new Label
                    {
                        Text = $"Recompensa: {mision.Recompensa}",
                        Font = FUENTE.ObtenerFont(12),
                        ForeColor = Color.FromArgb(255, 215, 100),
                        AutoSize = true,
                        Location = new Point(20, 85)
                    };
                    cardMision.Controls.Add(lblRecompensa);

                    // BOTON ACEPTAR
                    Button btnAceptar = new Button
                    {
                        Text = "ACEPTAR MISIÓN",
                        Font = FUENTE.ObtenerFont(12),
                        Size = new Size(160, 30),
                        Location = new Point(690, 80),
                        BackColor = Color.FromArgb(220, 170, 50),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand
                    };
                    btnAceptar.FlatAppearance.BorderSize = 0;
                    RedondearBoton(btnAceptar, 8);
                    btnAceptar.Click += (s, e) => MessageBox.Show($"Misión '{mision.Titulo}' aceptada!");
                    cardMision.Controls.Add(btnAceptar);

                    panelMisiones.Controls.Add(cardMision);
                    y += 135;
                }
            }

            void MostrarMensajeVacio(string mensaje, string submensaje)
            {
                Label lblVacio = new Label
                {
                    Text = mensaje,
                    Font = FUENTE.ObtenerFont(18),
                    ForeColor = Color.Gray,
                    AutoSize = false,
                    Size = new Size(880, 50),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(15, 180)
                };
                panelMisiones.Controls.Add(lblVacio);

                Label lblSub = new Label
                {
                    Text = submensaje,
                    Font = FUENTE.ObtenerFont(14),
                    ForeColor = Color.DarkGray,
                    AutoSize = false,
                    Size = new Size(880, 30),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(15, 230)
                };
                panelMisiones.Controls.Add(lblSub);
            }

            // EVENTOS DE PESTAÑAS
            void ActivarPestaña(Button activo, Button[] otros)
            {
                activo.BackColor = activo.Tag as Color? ?? Color.Gray;
                foreach (var otro in otros)
                {
                    otro.BackColor = Color.FromArgb(100, 30, 30, 30);
                }
            }

            btnActivas.Click += (s, e) =>
            {
                ActivarPestaña(btnActivas, new[] { btnCompletadas, btnFallidas, btnDisponibles });
                MostrarMisionesActivas();
            };

            btnCompletadas.Click += (s, e) =>
            {
                ActivarPestaña(btnCompletadas, new[] { btnActivas, btnFallidas, btnDisponibles });
                MostrarMisionesCompletadas();
            };

            btnFallidas.Click += (s, e) =>
            {
                ActivarPestaña(btnFallidas, new[] { btnActivas, btnCompletadas, btnDisponibles });
                MostrarMisionesFallidas();
            };

            btnDisponibles.Click += (s, e) =>
            {
                ActivarPestaña(btnDisponibles, new[] { btnActivas, btnCompletadas, btnFallidas });
                MostrarMisionesDisponibles();
            };

            // MOSTRAR ACTIVAS POR DEFECTO
            btnActivas.BackColor = Color.FromArgb(50, 180, 80);
            MostrarMisionesActivas();

            // FLECHAS DEL CARRUSEL
            Button flechaIzq = new Button
            {
                Text = "<",
                Width = 50,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = FUENTE.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Location = new Point(10, (this.ClientSize.Height - 50) / 2)
            };
            flechaIzq.FlatAppearance.BorderSize = 0;
            flechaIzq.FlatAppearance.MouseOverBackColor = Color.Transparent;
            flechaIzq.FlatAppearance.MouseDownBackColor = Color.Transparent;
            flechaIzq.MouseEnter += (s, e) => flechaIzq.ForeColor = Color.DarkGray;
            flechaIzq.MouseLeave += (s, e) => flechaIzq.ForeColor = Color.White;
            flechaIzq.Click += (s, e) => Navegar_FlechaIzquierda();
            overlay.Controls.Add(flechaIzq);

            Button flechaDer = new Button
            {
                Text = ">",
                Width = 50,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = FUENTE.ObtenerFont(30),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Location = new Point(this.ClientSize.Width - 50 - 10, (this.ClientSize.Height - 50) / 2)
            };
            flechaDer.FlatAppearance.BorderSize = 0;
            flechaDer.FlatAppearance.MouseOverBackColor = Color.Transparent;
            flechaDer.FlatAppearance.MouseDownBackColor = Color.Transparent;
            flechaDer.MouseEnter += (s, e) => flechaDer.ForeColor = Color.DarkGray;
            flechaDer.MouseLeave += (s, e) => flechaDer.ForeColor = Color.White;
            flechaDer.Click += (s, e) => Navegar_FlechaDerecha();
            overlay.Controls.Add(flechaDer);

            this.Controls.Add(panelMenuPausa);
            panelMenuPausa.BringToFront();
        }

        private Button CrearBotonPestañaMision(string texto, Color colorActivo)
        {
            Button btn = new Button
            {
                Text = texto,
                Width = 220,
                Height = 45,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(100, 30, 30, 30),
                ForeColor = Color.White,
                Font = FUENTE.ObtenerFont(14),
                Cursor = Cursors.Hand,
                Margin = new Padding(2),
                Tag = colorActivo
            };
            btn.FlatAppearance.BorderSize = 0;
            RedondearBoton(btn, 10);

            btn.MouseEnter += (s, e) =>
            {
                if (btn.BackColor == Color.FromArgb(100, 30, 30, 30))
                    btn.BackColor = Color.FromArgb(120, 50, 50, 50);
            };
            btn.MouseLeave += (s, e) =>
            {
                if (btn.BackColor == Color.FromArgb(120, 50, 50, 50))
                    btn.BackColor = Color.FromArgb(100, 30, 30, 30);
            };

            return btn;
        }

    }

    public static class Extensiones
    {
        public static void DoubleBuffered(this Control c, bool valor)
        {
            System.Reflection.PropertyInfo aProp =
                  typeof(Control).GetProperty("DoubleBuffered",
                  System.Reflection.BindingFlags.NonPublic |
                  System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, valor, null);
        }
    }

}
