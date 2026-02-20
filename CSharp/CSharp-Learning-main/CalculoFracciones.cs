using System;

class Fraccion
{
    public int Num, Den;

    public Fraccion(int n, int d)
    {
        if (d == 0)
            throw new ArgumentException("Denominador no puede ser cero");
        Num = n;
        Den = d;
        Simplificar();
    }

    private void Simplificar()
    {
        int mcd = MCD(Math.Abs(Num), Math.Abs(Den));
        Num /= mcd;
        Den /= mcd;
        // Aseguramos que el denominador siempre sea positivo
        if (Den < 0)
        {
            Num = -Num;
            Den = -Den;
        }
    }

    private int MCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public override string ToString()
    {
        if (Den == 1) return Num.ToString();
        if (Num % Den == 0) return (Num / Den).ToString();
        int entero = Num / Den;
        int nuevoNum = Math.Abs(Num % Den);
        return entero != 0 ? $"{entero} {nuevoNum}/{Den}" : $"{Num}/{Den}";
    }
}

class Program
{
    static Fraccion Operar(Fraccion a, Fraccion b, string operador)
    {
        switch (operador.ToLower())
        {
            case "suma": return new Fraccion(a.Num * b.Den + b.Num * a.Den, a.Den * b.Den);
            case "resta": return new Fraccion(a.Num * b.Den - b.Num * a.Den, a.Den * b.Den);
            case "multiplicacion": return new Fraccion(a.Num * b.Num, a.Den * b.Den);
            case "division": return new Fraccion(a.Num * b.Den, a.Den * b.Num);
            default: throw new ArgumentException("Operación no válida");
        }
    }

    static void Main()
    {
        bool continuar = true;
        Fraccion f1 = null, f2 = null;
        string operador = "";

        while (continuar)
        {
            if (f1 == null || f2 == null)
            {
                Console.Write("Ingrese primer numerador y denominador: ");
                f1 = new Fraccion(int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()));
                Console.Write("Ingrese segundo numerador y denominador: ");
                f2 = new Fraccion(int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()));
            }

            do
            {
                Console.Write("Ingrese operación (suma, resta, multiplicacion, division): ");
                operador = Console.ReadLine().ToLower();
            } while (operador != "suma" && operador != "resta" && operador != "multiplicacion" && operador != "division");

            Console.WriteLine("Resultado: " + Operar(f1, f2, operador));

            Console.Write("¿Desea hacer otra operación con las mismas fracciones? (si/no): ");
            string respuesta = Console.ReadLine().ToLower();
            if (respuesta != "si")
            {
                Console.Write("¿Desea ingresar nuevas fracciones? (si/no): ");
                string nuevaFraccion = Console.ReadLine().ToLower();
                if (nuevaFraccion == "si")
                {
                    f1 = null;
                    f2 = null;
                }
                else
                {
                    continuar = false;
                }
            }
        }
    }
}
