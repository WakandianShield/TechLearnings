using proyecto;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace PROYECTO_5TO___TOTЯ
{
    public class TIENDA : Form
    {
        private Personaje pj;
        private Panel panelPrincipal;
        private Label lblDinero;
        private Panel panelContenidoItems;
        private string categoriaActual = "consumibles";

        // Items organizados por categoría
        private readonly Dictionary<string, List<ItemTienda>> itemsPorCategoria = new Dictionary<string, List<ItemTienda>>
        {
            ["consumibles"] = new List<ItemTienda>
            {
                new ItemTienda("POCIÓN DE CURACIÓN MENOR", "Restaura 50 HP", 50, "⚗", Color.FromArgb(100, 220, 100)),
                new ItemTienda("POCIÓN DE CURACIÓN MAYOR", "Restaura 100 HP", 120, "⚗", Color.FromArgb(80, 255, 120)),
                new ItemTienda("POCIÓN DE MANÁ", "Restaura 30 MP", 80, "⚗", Color.FromArgb(100, 150, 255)),
                new ItemTienda("ANTÍDOTO", "Cura envenenamiento", 100, "⚗", Color.FromArgb(150, 255, 150)),
                new ItemTienda("ELIXIR DE VELOCIDAD", "Aumenta VEL +2 temporalmente", 150, "⚗", Color.FromArgb(255, 200, 100)),
                new ItemTienda("POCIÓN DE FUERZA", "Aumenta STR +3 temporalmente", 180, "⚗", Color.FromArgb(255, 150, 150))
            },

            ["armas"] = new List<ItemTienda>
            {
                new ItemTienda("ESPADA CORTA", "Daño base +5", 200, "⚔", Color.FromArgb(220, 80, 80)),
                new ItemTienda("ESPADA LARGA", "Daño base +8", 400, "⚔", Color.FromArgb(255, 100, 100)),
                new ItemTienda("HACHA DE BATALLA", "Daño base +10, -1 VEL", 500, "⚔", Color.FromArgb(200, 60, 60)),
                new ItemTienda("ARCO ÉLFICO", "Daño base +7, +1 DEX", 450, "🏹", Color.FromArgb(150, 200, 100)),
                new ItemTienda("DAGA ASESINA", "Daño base +6, +2 VEL", 350, "🗡", Color.FromArgb(180, 100, 200)),
                new ItemTienda("BÁCULO MÁGICO", "Daño mágico +12, +2 INT", 600, "🪄", Color.FromArgb(150, 150, 255))
            },

            ["armaduras"] = new List<ItemTienda>
            {
                new ItemTienda("ARMADURA DE CUERO", "CA +2", 300, "🛡", Color.FromArgb(80, 150, 220)),
                new ItemTienda("COTA DE MALLAS", "CA +4, -1 VEL", 500, "🛡", Color.FromArgb(100, 180, 255)),
                new ItemTienda("ARMADURA COMPLETA", "CA +6, -2 VEL", 800, "🛡", Color.FromArgb(120, 200, 255)),
                new ItemTienda("ESCUDO DE MADERA", "CA +1", 150, "🛡", Color.FromArgb(150, 120, 80)),
                new ItemTienda("ESCUDO DE HIERRO", "CA +3", 400, "🛡", Color.FromArgb(180, 180, 180)),
                new ItemTienda("TÚNICA DE MAGO", "CA +1, +2 INT", 350, "🛡", Color.FromArgb(100, 100, 255))
            },

            ["hechizos"] = new List<ItemTienda>
            {
                new ItemTienda("BOLA DE FUEGO", "Daño mágico de fuego (30 daño)", 250, "🔥", Color.FromArgb(255, 100, 50)),
                new ItemTienda("RAYO CONGELANTE", "Daño de hielo + ralentiza (25 daño)", 300, "❄", Color.FromArgb(100, 200, 255)),
                new ItemTienda("CURACIÓN DIVINA", "Restaura 75 HP", 200, "✨", Color.FromArgb(255, 255, 150)),
                new ItemTienda("ESCUDO ARCANO", "Aumenta CA +5 por 3 turnos", 350, "🔮", Color.FromArgb(150, 100, 255)),
                new ItemTienda("TELETRANSPORTE", "Te mueve a cualquier posición visible", 400, "🌀", Color.FromArgb(200, 150, 255)),
                new ItemTienda("INVOCACIÓN: FAMILIAR", "Invoca un aliado por 5 turnos", 500, "👻", Color.FromArgb(180, 100, 200))
            },

            ["habilidades"] = new List<ItemTienda>
            {
                new ItemTienda("GOLPE CRÍTICO", "Aumenta probabilidad de crítico +15%", 300, "💥", Color.FromArgb(255, 200, 0)),
                new ItemTienda("BLOQUEO EXPERTO", "Aumenta probabilidad de bloqueo +20%", 350, "🛡", Color.FromArgb(100, 150, 255)),
                new ItemTienda("SIGILO", "Aumenta probabilidad de evasión +25%", 400, "👤", Color.FromArgb(100, 100, 150)),
                new ItemTienda("PRIMEROS AUXILIOS", "Restaura 30 HP automáticamente al llegar a 20% HP", 450, "❤", Color.FromArgb(255, 100, 100)),
                new ItemTienda("CONTRAATAQUE", "Devuelve 30% del daño recibido", 500, "⚡", Color.FromArgb(255, 255, 100)),
                new ItemTienda("REGENERACIÓN", "Restaura 5 HP por turno", 600, "♻", Color.FromArgb(100, 255, 150))
            }
        };

        public TIENDA(Personaje personaje)
        {
            pj = personaje;
            InicializarFormulario();
            CrearInterfaz();
        }

        private void InicializarFormulario()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(15, 15, 20);
            this.DoubleBuffered = true;
            this.Size = new Size(1920, 1080);
        }

        private void CrearInterfaz()
        {
            // PANEL PRINCIPAL
            panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Size = new Size(1920, 1080)
            };
            this.Controls.Add(panelPrincipal);

            CrearHeader();
            CrearMenuCategorias();
            CrearPanelItems();
            CrearFooter();
        }

        private void CrearHeader()
        {
            Panel header = new Panel
            {
                Size = new Size(1920, 120),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(25, 28, 35)
            };
            panelPrincipal.Controls.Add(header);

            // TÍTULO
            Label lblTitulo = new Label
            {
                Text = "⚒ TIENDA DEL AVENTURERO ⚒",
                Font = FUENTE.ObtenerFont(42),
                ForeColor = Color.FromArgb(255, 200, 80),
                AutoSize = true,
                Location = new Point(60, 35)
            };
            header.Controls.Add(lblTitulo);

            // BOTÓN CERRAR
            Button btnCerrar = new Button
            {
                Text = "✕",
                Font = FUENTE.ObtenerFont(32),
                Size = new Size(70, 70),
                Location = new Point(1820, 25),
                BackColor = Color.FromArgb(80, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            RedondearBoton(btnCerrar, 20);
            btnCerrar.Click += (s, e) => this.Close();
            btnCerrar.MouseEnter += (s, e) => btnCerrar.BackColor = Color.FromArgb(120, 60, 60);
            btnCerrar.MouseLeave += (s, e) => btnCerrar.BackColor = Color.FromArgb(80, 50, 50);
            header.Controls.Add(btnCerrar);

            // PANEL DINERO
            Panel panelDinero = new Panel
            {
                Size = new Size(250, 70),
                Location = new Point(1550, 25),
                BackColor = Color.FromArgb(40, 45, 55)
            };
            header.Controls.Add(panelDinero);
            RedondearPanel(panelDinero, 20);

            lblDinero = new Label
            {
                Text = $"{pj.DIN} 🪙",
                Font = FUENTE.ObtenerFont(28),
                ForeColor = Color.Gold,
                AutoSize = false,
                Size = new Size(230, 70),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelDinero.Controls.Add(lblDinero);
        }

        private void CrearMenuCategorias()
        {
            Panel menuCategorias = new Panel
            {
                Size = new Size(1800, 90),
                Location = new Point(60, 140),
                BackColor = Color.FromArgb(25, 28, 35)
            };
            panelPrincipal.Controls.Add(menuCategorias);
            RedondearPanel(menuCategorias, 20);

            string[] categorias = { "consumibles", "armas", "armaduras", "hechizos", "habilidades" };
            string[] nombres = { "CONSUMIBLES", "ARMAS", "ARMADURAS", "HECHIZOS", "HABILIDADES" };
            string[] iconos = { "⚗", "⚔", "🛡", "🔮", "💫" };
            Color[] colores = {
                Color.FromArgb(100, 220, 100),
                Color.FromArgb(220, 80, 80),
                Color.FromArgb(80, 150, 220),
                Color.FromArgb(150, 100, 255),
                Color.FromArgb(255, 200, 0)
            };

            int anchoBoton = (menuCategorias.Width - 80) / 5;
            for (int i = 0; i < categorias.Length; i++)
            {
                int index = i;
                Button btnCategoria = new Button
                {
                    Text = $"{iconos[i]}\n{nombres[i]}",
                    Size = new Size(anchoBoton, 80),
                    Location = new Point(15 + (anchoBoton + 10) * i, 5),
                    BackColor = i == 0 ? colores[i] : Color.FromArgb(40, 45, 55),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = FUENTE.ObtenerFont(16),
                    Cursor = Cursors.Hand,
                    Tag = new { categoria = categorias[i], color = colores[i] }
                };
                btnCategoria.FlatAppearance.BorderSize = 0;
                RedondearBoton(btnCategoria, 15);

                btnCategoria.Click += (s, e) =>
                {
                    // Resetear todos los botones
                    foreach (Control c in menuCategorias.Controls)
                    {
                        if (c is Button btn)
                        {
                            var tag = btn.Tag as dynamic;
                            btn.BackColor = Color.FromArgb(40, 45, 55);
                        }
                    }

                    // Activar botón seleccionado
                    var tagData = btnCategoria.Tag as dynamic;
                    btnCategoria.BackColor = tagData.color;
                    categoriaActual = tagData.categoria;
                    CargarItems();
                };

                btnCategoria.MouseEnter += (s, e) =>
                {
                    if (btnCategoria.BackColor == Color.FromArgb(40, 45, 55))
                        btnCategoria.BackColor = Color.FromArgb(60, 65, 75);
                };
                btnCategoria.MouseLeave += (s, e) =>
                {
                    var tagData = btnCategoria.Tag as dynamic;
                    if (categoriaActual != tagData.categoria)
                        btnCategoria.BackColor = Color.FromArgb(40, 45, 55);
                };

                menuCategorias.Controls.Add(btnCategoria);
            }
        }

        private void CrearPanelItems()
        {
            panelContenidoItems = new Panel
            {
                Size = new Size(1800, 650),
                Location = new Point(60, 250),
                BackColor = Color.FromArgb(20, 23, 30),
                AutoScroll = true,
                Padding = new Padding(20)
            };
            panelPrincipal.Controls.Add(panelContenidoItems);
            RedondearPanel(panelContenidoItems, 20);

            CargarItems();
        }

        private void CargarItems()
        {
            panelContenidoItems.Controls.Clear();

            if (!itemsPorCategoria.ContainsKey(categoriaActual))
                return;

            var items = itemsPorCategoria[categoriaActual];
            int columns = 4; // Una columna más para mejor uso del espacio
            int cardWidth = 400; // Ancho fijo para las cards
            int cardHeight = 200; // Altura aumentada para mejor visualización
            int padding = 25;

            for (int i = 0; i < items.Count; i++)
            {
                int col = i % columns;
                int row = i / columns;
                int x = padding + (cardWidth + padding) * col;
                int y = padding + (cardHeight + padding) * row;

                Panel card = CrearCardItem(items[i], cardWidth, cardHeight);
                card.Location = new Point(x, y);
                panelContenidoItems.Controls.Add(card);
            }
        }

        private Panel CrearCardItem(ItemTienda item, int ancho, int alto)
        {
            Panel card = new Panel
            {
                Size = new Size(ancho, alto),
                BackColor = Color.FromArgb(35, 40, 50)
            };
            RedondearPanel(card, 15);

            // BARRA SUPERIOR DE COLOR
            Panel barraColor = new Panel
            {
                Size = new Size(ancho, 8),
                Location = new Point(0, 0),
                BackColor = item.Color
            };
            card.Controls.Add(barraColor);

            // ICONO GRANDE
            Label lblIcono = new Label
            {
                Text = item.Icono,
                Font = FUENTE.ObtenerFont(52),
                ForeColor = item.Color,
                AutoSize = false,
                Size = new Size(80, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 20)
            };
            card.Controls.Add(lblIcono);

            // NOMBRE
            Label lblNombre = new Label
            {
                Text = item.Nombre,
                Font = FUENTE.ObtenerFont(16),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(ancho - 120, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(110, 25)
            };
            card.Controls.Add(lblNombre);

            // DESCRIPCIÓN
            Label lblDesc = new Label
            {
                Text = item.Descripcion,
                Font = FUENTE.ObtenerFont(12),
                ForeColor = Color.LightGray,
                AutoSize = false,
                Size = new Size(ancho - 40, 40),
                TextAlign = ContentAlignment.TopLeft,
                Location = new Point(20, 110)
            };
            card.Controls.Add(lblDesc);

            // PRECIO
            Label lblPrecio = new Label
            {
                Text = $"{item.Precio} 🪙",
                Font = FUENTE.ObtenerFont(18),
                ForeColor = Color.Gold,
                AutoSize = true,
                Location = new Point(20, alto - 40)
            };
            card.Controls.Add(lblPrecio);

            // BOTÓN COMPRAR
            Button btnComprar = new Button
            {
                Text = "COMPRAR",
                Size = new Size(140, 35),
                Location = new Point(ancho - 160, alto - 45),
                BackColor = Color.FromArgb(50, 150, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = FUENTE.ObtenerFont(14),
                Cursor = Cursors.Hand
            };
            btnComprar.FlatAppearance.BorderSize = 0;
            RedondearBoton(btnComprar, 10);
            btnComprar.Click += (s, e) => ComprarItem(item);
            btnComprar.MouseEnter += (s, e) => btnComprar.BackColor = Color.FromArgb(60, 180, 100);
            btnComprar.MouseLeave += (s, e) => btnComprar.BackColor = Color.FromArgb(50, 150, 80);
            card.Controls.Add(btnComprar);

            return card;
        }

        private void ComprarItem(ItemTienda item)
        {
            if (pj.DIN < item.Precio)
            {
                MessageBox.Show("¡NO TIENES SUFICIENTE DINERO!",
                    "Fondos Insuficientes",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                $"¿Deseas comprar '{item.Nombre}' por {item.Precio} monedas?",
                "Confirmar Compra",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                // Restar dinero
                pj.DIN -= item.Precio;
                lblDinero.Text = $"{pj.DIN} 🪙";

                // Actualizar en base de datos
                BASE_DE_DATOS.ActualizarDinero(pj.ID, pj.DIN);

                // Agregar al inventario según categoría
                BASE_DE_DATOS.AgregarItemInventario(pj.ID, item.Nombre, categoriaActual);

                MessageBox.Show(
                    $"¡Compraste '{item.Nombre}'!\nSe ha agregado a tu inventario.",
                    "Compra Exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void CrearFooter()
        {
            Panel footer = new Panel
            {
                Size = new Size(1920, 80),
                Location = new Point(0, 920),
                BackColor = Color.FromArgb(25, 28, 35)
            };
            panelPrincipal.Controls.Add(footer);

            Label lblInfo = new Label
            {
                Text = "💡 Consejo: Los consumibles se gastan al usarse. Las armas, armaduras, hechizos y habilidades son permanentes.",
                Font = FUENTE.ObtenerFont(14),
                ForeColor = Color.FromArgb(180, 180, 180),
                AutoSize = false,
                Size = new Size(1800, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(60, 0)
            };
            footer.Controls.Add(lblInfo);
        }

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

        // Clase auxiliar para items
        private class ItemTienda
        {
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public int Precio { get; set; }
            public string Icono { get; set; }
            public Color Color { get; set; }

            public ItemTienda(string nombre, string desc, int precio, string icono, Color color)
            {
                Nombre = nombre;
                Descripcion = desc;
                Precio = precio;
                Icono = icono;
                Color = color;
            }
        }
    }
}