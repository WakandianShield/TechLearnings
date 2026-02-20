namespace CALCULADORA
{
    public partial class CALCULADORA : Form
    {
        public CALCULADORA()
        {
            InitializeComponent();
        }

        List<string> historialOperaciones = new List<string>();
        bool S = true;
        string operador;
        double num1;
        double num2;
        double resultado;

        private void boton1_Click(object sender, EventArgs e)
        {
            if (S == true)
            {
                pantallaprincipal.Text = "1";
                S = false;
            }
            else
            {
                pantallaprincipal.Text = pantallaprincipal.Text + "1";
            }
        }

        private void boton2_Click(object sender, EventArgs e)
        {
            if (S == true)
            {
                pantallaprincipal.Text = "2";
                S = false;
            }
            else
            {
                pantallaprincipal.Text = pantallaprincipal.Text + "2";
            }
        }

        private void boton3_Click(object sender, EventArgs e)
        {
            if (S == true)
            {
                pantallaprincipal.Text = "3";
                S = false;
            }
            else
            {
                pantallaprincipal.Text = pantallaprincipal.Text + "3";
            }
        }

        private void boton4_Click(object sender, EventArgs e)
        {
            if (S == true)
            {
                pantallaprincipal.Text = "4";
                S = false;
            }
            else
            {
                pantallaprincipal.Text = pantallaprincipal.Text + "4";
            }
        }

        private void boton5_Click(object sender, EventArgs e)
        {
            if (S == true)
            {
                pantallaprincipal.Text = "5";
                S = false;
            }
            else
            {
                pantallaprincipal.Text = pantallaprincipal.Text + "5";
            }
        }

        private void boton6_Click(object sender, EventArgs e)
        {
            if (S == true)
            {
                pantallaprincipal.Text = "6";
                S = false;
            }
            else
            {
                pantallaprincipal.Text = pantallaprincipal.Text + "6";
            }
        }

        private void boton7_Click(object sender, EventArgs e)
        {
            if (S == true)
            {
                pantallaprincipal.Text = "7";
                S = false;
            }
            else
            {
                pantallaprincipal.Text = pantallaprincipal.Text + "7";
            }
        }

        private void boton8_Click(object sender, EventArgs e)
        {
            if (S == true)
            {
                pantallaprincipal.Text = "8";
                S = false;
            }
            else
            {
                pantallaprincipal.Text = pantallaprincipal.Text + "8";
            }
        }

        private void boton9_Click(object sender, EventArgs e)
        {
            if (S == true)
            {
                pantallaprincipal.Text = "9";
                S = false;
            }
            else
            {
                pantallaprincipal.Text = pantallaprincipal.Text + "9";
            }
        }

        private void boton0_Click(object sender, EventArgs e)
        {
            if (pantallaprincipal.Text == "0")
            {
                pantallaprincipal.Text = "0";
            }
            else
            {
                pantallaprincipal.Text = pantallaprincipal.Text + "0";
            }
        }

        private void opmas_Click(object sender, EventArgs e)
        {
            operador = "+";
            num1 = double.Parse(pantallaprincipal.Text);
            pantallasecundaria.Text = pantallaprincipal.Text + "+";
            S = true;
        }

        private void opmenos_Click(object sender, EventArgs e)
        {
            operador = "-";
            num1 = double.Parse(pantallaprincipal.Text);
            pantallasecundaria.Text = pantallaprincipal.Text + "-";
            S = true;
        }

        private void opmultiplicar_Click(object sender, EventArgs e)
        {
            operador = "x";
            num1 = double.Parse(pantallaprincipal.Text);
            pantallasecundaria.Text = pantallaprincipal.Text + "x";
            S = true;
        }

        private void opdivisor_Click(object sender, EventArgs e)
        {
            operador = "/";
            num1 = double.Parse(pantallaprincipal.Text);
            pantallasecundaria.Text = pantallaprincipal.Text + "/";
            S = true;
        }

        private void botonigual_Click(object sender, EventArgs e)
        {
            num2 = double.Parse(pantallaprincipal.Text);
            switch (operador)
            {
                case "+":
                    resultado = num1 + num2;
                    pantallasecundaria.Text = pantallasecundaria.Text + num2.ToString() + "=";
                    pantallaprincipal.Text = resultado.ToString();
                    break;
                case "-":
                    resultado = num1 - num2;
                    pantallasecundaria.Text = pantallasecundaria.Text + num2.ToString() + "=";
                    pantallaprincipal.Text = resultado.ToString();
                    break;
                case "x":
                    resultado = num1 * num2;
                    pantallasecundaria.Text = pantallasecundaria.Text + num2.ToString() + "=";
                    pantallaprincipal.Text = resultado.ToString();
                    break;
                case "/":
                    resultado = num1 / num2;
                    pantallasecundaria.Text = pantallasecundaria.Text + num2.ToString() + "=";
                    pantallaprincipal.Text = resultado.ToString();
                    break;
            }

            string operacion = $"{num1} {operador} {num2} = {resultado}";
            historialOperaciones.Add(operacion);
            pantallasecundaria.Text = operacion;
            pantallaprincipal.Text = resultado.ToString();

            ActualizarHistorial();
        }

        private void botondelete_Click(object sender, EventArgs e)
        {
            pantallaprincipal.Text = "0";
            pantallasecundaria.Text = " ";
        }

        private void ActualizarHistorial()
        {
            historial.Text = string.Join("\n", historialOperaciones);
        }

        private void historial_Click(object sender, EventArgs e)
        {

        }

        private void historialboton_Click(object sender, EventArgs e)
        {
            historial.Text = " ";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }

}
