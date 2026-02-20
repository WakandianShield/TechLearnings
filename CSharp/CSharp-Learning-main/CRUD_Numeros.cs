using System;
using System.Collections.Generic;

class Program
{
    static List<int> numeros = new List<int>();
    static int capacidadMaxima = 20;

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nMenú:");
            Console.WriteLine("1- Ingresar número");
            Console.WriteLine("2- Eliminar número");
            Console.WriteLine("3- Buscar número");
            Console.WriteLine("4- Modificar número");
            Console.WriteLine("5- Mostrar números");
            Console.WriteLine("6- Salir");
            Console.Write("Seleccione una opción: ");

            if (int.TryParse(Console.ReadLine(), out int opcion))
            {
                switch (opcion)
                {
                    case 1: IngresarNumero(); break;
                    case 2: EliminarNumero(); break;
                    case 3: BuscarNumero(); break;
                    case 4: ModificarNumero(); break;
                    case 5: MostrarNumeros(); break;
                    case 6: return;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
            else
            {
                Console.WriteLine("Entrada no válida. Intente nuevamente.");
            }
        }
    }

    static void IngresarNumero()
    {
        if (numeros.Count >= capacidadMaxima)
        {
            Console.WriteLine("La lista está llena (20 números máximo).");
            return;
        }
        Console.Write("Ingrese un número: ");
        if (int.TryParse(Console.ReadLine(), out int num))
        {
            numeros.Add(num);
            Console.WriteLine("Número agregado.");
        }
        else
        {
            Console.WriteLine("Entrada no válida.");
        }
    }

    static void EliminarNumero()
    {
        if (numeros.Count == 0)
        {
            Console.WriteLine("La lista está vacía. Agregue números antes de eliminar.");
            return;
        }

        Console.Write("Ingrese el número a eliminar: ");
        if (int.TryParse(Console.ReadLine(), out int num) && numeros.Remove(num))
        {
            Console.WriteLine("Número eliminado.");
        }
        else
        {
            Console.WriteLine("Número no encontrado o entrada inválida.");
        }
    }

    static void BuscarNumero()
    {
        if (numeros.Count == 0)
        {
            Console.WriteLine("La lista está vacía. Agregue números antes de buscar.");
            return;
        }

        Console.Write("Ingrese el número a buscar: ");
        if (int.TryParse(Console.ReadLine(), out int num))
        {
            int index = numeros.IndexOf(num);
            if (index != -1)
                Console.WriteLine($"El número está en la lista en la posición {index + 1}.");
            else
                Console.WriteLine("Número no encontrado.");
        }
        else
        {
            Console.WriteLine("Entrada no válida.");
        }
    }

    static void ModificarNumero()
    {
        if (numeros.Count == 0)
        {
            Console.WriteLine("La lista está vacía. Agregue números antes de modificar.");
            return;
        }

        Console.Write("Ingrese el número a modificar: ");
        if (int.TryParse(Console.ReadLine(), out int num) && numeros.Contains(num))
        {
            Console.Write("Ingrese el nuevo valor: ");
            if (int.TryParse(Console.ReadLine(), out int nuevoNum))
            {
                int index = numeros.IndexOf(num);
                numeros[index] = nuevoNum;
                Console.WriteLine("Número modificado.");
            }
            else
            {
                Console.WriteLine("Entrada no válida.");
            }
        }
        else
        {
            Console.WriteLine("Número no encontrado o entrada inválida.");
        }
    }

    static void MostrarNumeros()
    {
        if (numeros.Count == 0)
        {
            Console.WriteLine("La lista está vacía. Agregue números antes de mostrar.");
            return;
        }

        Console.WriteLine("Números en la lista:");
        for (int i = 0; i < numeros.Count; i++)
        {
            Console.Write(numeros[i] + " ");
            if ((i + 1) % 3 == 0)
            {
                Console.WriteLine();
            }
        }
        Console.WriteLine();
    }
}
