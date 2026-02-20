using PROYECTO_5TO___TOTЯ;
using System.ComponentModel;
using System.Data.SQLite;

namespace proyecto
{
    public class PANTALLA_CARGAR : Form
    {
        private ListBox lstPersonajes;
        private Button btnCargar, btnCancelar, btnEliminar;
        private Panel panelInfo;
        private Label lblInfo;
        private Image fondo;
        private Label lblTitulo = null!;
        private PictureBox picPersonajePreview;
        private Panel panelStatsPreview;
        private Label lblSelecciona;
        private Panel contenedorPreview;

        public PANTALLA_CARGAR()
        {
            FUENTE.CargarFuente();
            InicializarFormulario();
            CrearControles();
            CargarPersonajesDesdeBD();
        }

        private void InicializarFormulario()
        {
            Text = "Cargar Partida";
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
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 50,
                Height = 50
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnCancelar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnCancelar.MouseEnter += (s, e) => { btnCancelar.ForeColor = Color.Red; };
            btnCancelar.MouseLeave += (s, e) => { btnCancelar.ForeColor = Color.White; };
            RedondearBoton(btnCancelar, 30);
            btnCancelar.Click += (s, e) => { var menu = new MENU_PRINCIPAL(); menu.Show(); this.Close(); };
            Controls.Add(btnCancelar);
            btnCancelar.BringToFront();
            btnCancelar.Left = 20;
            btnCancelar.Top = 20;

            // TÍTULO
            lblTitulo = new Label()
            {
                Text = "CARGAR PARTIDA",
                Font = FUENTE.ObtenerFont(42),
                ForeColor = Color.FromArgb(120, 200, 255),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };
            Controls.Add(lblTitulo);

            int tituloWidth = 900;
            int tituloHeight = 60;
            lblTitulo.Size = new Size(tituloWidth, tituloHeight);
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
                Text = "Selecciona una partida guardada para continuar tu aventura",
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
                Size = new Size(1400, 700),
                BackColor = Color.Transparent
            };
            Controls.Add(contenedorPrincipal);
            contenedorPrincipal.Anchor = AnchorStyles.None;
            contenedorPrincipal.Left = (this.ClientSize.Width - contenedorPrincipal.Width) / 2;
            contenedorPrincipal.Top = (this.ClientSize.Height - contenedorPrincipal.Height) / 2 + 40;

            this.Resize += (s, e) =>
            {
                contenedorPrincipal.Left = (this.ClientSize.Width - contenedorPrincipal.Width) / 2;
                contenedorPrincipal.Top = (this.ClientSize.Height - contenedorPrincipal.Height) / 2 + 40;
            };

            // PANEL IZQUIERDO - LISTA DE PARTIDAS
            Panel panelLista = new Panel
            {
                Size = new Size(450, 700),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(220, 28, 30, 35)
            };
            contenedorPrincipal.Controls.Add(panelLista);
            RedondearMenu(panelLista, 20);

            Label lblListaTitulo = new Label
            {
                Text = "PARTIDAS GUARDADAS",
                Font = FUENTE.ObtenerFont(18),
                ForeColor = Color.FromArgb(255, 180, 100),
                AutoSize = false,
                Size = new Size(430, 40),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(20, 15)
            };
            panelLista.Controls.Add(lblListaTitulo);

