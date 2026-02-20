using PROYECTO_5TO___TOTЯ;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.SQLite;

namespace proyecto
{
    public class RESUMEN_PERSONAJE : Form
    {
        private Personaje pj;
        private TextBox txtNombrePartida;
        private Button btnGuardar, btnCancelar;
        private PictureBox picPersonaje;

        public RESUMEN_PERSONAJE(Personaje personaje)
        {
            pj = personaje;
            FUENTE.CargarFuente();
            InicializarFormulario();
            CrearControles();
        }

        private void InicializarFormulario()
        {
            Text = "Resumen del Personaje";
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
            BackColor = Color.FromArgb(18, 18, 22);

            string rutaFondo = Path.Combine(Application.StartupPath, "Resources", "fondoblur.png");
            if (File.Exists(rutaFondo))
            {
                BackgroundImage = Image.FromFile(rutaFondo);
                BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void CrearControles()
        {
            // BOTÓN CERRAR
            btnCancelar = new Button()
            {
                Text = "X",
                Font = FUENTE.ObtenerFont(18),
                Size = new Size(45, 45),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnCancelar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnCancelar.MouseEnter += (s, e) => { btnCancelar.ForeColor = Color.Red; };
            btnCancelar.MouseLeave += (s, e) => { btnCancelar.ForeColor = Color.White; };
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCancelar);
            btnCancelar.BringToFront();

            // TÍTULO PRINCIPAL
            Label lblTitulo = new Label()
            {
                Text = "RESUMEN DEL PERSONAJE",
                Font = FUENTE.ObtenerFont(42),
                ForeColor = Color.FromArgb(120, 200, 255),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Size = new Size(900, 60)
            };
            Controls.Add(lblTitulo);
            lblTitulo.Left = (this.ClientSize.Width - lblTitulo.Width) / 2;
            lblTitulo.Top = 30;
            lblTitulo.BringToFront();

            this.Resize += (s, e) =>
            {
                lblTitulo.Left = (this.ClientSize.Width - lblTitulo.Width) / 2;
            };

            // SUBTÍTULO
            Label lblSubtitulo = new Label()
            {
                Text = "Revisa tu personaje antes de guardar la partida",
                Font = FUENTE.ObtenerFont(14),
                ForeColor = Color.LightGray,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Size = new Size(700, 30)
            };
            Controls.Add(lblSubtitulo);
            lblSubtitulo.Left = (this.ClientSize.Width - lblSubtitulo.Width) / 2;
            lblSubtitulo.Top = 95;
            lblSubtitulo.BringToFront();

            this.Resize += (s, e) =>
            {
                lblSubtitulo.Left = (this.ClientSize.Width - lblSubtitulo.Width) / 2;
            };

            // CONTENEDOR PRINCIPAL
            Panel contenedorPrincipal = new Panel
            {
                Size = new Size(1400, 720),
                BackColor = Color.Transparent
            };
            Controls.Add(contenedorPrincipal);
            contenedorPrincipal.Left = (this.ClientSize.Width - contenedorPrincipal.Width) / 2;
            contenedorPrincipal.Top = (this.ClientSize.Height - contenedorPrincipal.Height) / 2 + 40;

            this.Resize += (s, e) =>
            {
                contenedorPrincipal.Left = (this.ClientSize.Width - contenedorPrincipal.Width) / 2;
                contenedorPrincipal.Top = (this.ClientSize.Height - contenedorPrincipal.Height) / 2 + 40;
            };

            // PANEL IZQUIERDO - PERSONAJE
            Panel panelPersonaje = new Panel
            {
                Size = new Size(300, 720),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(220, 28, 30, 35)
            };
            contenedorPrincipal.Controls.Add(panelPersonaje);
            RedondearMenu(panelPersonaje, 20);

            // IMAGEN DEL PERSONAJE
            picPersonaje = new PictureBox
            {
                Size = new Size(200, 300),
                Location = new Point(50, 30),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(35, 38, 45)
            };
            panelPersonaje.Controls.Add(picPersonaje);
            RedondearMenu(picPersonaje, 15);
            CargarImagenPersonaje();

            // NOMBRE DEL PERSONAJE
            Label lblNombrePersonaje = new Label
            {
                Text = "NUEVO PERSONAJE",
                Font = FUENTE.ObtenerFont(20),
                ForeColor = Color.Gold,
                AutoSize = false,
                Size = new Size(280, 35),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(10, 345)
            };
            panelPersonaje.Controls.Add(lblNombrePersonaje);

            // INFO BÁSICA
            Label lblInfoBasica = new Label
            {
                Text = $"{pj.RAZA} {pj.SUBRAZA}\n{pj.CLASE}\n{pj.TRASFONDO}\n{pj.ALINEAMIENTO}",
                Font = FUENTE.ObtenerFont(14),
                ForeColor = Color.LightGray,
                AutoSize = false,
                Size = new Size(280, 100),
                TextAlign = ContentAlignment.TopCenter,
                Location = new Point(10, 385)
            };
            panelPersonaje.Controls.Add(lblInfoBasica);

            // ESTADÍSTICAS PRINCIPALES
            Panel panelStatsPrincipales = new Panel
            {
                Size = new Size(280, 100),
                Location = new Point(10, 500),
                BackColor = Color.FromArgb(150, 10, 10, 15)
            };
            panelPersonaje.Controls.Add(panelStatsPrincipales);
            RedondearMenu(panelStatsPrincipales, 10);

            string[] statsLabels = { "HP", "CA", "VEL", "INI" };
            int[] statsValues = { pj.HP, pj.CA, pj.VEL, pj.INI };

            for (int i = 0; i < 4; i++)
            {
                int x = (i % 2) * 140;
                int y = (i / 2) * 50;

                Label lblStat = new Label
                {
                    Text = $"{statsLabels[i]}: {statsValues[i]}",
                    Font = FUENTE.ObtenerFont(16),
                    ForeColor = Color.White,
                    AutoSize = false,
                    Size = new Size(130, 40),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Location = new Point(10 + x, 5 + y)
                };
                panelStatsPrincipales.Controls.Add(lblStat);
            }

            // PANEL DERECHO - DETALLES
            Panel panelDetalles = new Panel
            {
                Size = new Size(1080, 720),
                Location = new Point(320, 0),
                BackColor = Color.FromArgb(220, 28, 30, 35)
            };
            contenedorPrincipal.Controls.Add(panelDetalles);
            RedondearMenu(panelDetalles, 20);

            // PESTAÑAS
            FlowLayoutPanel panelPestañas = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Size = new Size(1060, 50),
                Location = new Point(10, 10),
                BackColor = Color.Transparent,
                WrapContents = false
            };
            panelDetalles.Controls.Add(panelPestañas);

            // PANEL DE CONTENIDO
            Panel panelContenido = new Panel
            {
                Size = new Size(1060, 510),
                Location = new Point(10, 70),
                BackColor = Color.FromArgb(200, 10, 10, 15),
                AutoScroll = true
            };
            panelDetalles.Controls.Add(panelContenido);
            RedondearMenu(panelContenido, 15);

            // BOTONES DE PESTAÑAS
            Button btnAtributos = CrearBotonPestaña("ATRIBUTOS", Color.FromArgb(255, 180, 100));
            Button btnHabilidades = CrearBotonPestaña("HABILIDADES", Color.DodgerBlue);
            Button btnEquipamiento = CrearBotonPestaña("EQUIPAMIENTO", Color.FromArgb(255, 100, 100));

            panelPestañas.Controls.Add(btnAtributos);
            panelPestañas.Controls.Add(btnHabilidades);
            panelPestañas.Controls.Add(btnEquipamiento);

            // FUNCIONES PARA MOSTRAR CONTENIDO
            void MostrarAtributos()
            {
                panelContenido.Controls.Clear();

                Label lblTituloAtrib = new Label
                {
                    Text = "ATRIBUTOS DEL PERSONAJE",
                    Font = FUENTE.ObtenerFont(22),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(20, 20)
                };
                panelContenido.Controls.Add(lblTituloAtrib);

                string[] nombres = { "STR", "DEX", "CON", "INT", "WIS", "CHA" };
                int[] valores = { pj.STR, pj.DEX, pj.CON, pj.INT, pj.WIS, pj.CHA };

                for (int i = 0; i < 6; i++)
                {
                    int col = i % 3;
                    int row = i / 3;
                    Panel cajaAtrib = CrearCajaAtributo(nombres[i], valores[i]);
                    cajaAtrib.Location = new Point(20 + col * 345, 80 + row * 150);
                    panelContenido.Controls.Add(cajaAtrib);
                }
            }

            void MostrarHabilidades()
            {
                panelContenido.Controls.Clear();

                Label lblTituloHab = new Label
                {
                    Text = "HABILIDADES Y COMPETENCIAS",
                    Font = FUENTE.ObtenerFont(22),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Location = new Point(20, 20)
                };
                panelContenido.Controls.Add(lblTituloHab);

                int y = 80;
                foreach (var hab in pj.HABILIDADES)
                {
                    Panel habPanel = new Panel
                    {
                        Size = new Size(1020, 70),
                        Location = new Point(20, y),
                        BackColor = Color.FromArgb(150, 20, 30, 50)
                    };
                    RedondearMenu(habPanel, 10);

                    Label lblNombre = new Label
                    {
                        Text = hab.NOMBRE,
                        Font = FUENTE.ObtenerFont(18),
                        ForeColor = Color.Cyan,
                        AutoSize = true,
                        Location = new Point(15, 12)
                    };
                    habPanel.Controls.Add(lblNombre);

                    Label lblDetalles = new Label
                    {
                        Text = $"({hab.STAT_ASOCIADO}) - Modificador: {hab.MODIFICADOR_STAT:+0;-#} | Competencia: {hab.BONIFICADOR_COMPETENCIA:+0;-#}",
                        Font = FUENTE.ObtenerFont(13),
                        ForeColor = Color.LightGray,
                        AutoSize = true,
                        Location = new Point(15, 38)
                    };
                    habPanel.Controls.Add(lblDetalles);

                    Label lblTotal = new Label
                    {
                        Text = hab.TOTAL.ToString("+0;-#"),
                        Font = FUENTE.ObtenerFont(28),
                        ForeColor = Color.LimeGreen,
                        AutoSize = true,
                        Location = new Point(930, 15)
                    };
                    habPanel.Controls.Add(lblTotal);

                    panelContenido.Controls.Add(habPanel);
                    y += 85;
                }
            }

            void MostrarEquipamiento()
            {
                panelContenido.Controls.Clear();

                // ARMAS
                Label lblTituloArmas = new Label
                {
                    Text = "⚔ ARMAS",
                    Font = FUENTE.ObtenerFont(20),
                    ForeColor = Color.OrangeRed,
                    AutoSize = true,
                    Location = new Point(20, 20)
                };
                panelContenido.Controls.Add(lblTituloArmas);

                int yArmas = 60;
                if (pj.ARMAS.Count == 0)
                {
                    Label lblNoArmas = new Label
                    {
                        Text = "Sin armas equipadas",
                        Font = FUENTE.ObtenerFont(14),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        Location = new Point(40, yArmas)
                    };
                    panelContenido.Controls.Add(lblNoArmas);
                    yArmas += 40;
                }
                else
                {
                    foreach (var arma in pj.ARMAS)
                    {
                        Label lblArma = new Label
                        {
                            Text = $"• {arma}",
                            Font = FUENTE.ObtenerFont(16),
                            ForeColor = Color.White,
                            AutoSize = true,
                            Location = new Point(40, yArmas)
                        };
                        panelContenido.Controls.Add(lblArma);
                        yArmas += 35;
                    }
                }

                // HECHIZOS
                Label lblTituloHechizos = new Label
                {
                    Text = "✨ HECHIZOS",
                    Font = FUENTE.ObtenerFont(20),
                    ForeColor = Color.MediumPurple,
                    AutoSize = true,
                    Location = new Point(20, yArmas + 20)
                };
                panelContenido.Controls.Add(lblTituloHechizos);

                int yHechizos = yArmas + 60;
                if (pj.HECHIZOS.Count == 0)
                {
                    Label lblNoHechizos = new Label
                    {
                        Text = "Sin hechizos conocidos",
                        Font = FUENTE.ObtenerFont(14),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        Location = new Point(40, yHechizos)
                    };
                    panelContenido.Controls.Add(lblNoHechizos);
                }
                else
                {
                    foreach (var hechizo in pj.HECHIZOS)
                    {
                        Label lblHechizo = new Label
                        {
                            Text = $"• {hechizo}",
                            Font = FUENTE.ObtenerFont(16),
                            ForeColor = Color.White,
                            AutoSize = true,
                            Location = new Point(40, yHechizos)
                        };
                        panelContenido.Controls.Add(lblHechizo);
                        yHechizos += 35;
                    }
                }
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

            btnAtributos.Click += (s, e) =>
            {
                ActivarPestaña(btnAtributos, new[] { btnHabilidades, btnEquipamiento });
                MostrarAtributos();
            };

            btnHabilidades.Click += (s, e) =>
            {
                ActivarPestaña(btnHabilidades, new[] { btnAtributos, btnEquipamiento });
                MostrarHabilidades();
            };

            btnEquipamiento.Click += (s, e) =>
            {
                ActivarPestaña(btnEquipamiento, new[] { btnAtributos, btnHabilidades });
                MostrarEquipamiento();
            };

            // MOSTRAR ATRIBUTOS POR DEFECTO
            btnAtributos.BackColor = Color.FromArgb(255, 180, 100);
            MostrarAtributos();

            // SECCIÓN INFERIOR - GUARDAR
            Panel panelGuardar = new Panel
            {
                Size = new Size(1060, 120),
                Location = new Point(10, 590),
                BackColor = Color.FromArgb(180, 15, 15, 20)
            };
            panelDetalles.Controls.Add(panelGuardar);
            RedondearMenu(panelGuardar, 15);

            Label lblNombrePartida = new Label()
            {
                Text = "NOMBRE DE LA PARTIDA:",
                Font = FUENTE.ObtenerFont(18),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            panelGuardar.Controls.Add(lblNombrePartida);

            txtNombrePartida = new TextBox()
            {
                Font = FUENTE.ObtenerFont(16),
                Size = new Size(500, 35),
                Location = new Point(20, 55),
                BackColor = Color.FromArgb(35, 38, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panelGuardar.Controls.Add(txtNombrePartida);

            btnGuardar = new Button()
            {
                Text = "GUARDAR PARTIDA",
                Font = FUENTE.ObtenerFont(18),
                Size = new Size(500, 50),
                Location = new Point(540, 45),
                BackColor = Color.FromArgb(50, 180, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 200, 90);
            RedondearBoton(btnGuardar, 12);
            btnGuardar.Click += BtnGuardar_Click;
            panelGuardar.Controls.Add(btnGuardar);
        }

        private Button CrearBotonPestaña(string texto, Color colorActivo)
        {
            Button btn = new Button
            {
                Text = texto,
                Width = 345,
                Height = 45,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(100, 30, 30, 30),
                ForeColor = Color.White,
                Font = FUENTE.ObtenerFont(16),
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

        private Panel CrearCajaAtributo(string nombre, int valor)
        {
            Panel caja = new Panel
            {
                Size = new Size(320, 130),
                BackColor = Color.FromArgb(150, 35, 38, 45)
            };
            RedondearMenu(caja, 12);

            Label lblNombre = new Label
            {
                Text = nombre,
                Font = FUENTE.ObtenerFont(16),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(20, 15)
            };
            caja.Controls.Add(lblNombre);

            Label lblValor = new Label
            {
                Text = valor.ToString(),
                Font = FUENTE.ObtenerFont(48),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 45)
            };
            caja.Controls.Add(lblValor);

            int mod = (valor - 10) / 2;
            Label lblMod = new Label
            {
                Text = (mod >= 0 ? "+" : "") + mod.ToString(),
                Font = FUENTE.ObtenerFont(32),
                ForeColor = mod >= 0 ? Color.LimeGreen : Color.OrangeRed,
                AutoSize = true,
                Location = new Point(230, 55)
            };
            caja.Controls.Add(lblMod);

            return caja;
        }

        private void CargarImagenPersonaje()
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

            try
            {
                if (picPersonaje.Image != null)
                {
                    picPersonaje.Image.Dispose();
                }
                picPersonaje.Image = Image.FromFile(ruta);
            }
            catch { }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombrePartida.Text))
            {
                MessageBox.Show("Debes escribir un nombre de partida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                pj.NOMBRE = txtNombrePartida.Text.Trim();
                // Ruta de la base de datos
                BASE_DE_DATOS.GuardarPersonaje(pj);
                

                // Mensaje opcional
                MessageBox.Show("Partida guardada correctamente", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Ir a pantalla principal
                this.DialogResult = DialogResult.OK;
                this.Close();


            }

            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void RedondearMenu(Control control, int radio)
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
    }
}