using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[Serializable]
public class ALUMNOS
{
    public string NOMBRE;
    public string APELLIDO;
    public int EDAD;
}

class PROGRAMA
{
    static List<ALUMNOS> listalumno = new List<ALUMNOS>();
    static string archivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "alumnos.xml");

    static void GuardarXML()
    {
        XmlSerializer serializador = new XmlSerializer(typeof(List<ALUMNOS>));
        using (StreamWriter escritor = new StreamWriter(archivo))
        {
            serializador.Serialize(escritor, listalumno);
        }
    }

    static void CargarXML()
    {
        if (File.Exists(archivo))
        {
            XmlSerializer serializador = new XmlSerializer(typeof(List<ALUMNOS>));
            using (StreamReader lector = new StreamReader(archivo))
            {
                listalumno = (List<ALUMNOS>)serializador.Deserialize(lector);
            }
        }
    }

    static void agregar()
    {
        ALUMNOS a = new ALUMNOS();
        Console.WriteLine("AGREGANDO UN NUEVO ALUMNO");

        Console.Write("NOMBRE: ");
        a.NOMBRE = Console.ReadLine().ToUpper();

        Console.Write("APELLIDO: ");
        a.APELLIDO = Console.ReadLine().ToUpper();

        Console.Write("EDAD: ");
        a.EDAD = Convert.ToInt32(Console.ReadLine());

        listalumno.Add(a);
        GuardarXML();  // Guardar tras agregar
    }

    static void eliminar()
    {
        if (listalumno.Count != 0)
        {
            Console.Write("PON EL NOMBRE QUE DESEAS ELIMINAR DE LA LISTA: ");
            string nombre = Console.ReadLine().ToUpper();
            bool eliminado = false;
            for (int i = listalumno.Count - 1; i >= 0; i--)
            {
                if (listalumno[i].NOMBRE == nombre)
                {
                    listalumno.RemoveAt(i);
                    eliminado = true;
                }
            }
            if (eliminado)
            {
                Console.WriteLine("ALUMNO(S) ELIMINADO(S)\n");
                GuardarXML();
            }
            else
            {
                Console.WriteLine("NO SE ENCONTRÓ ALUMNO CON ESE NOMBRE\n");
            }
        }
        else
        {
            Console.WriteLine("LISTA VACÍA");
        }
    }

    static void buscar()
    {
        if (listalumno.Count != 0)
        {
            Console.Write("\nPON EL NOMBRE QUE DESEAS BUSCAR DE LA LISTA: ");
            string nombre = Console.ReadLine().ToUpper();

            bool encontrado = false;
            for (int i = 0; i < listalumno.Count; i++)
            {
                if (listalumno[i].NOMBRE == nombre)
                {
                    Console.WriteLine("\nPOSICION DE LA LISTA: " + i);
                    Console.WriteLine("NOMBRE: " + listalumno[i].NOMBRE);
                    Console.WriteLine("APELLIDO: " + listalumno[i].APELLIDO);
                    Console.WriteLine("EDAD: " + listalumno[i].EDAD);
                    encontrado = true;
                }
            }
            if (!encontrado)
                Console.WriteLine("NO SE ENCONTRÓ ALUMNO CON ESE NOMBRE");
        }
        else
        {
            Console.WriteLine("LISTA VACÍA");
        }
    }

    static void modificar()
    {
        if (listalumno.Count != 0)
        {
            Console.Write("PON EL NOMBRE DEL ALUMNO QUE DESEAS MODIFICAR DE LA LISTA: ");
            string nombre = Console.ReadLine().ToUpper();

            bool encontrado = false;
            for (int i = 0; i < listalumno.Count; i++)
            {
                if (listalumno[i].NOMBRE == nombre)
                {
                    encontrado = true;
                    Console.WriteLine("¿QUE DATO DESEAS MODIFICAR DEL ALUMNO?");
                    Console.WriteLine("1. NOMBRE");
                    Console.WriteLine("2. APELLIDO");
                    Console.WriteLine("3. EDAD");
                    int op = Convert.ToInt32(Console.ReadLine());
                    switch (op)
                    {
                        case 1:
                            Console.Write("NOMBRE NUEVO: ");
                            listalumno[i].NOMBRE = Console.ReadLine().ToUpper();
                            break;
                        case 2:
                            Console.Write("APELLIDO NUEVO: ");
                            listalumno[i].APELLIDO = Console.ReadLine().ToUpper();
                            break;
                        case 3:
                            Console.Write("EDAD NUEVA: ");
                            listalumno[i].EDAD = Convert.ToInt32(Console.ReadLine());
                            break;
                        default:
                            Console.WriteLine("OPCIÓN INVÁLIDA");
                            break;
                    }
                    GuardarXML();
                    break;
                }
            }
            if (!encontrado)
                Console.WriteLine("NO SE ENCONTRÓ ALUMNO CON ESE NOMBRE");
        }
        else
        {
            Console.WriteLine("LISTA VACÍA");
        }
    }

    static void mostrar()
    {
        if (listalumno.Count == 0)
        {
            Console.WriteLine("LISTA VACÍA");
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
        CargarXML();  // Cargar datos al iniciar

        int opcion = 0;
        while (opcion != 6)
        {
            Console.WriteLine("\n1-Agregar");
            Console.WriteLine("2-Eliminar");
            Console.WriteLine("3-Buscar");
            Console.WriteLine("4-Modificar");
            Console.WriteLine("5-Mostrar");
            Console.WriteLine("6-Salir");
            Console.Write("Elige una opcion (pon el numero): ");
            opcion = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            switch (opcion)
            {
                case 1: agregar(); break;
                case 2: eliminar(); break;
                case 3: buscar(); break;
                case 4: modificar(); break;
                case 5: mostrar(); break;
                case 6: Console.WriteLine("Saliendo..."); break;
                default: Console.WriteLine("Opción inválida"); break;
            }
        }
    }
}