            // LISTA DE PERSONAJES MEJORADA
            lstPersonajes = new ListBox
            {
                Font = FUENTE.ObtenerFont(18),
                Size = new Size(410, 550),
                Location = new Point(20, 65),
                IntegralHeight = false,
                BackColor = Color.FromArgb(35, 38, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 60
            };
            lstPersonajes.DrawItem += LstPersonajes_DrawItem;
            lstPersonajes.SelectedIndexChanged += LstPersonajes_SelectedIndexChanged;
            panelLista.Controls.Add(lstPersonajes);

            // BOTONES DE ACCIÓN
            btnCargar = new Button
            {
                Text = "CARGAR",
                Font = FUENTE.ObtenerFont(16),
                Size = new Size(195, 50),
                Location = new Point(20, 630),
                BackColor = Color.FromArgb(50, 180, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCargar.FlatAppearance.BorderSize = 0;
            btnCargar.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 200, 90);
            RedondearBoton(btnCargar, 12);
            btnCargar.Click += BtnCargar_Click;
            panelLista.Controls.Add(btnCargar);

            btnEliminar = new Button
            {
                Text = "ELIMINAR",
                Font = FUENTE.ObtenerFont(16),
                Size = new Size(195, 50),
                Location = new Point(235, 630),
                BackColor = Color.FromArgb(200, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 80, 80);
            RedondearBoton(btnEliminar, 12);
            btnEliminar.Click += BtnEliminar_Click;
            panelLista.Controls.Add(btnEliminar);

            // PANEL DERECHO - VISTA PREVIA
            Panel panelPreview = new Panel
            {
                Size = new Size(930, 700),
                Location = new Point(470, 0),
                BackColor = Color.FromArgb(220, 28, 30, 35)
            };
            contenedorPrincipal.Controls.Add(panelPreview);
            RedondearMenu(panelPreview, 20);

            Label lblPreviewTitulo = new Label
            {
                Text = "VISTA PREVIA",
                Font = FUENTE.ObtenerFont(18),
                ForeColor = Color.FromArgb(120, 200, 255),
                AutoSize = false,
                Size = new Size(890, 40),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(20, 15)
            };
            panelPreview.Controls.Add(lblPreviewTitulo);

            // MENSAJE CUANDO NO HAY SELECCIÓN
            lblSelecciona = new Label
            {
                Text = "Selecciona una partida para ver detalles",
                Font = FUENTE.ObtenerFont(20),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(890, 640),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 60)
            };
            panelPreview.Controls.Add(lblSelecciona);

            // CONTENEDOR DE PREVIEW (OCULTO INICIALMENTE)
            contenedorPreview = new Panel
            {
                Size = new Size(890, 640),
                Location = new Point(20, 60),
                BackColor = Color.Transparent,
                Visible = false
            };
            panelPreview.Controls.Add(contenedorPreview);

            // IMAGEN DEL PERSONAJE
            picPersonajePreview = new PictureBox
            {
                Size = new Size(200, 300),
                Location = new Point(20, 20),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(35, 38, 45)
            };
            RedondearMenu(picPersonajePreview, 15);
            contenedorPreview.Controls.Add(picPersonajePreview);

            // INFORMACIÓN PRINCIPAL
            lblInfo = new Label
            {
                Font = FUENTE.ObtenerFont(16),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(630, 300),
                Location = new Point(240, 20),
                TextAlign = ContentAlignment.TopLeft
            };
            contenedorPreview.Controls.Add(lblInfo);

            // PANEL DE ESTADÍSTICAS
            panelStatsPreview = new Panel
            {
                Size = new Size(850, 300),
                Location = new Point(20, 330),
                BackColor = Color.FromArgb(35, 38, 45)
            };
            RedondearMenu(panelStatsPreview, 15);
            contenedorPreview.Controls.Add(panelStatsPreview);
        }

        private void LstPersonajes_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color bgColor = isSelected ? Color.FromArgb(60, 120, 200) : Color.FromArgb(35, 38, 45);
            Color textColor = Color.White;

            using (Brush bgBrush = new SolidBrush(bgColor))
            {
                e.Graphics.FillRectangle(bgBrush, e.Bounds);
            }

            string nombre = lstPersonajes.Items[e.Index].ToString();
            using (Brush textBrush = new SolidBrush(textColor))
            {
                StringFormat sf = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near
                };
                Rectangle textRect = new Rectangle(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width - 15, e.Bounds.Height);
                e.Graphics.DrawString(nombre, e.Font, textBrush, textRect, sf);
            }

            e.DrawFocusRectangle();
        }



        private void CargarPersonajesDesdeBD()
        {
            lstPersonajes.Items.Clear();

            try
            {
                using (var conexion = new SQLiteConnection("Data Source=dnd_totr.sqlite;Version=3;"))
                {
                    conexion.Open();
                    string query = "SELECT NOMBRE FROM PERSONAJES ORDER BY ID DESC";
                    using (var cmd = new SQLiteCommand(query, conexion))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nombre = reader["NOMBRE"].ToString()!;
                            lstPersonajes.Items.Add(nombre);
                        }
                    }
                }

