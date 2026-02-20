using System;
using System.Collections.Generic;
class ALUMNOS
{
    public string NOMBRE;
    public string APELLIDO;
    public int EDAD;
}
class PROGRAMA
{
    static List<ALUMNOS> listalumno = new List<ALUMNOS>();
    static void agregar()
    {
        ALUMNOS a = new ALUMNOS();
        Console.WriteLine("AGREGANDO UN NUEVO ALUMNO");

        Console.WriteLine("NOMBRE: ");
        a.NOMBRE = Console.ReadLine().ToUpper();


        Console.WriteLine("APELLIDO: ");
        a.APELLIDO = Console.ReadLine().ToUpper();

        Console.WriteLine("EDAD: ");
        a.EDAD = Convert.ToInt32(Console.ReadLine());

        listalumno.Add(a);
    }
    static void eliminar()
    {
        if (listalumno.Count != 0)
        {
            Console.WriteLine("PON EL NOMBRE QUE DESEAS ELIMINAR DE LA LISTA: ");
            string nombre = Console.ReadLine().ToUpper();
            for (int i = 0; i < listalumno.Count; i++)
            {
                if (listalumno[i].NOMBRE == nombre)
                {
                    listalumno.RemoveAt(i);
                    Console.WriteLine("ALUMNO ELIMINADO\n");
                }
            }
        }
        else
        {
            Console.WriteLine("LISTA VACIA");
        }
    }
    static void buscar()
    {
        if (listalumno.Count != 0)
        {
            Console.WriteLine("\nPON EL NOMBRE QUE DESEAS BUSCAR DE LA LISTA: ");
            string nombre = Console.ReadLine().ToUpper();

            for (int i = 0; i < listalumno.Count; i++)
            {
                if (listalumno[i].NOMBRE == nombre)
                {
                    Console.WriteLine("\nPOSICION DE LA LISTA: " + i);
                    Console.WriteLine("NOMBRE: " + listalumno[i].NOMBRE);
                    Console.WriteLine("APELLIDO: " + listalumno[i].APELLIDO);
                    Console.WriteLine("EDAD: " + listalumno[i].EDAD);
                }
            }
        }
        else
        {
            Console.WriteLine("LISTA VACIA");
        }
    }
    static void modificar()
    {
        if (listalumno.Count != 0)
        {
            Console.WriteLine("PON EL NOMBRE DEL ALUMNO QUE DESEAS MODIFICAR DE LA LISTA: ");
            string nombre = Console.ReadLine().ToUpper();

            for (int i = 0; i < listalumno.Count; i++)
            {
                if (listalumno[i].NOMBRE == nombre)
                {
                    Console.WriteLine("¿QUE DATO DESEAS MODIFICAR DEL ALUMNO?");
                    Console.WriteLine("1. NOMBRE");
                    Console.WriteLine("2. APELLIDO");
                    Console.WriteLine("3. EDAD");
                    int op = Convert.ToInt32(Console.ReadLine());
                    switch (op)
                    {
                        case 1:
                            Console.WriteLine("NOMBRE NUEVO: ");
                            listalumno[i].NOMBRE = Console.ReadLine().ToUpper();
                            break;
                        case 2:
                            Console.WriteLine("APELLIDO NUEVO: ");
                            listalumno[i].APELLIDO = Console.ReadLine().ToUpper();
                            break;
                        case 3:
                            Console.WriteLine("EDAD NUEVA: ");
                            listalumno[i].EDAD = Convert.ToInt32(Console.ReadLine().ToUpper());
                            break;
                            return;
                    }
                    ;
                }
            }
        }
        else
        {
            Console.WriteLine("LISTA VACIA");
        }
    }

    static void mostrar()
    {
        if (listalumno.Count == 0)
        {
            Console.WriteLine("LISTA VACIA");
        }
        else
        {
            for (int i = 0; i < listalumno.Count; i++)
            {
                Console.WriteLine(i + ". NOMBRE: -" + listalumno[i].NOMBRE + "- APELLIDO: -" + listalumno[i].APELLIDO + "- EDAD: -" + listalumno[i].EDAD + "-\n");
            }
        }
    }
    static void Main()
    {
        int opcion = 0;

        while (opcion < 6)
        {
            Console.WriteLine("\n1-Agregar");
            Console.WriteLine("2-Eliminar");
            Console.WriteLine("3-Buscar");
            Console.WriteLine("4-Modificar");
            Console.WriteLine("5-Mostrar");
            Console.WriteLine("6-Salir");
            Console.Write("Elige una opcion (pon el numero): ");
            opcion = Convert.ToInt32(Console.ReadLine());
            Console.Write('\n');

            if (opcion == 1) agregar();
            else if (opcion == 2) eliminar();
            else if (opcion == 3) buscar();
            else if (opcion == 4) modificar();
            else if (opcion == 5) mostrar();
        }
    }
}