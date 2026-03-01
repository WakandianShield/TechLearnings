using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Evento para cuando el checkbox de "MALA" se cambia
        private void CHECKBOX_MALA_CheckedChanged(object sender, EventArgs e)
        {
            // Si CHECKBOX_MALA se selecciona, desmarcar CHECKBOX_BUENA
            if (CHECKBOX_MALA.Checked)
            {
                CHECKBOX_BUENA.Checked = false;
            }
        }

        // Evento para cuando el checkbox de "BUENA" se cambia
        private void CHECKBOX_BUENA_CheckedChanged(object sender, EventArgs e)
        {
            // Si CHECKBOX_BUENA se selecciona, desmarcar CHECKBOX_MALA
            if (CHECKBOX_BUENA.Checked)
            {
                CHECKBOX_MALA.Checked = false;
            }
        }

        private void BOTON_ENVIAR_Click_1(object sender, EventArgs e)
        {
            // Verificar que todos los campos estén completos
            if (string.IsNullOrWhiteSpace(TEXTBOX_NOMBRE.Text) || string.IsNullOrWhiteSpace(TEXTBOX_REGISTRO.Text) || string.IsNullOrWhiteSpace(TEXTBOX_EDAD.Text) || COMBOBOX_SEMESTRE.SelectedIndex == -1 || (!RADIOBUTTON_REGULAR.Checked && !RADIOBUTTON_NO_REGULAR.Checked))
            {
                MessageBox.Show("Por favor completa todos los campos.");
                return;
            }

            // Obtener los valores ingresados
            string nombre = TEXTBOX_NOMBRE.Text;
            string semestre = COMBOBOX_SEMESTRE.SelectedItem.ToString();
            string registro = TEXTBOX_REGISTRO.Text;
            string edad = TEXTBOX_EDAD.Text;
            string estatus = RADIOBUTTON_REGULAR.Checked ? "REGULAR" : "NO REGULAR";

            // Preparar la información a guardar con un formato claro
            string informacion = $"Nombre: {nombre}\n" +
                                 $"Registro: {registro}\n" +
                                 $"Edad: {edad}\n" +
                                 $"Semestre: {semestre}\n" +
                                 $"Estatus: {estatus}\n" +
                                 $"Opinion Mala: {(CHECKBOX_MALA.Checked ? "Sí" : "No")}\n" +
                                 $"Opinion Buena: {(CHECKBOX_BUENA.Checked ? "Sí" : "No")}\n" +
                                 $"----------------------------\n";

            // Especificar la ruta del archivo en el escritorio del usuario
            string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "INFORMACION_DEL_ALUMNO.txt");

            try
            {
                // Verificar si el archivo ya existe
                if (!File.Exists(rutaArchivo))
                {
                    // Si el archivo no existe, se crea uno nuevo con un encabezado
                    File.WriteAllText(rutaArchivo, "Información de estudiantes:\n\n");
                }

                // Usar File.AppendAllText para agregar la información al archivo sin sobrescribirlo
                File.AppendAllText(rutaArchivo, informacion);
                MessageBox.Show("Información guardada exitosamente en el escritorio.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la información: {ex.Message}");
            }
        }
    }
}