                if (lstPersonajes.Items.Count == 0)
                {
                    lstPersonajes.Items.Add("No hay partidas guardadas");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los personajes: " + ex.Message);
            }
        }

        private void LstPersonajes_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstPersonajes.SelectedIndex == -1) return;
            string nombre = lstPersonajes.SelectedItem.ToString()!;
            if (nombre == "No hay partidas guardadas") return;
            MostrarInfoPersonaje(nombre);
        }

        private void MostrarInfoPersonaje(string nombre)
        {
            try
            {
                using (var conexion = new SQLiteConnection("Data Source=dnd_totr.sqlite;Version=3;"))
                {
                    conexion.Open();
                    string query = "SELECT * FROM PERSONAJES WHERE NOMBRE=@nombre";
                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Personaje pj = new Personaje()
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    NOMBRE = reader["NOMBRE"].ToString()!,
                                    RAZA = reader["RAZA"].ToString()!,
                                    SUBRAZA = reader["SUBRAZA"].ToString()!,
                                    CLASE = reader["CLASE"].ToString()!,
                                    TRASFONDO = reader["TRASFONDO"].ToString()!,
                                    ALINEAMIENTO = reader["ALINEAMIENTO"].ToString()!,
                                    LVL = Convert.ToInt32(reader["LVL"]),
                                    STR = Convert.ToInt32(reader["STR"]),
                                    DEX = Convert.ToInt32(reader["DEX"]),
                                    CON = Convert.ToInt32(reader["CON"]),
                                    INT = Convert.ToInt32(reader["INTE"]),
                                    WIS = Convert.ToInt32(reader["WIS"]),
                                    CHA = Convert.ToInt32(reader["CHA"]),
                                    HP = Convert.ToInt32(reader["HP"]),
                                    CA = Convert.ToInt32(reader["CA"]),
                                    VEL = Convert.ToInt32(reader["VEL"]),
                                    INI = Convert.ToInt32(reader["INI"])
                                };

                                // MOSTRAR PANEL DE PREVIEW
                                lblSelecciona.Visible = false;
                                contenedorPreview.Visible = true;

                                // CARGAR IMAGEN
                                CargarImagenPersonaje(pj);

                                // INFORMACIÓN BÁSICA
                                lblInfo.Text = $"NOMBRE: {pj.NOMBRE}\n" +
                                              $"────────────────────────\n" +
                                              $"RAZA: {pj.RAZA} {pj.SUBRAZA}\n" +
                                              $"CLASE: {pj.CLASE}\n" +
                                              $"TRASFONDO: {pj.TRASFONDO}\n" +
                                              $"ALINEAMIENTO: {pj.ALINEAMIENTO}\n" +
                                              $"NIVEL: {pj.LVL}\n\n" +
                                              $"HP: {pj.HP}\n" +
                                              $"CA: {pj.CA}\n" +
                                              $"VEL: {pj.VEL}\n" +
                                              $"INI: {pj.INI}";

                                // MOSTRAR ATRIBUTOS
                                MostrarAtributos(pj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar info del personaje: " + ex.Message);
            }
        }

        private void CargarImagenPersonaje(Personaje pj)
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
                if (picPersonajePreview.Image != null)
                {
                    picPersonajePreview.Image.Dispose();
                }
                picPersonajePreview.Image = Image.FromFile(ruta);
            }
            catch { }
        }

        private void MostrarAtributos(Personaje pj)
        {
            panelStatsPreview.Controls.Clear();

            Label lblAtributosTitulo = new Label
            {
                Text = "ATRIBUTOS Y ESTADÍSTICAS",
                Font = FUENTE.ObtenerFont(16),
                ForeColor = Color.FromArgb(255, 180, 100),
                AutoSize = false,
                Size = new Size(830, 30),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(15, 15)
            };
            panelStatsPreview.Controls.Add(lblAtributosTitulo);

            string[] nombres = { "STR", "DEX", "CON", "INT", "WIS", "CHA" };
            int[] valores = { pj.STR, pj.DEX, pj.CON, pj.INT, pj.WIS, pj.CHA };

            for (int i = 0; i < 6; i++)
            {
                int col = i % 3;
                int row = i / 3;
                Panel cajaAtrib = CrearCajaAtributo(nombres[i], valores[i]);
                cajaAtrib.Location = new Point(15 + col * 280, 60 + row * 110);
                panelStatsPreview.Controls.Add(cajaAtrib);
            }
        }

        private Panel CrearCajaAtributo(string nombre, int valor)
        {
            Panel caja = new Panel
            {
                Size = new Size(270, 100),
                BackColor = Color.FromArgb(45, 48, 55)
            };
            RedondearMenu(caja, 12);

            Label lblNombre = new Label
            {
                Text = nombre,
                Font = FUENTE.ObtenerFont(14),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(15, 10)
            };
            caja.Controls.Add(lblNombre);

            Label lblValor = new Label
            {
                Text = valor.ToString(),
                Font = FUENTE.ObtenerFont(32),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(15, 35)
            };
            caja.Controls.Add(lblValor);

            int mod = (valor - 10) / 2;
            Label lblMod = new Label
            {
                Text = (mod >= 0 ? "+" : "") + mod.ToString(),
                Font = FUENTE.ObtenerFont(20),
                ForeColor = mod >= 0 ? Color.LimeGreen : Color.OrangeRed,
                AutoSize = true,
                Location = new Point(210, 45)
            };
            caja.Controls.Add(lblMod);

            return caja;
        }

        private void BtnCargar_Click(object? sender, EventArgs e)
        {
            if (lstPersonajes.SelectedIndex == -1)
            {
                MessageBox.Show("Selecciona un personaje para cargar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombre = lstPersonajes.SelectedItem.ToString()!;
            if (nombre == "No hay partidas guardadas") return;

            try
            {
                using (var conexion = new SQLiteConnection("Data Source=dnd_totr.sqlite;Version=3;"))
                {
                    conexion.Open();
                    string query = "SELECT * FROM PERSONAJES WHERE NOMBRE=@nombre";
                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Personaje pj = new Personaje()
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    NOMBRE = reader["NOMBRE"].ToString(),
                                    RAZA = reader["RAZA"].ToString(),
                                    SUBRAZA = reader["SUBRAZA"].ToString(),
                                    CLASE = reader["CLASE"].ToString(),
                                    TRASFONDO = reader["TRASFONDO"].ToString(),
                                    ALINEAMIENTO = reader["ALINEAMIENTO"].ToString(),
                                    LVL = Convert.ToInt32(reader["LVL"]),
                                    STR = Convert.ToInt32(reader["STR"]),
                                    DEX = Convert.ToInt32(reader["DEX"]),
                                    CON = Convert.ToInt32(reader["CON"]),
                                    INT = Convert.ToInt32(reader["INTE"]),
                                    WIS = Convert.ToInt32(reader["WIS"]),
                                    CHA = Convert.ToInt32(reader["CHA"]),
                                    HP = Convert.ToInt32(reader["HP"]),
                                    CA = Convert.ToInt32(reader["CA"]),
                                    VEL = Convert.ToInt32(reader["VEL"]),
                                    INI = Convert.ToInt32(reader["INI"]),
                                    DIN = Convert.ToInt32(reader["DIN"])
                                };

                                var pantallaJuego = new PANTALLA_JUEGO(pj);
                                pantallaJuego.Show();
                                this.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el personaje: " + ex.Message);
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (lstPersonajes.SelectedIndex == -1)
            {
                MessageBox.Show("Selecciona un personaje para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombre = lstPersonajes.SelectedItem.ToString()!;
            if (nombre == "No hay partidas guardadas") return;

            var resultado = MessageBox.Show(
                $"¿Estás seguro de que quieres eliminar la partida de '{nombre}'?\nEsta acción no se puede deshacer.",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    using (var conexion = new SQLiteConnection("Data Source=dnd_totr.sqlite;Version=3;"))
                    {
                        conexion.Open();
                        string query = "DELETE FROM PERSONAJES WHERE NOMBRE=@nombre";
                        using (var cmd = new SQLiteCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@nombre", nombre);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show($"Partida '{nombre}' eliminada correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarPersonajesDesdeBD();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el personaje: " + ex.Message);
                }
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