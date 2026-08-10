using System;

class Program
{
    public class Nodo
    {
        public int Valor;
        public Nodo Izquierda;
        public Nodo Derecha;

        public Nodo(int valor)
        {
            Valor = valor;
            Izquierda = null;
            Derecha = null;
        }
    }

    public static Nodo Insertar(Nodo raiz, int valor)
    {
        if (raiz == null)
        {
            return new Nodo(valor);
        }

        if (valor < raiz.Valor)
        {
            raiz.Izquierda = Insertar(raiz.Izquierda, valor);
        }
        else if (valor > raiz.Valor)
        {
            raiz.Derecha = Insertar(raiz.Derecha, valor);
        }

        return raiz;
    }

    public static void ImprimirInorder(Nodo raiz)
    {
        if (raiz != null)
        {
            ImprimirInorder(raiz.Izquierda);
            Console.Write(raiz.Valor + " ");
            ImprimirInorder(raiz.Derecha);
        }
    }

    static void Main(string[] args)
    {
        Nodo arbol = new Nodo(50);

        Insertar(arbol, 30);
        Insertar(arbol, 70);
        Insertar(arbol, 20);
        Insertar(arbol, 40);
        Insertar(arbol, 60);
        Insertar(arbol, 80);

        Console.WriteLine("Árbol ordenado (Inorder):");
        ImprimirInorder(arbol);
    }
}
