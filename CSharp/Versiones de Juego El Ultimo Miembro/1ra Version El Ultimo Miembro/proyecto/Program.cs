using System;
using System.Drawing.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms;

namespace proyecto
{


    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenuForm());
        }
    }

    // ESTO ES PA LA SIMULACION OSEA LO VAMOS A BORRAR LUEIGO

    public static class GameData
    {
        public static bool TienePartida { get; set; } = false;
        public static string PersonajeNombre { get; set; } = "";
        public static string Raza { get; set; } = "";
        public static string SubRaza { get; set; } = "";
        public static string Trasfondo { get; set; } = "";
        public static string Alineamiento { get; set; } = "";
        public static string Habilidad { get; set; } = "";
        public static string RazaSeleccionada { get; set; } = "";
        public static string ClaseSeleccionada { get; set; } = "";
        public static string Subraza { get; set; } = "";
        public static string AlineamientoSeleccionado { get; set; } = "";
        public static string HabilidadSeleccionada { get; set; } = "";
        public static string PersonajeNombreFinal { get; set; } = "";
        


            // Selecciones del jugador
            public static string PersonajeRaza;
            public static string PersonajeSubraza;
            public static string PersonajeClase;
            public static string PersonajeTrasfondo;
            public static string PersonajeAlineamiento;

            // Estadísticas
            public static int StatsFuerza;
            public static int StatsDestreza;
            public static int StatsConstitucion;
            public static int StatsInteligencia;
            public static int StatsSabiduria;
            public static int StatsCarisma;

            public static void Inicializar()
            {
                // Valores por defecto, opcional
                PersonajeRaza = "";
                PersonajeSubraza = "";
                PersonajeClase = "";
                PersonajeTrasfondo = "";
                PersonajeAlineamiento = "";

                StatsFuerza = 10;
                StatsDestreza = 10;
                StatsConstitucion = 10;
                StatsInteligencia = 10;
                StatsSabiduria = 10;
                StatsCarisma = 10;
            }

            // Método para serializar a JSON y guardar
            public static string GuardarPersonajeComoJson()
            {
                var personaje = new
                {
                    Raza = PersonajeRaza,
                    Subraza = PersonajeSubraza,
                    Clase = PersonajeClase,
                    Trasfondo = PersonajeTrasfondo,
                    Alineamiento = PersonajeAlineamiento,
                    Stats = new
                    {
                        Fuerza = StatsFuerza,
                        Destreza = StatsDestreza,
                        Constitucion = StatsConstitucion,
                        Inteligencia = StatsInteligencia,
                        Sabiduria = StatsSabiduria,
                        Carisma = StatsCarisma
                    }
                };

                return JsonSerializer.Serialize(personaje, new JsonSerializerOptions { WriteIndented = true });
            }



        public static void CargarPersonajeDesdeJson(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
                throw new FileNotFoundException("Archivo de personaje no encontrado");

            string json = File.ReadAllText(rutaArchivo);
            var personaje = JsonSerializer.Deserialize<PersonajeData>(json);

            PersonajeRaza = personaje.Raza;
            PersonajeSubraza = personaje.Subraza;
            PersonajeClase = personaje.Clase;
            PersonajeTrasfondo = personaje.Trasfondo;
            PersonajeAlineamiento = personaje.Alineamiento;

            StatsFuerza = personaje.Stats.Fuerza;
            StatsDestreza = personaje.Stats.Destreza;
            StatsConstitucion = personaje.Stats.Constitucion;
            StatsInteligencia = personaje.Stats.Inteligencia;
            StatsSabiduria = personaje.Stats.Sabiduria;
            StatsCarisma = personaje.Stats.Carisma;
        }


        private class PersonajeData
        {
            public string Raza { get; set; }
            public string Subraza { get; set; }
            public string Clase { get; set; }
            public string Trasfondo { get; set; }
            public string Alineamiento { get; set; }
            public StatsData Stats { get; set; }
        }

        private class StatsData
        {
            public int Fuerza { get; set; }
            public int Destreza { get; set; }
            public int Constitucion { get; set; }
            public int Inteligencia { get; set; }
            public int Sabiduria { get; set; }
            public int Carisma { get; set; }
        }


    }


}
