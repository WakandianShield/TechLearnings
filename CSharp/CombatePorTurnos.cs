using System;

public class Personaje
{
    public int Vida { get; set; }
    public int Ataque { get; set; }
    public int Velocidad { get; set; }
    public int Nivel { get; set; }

    public Personaje(int vida, int ataque, int velocidad, int nivel)
    {
        Vida = vida;
        Ataque = ataque;
        Velocidad = velocidad;
        Nivel = nivel;
    }

    public virtual void Atacar(Personaje objetivo)
    {
        Console.WriteLine($"{GetType().Name} está atacando...");
        objetivo.Vida -= Ataque;
        Console.WriteLine($"Ha hecho {Ataque} de daño. Vida restante del objetivo: {objetivo.Vida}\n");
    }

    public bool DefinirTurno(Personaje otro)
    {
        return Velocidad >= otro.Velocidad;
    }
}

public class Jugador : Personaje
{
    public int PoderMagico { get; set; }
    public int Exp { get; set; }

    public Jugador(int vida, int ataque, int velocidad, int nivel, int poderMagico, int exp)
        : base(vida, ataque, velocidad, nivel)
    {
        PoderMagico = poderMagico;
        Exp = exp;
    }

    public void UsarPoderMagico()
    {
        if (PoderMagico > 0)
        {
            Console.WriteLine("Jugador está curándose...");
            Vida += 20;
            PoderMagico--;
            Console.WriteLine($"Se ha curado 20 puntos. Su vida actual es de {Vida}. PM restante: {PoderMagico}\n");
        }
        else
        {
            Console.WriteLine("No hay poder disponible.\n");
        }
    }

    public void SetNivel(int nuevoNivel)
    {
        Nivel = nuevoNivel;
        Ataque = 15 + 5 * (nuevoNivel - 1);
        Vida = 100 + 10 * (nuevoNivel - 1);
        Console.WriteLine($"Nuevo nivel del Jugador: {Nivel}, Ataque: {Ataque}, Vida: {Vida}");
    }

    public void GanarExp(int cantidad)
    {
        Exp += cantidad;
        Console.WriteLine($"Has ganado {cantidad} de experiencia. Exp total: {Exp}");
    }
}

public class Enemigo : Personaje
{
    public int Puntaje { get; set; }

    public Enemigo(int vida, int ataque, int velocidad, int nivel)
        : base(vida, ataque, velocidad, nivel) { }

    public void SetNivel(int nuevoNivel)
    {
        Nivel = nuevoNivel;
        Ataque = 5 + 5 * (nuevoNivel - 1);
        Vida = 80 + 10 * (nuevoNivel - 1);
        Console.WriteLine($"-ENEMIGO NIVEL {Nivel}- \n{Vida} puntos de vida \n{Ataque} de ataque\n");
    }
}

class Program
{
    static void Main()
    {
        var jugador = new Jugador(100, 15, 10, 1, 3, 0);
        var enemigo = new Enemigo(80, 5, 8, 1);

        Console.WriteLine("¡Bienvenido al combate por turnos!");

        while (jugador.Vida > 0)
        {
            Console.WriteLine("\n--- Turno del Jugador ---");
            Console.WriteLine("1. Atacar");
            Console.WriteLine("2. Curarse");
            Console.WriteLine("3. Salir");
            Console.Write("Elige una opción: ");

            switch (Console.ReadLine())
            {
                case "1":
                    jugador.Atacar(enemigo);
                    break;
                case "2":
                    jugador.UsarPoderMagico();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Opción no válida. Intenta de nuevo.");
                    continue;
            }

            if (enemigo.Vida <= 0)
            {
                Console.WriteLine("¡Has derrotado al enemigo!");
                jugador.GanarExp(10); 
                jugador.SetNivel(jugador.Nivel + 1);
                enemigo.SetNivel(enemigo.Nivel + 1);
                Console.WriteLine("Un nuevo enemigo ha aparecido.");
                enemigo.Vida = enemigo.Nivel * 20 + 30; 
                enemigo.Ataque = enemigo.Nivel * 5 + 5;
            }

            if (enemigo.Vida > 0)
            {
                Console.WriteLine("\n--- Turno del Enemigo ---");
                enemigo.Atacar(jugador);

                if (jugador.Vida <= 0)
                {
                    Console.WriteLine("¡Has sido derrotado!");
                    break;
                }
            }

            Console.WriteLine($"\nEstado actual: Jugador - Vida: {jugador.Vida}, PM: {jugador.PoderMagico}, Nivel: {jugador.Nivel}, Exp: {jugador.Exp}");
            Console.WriteLine($"Enemigo - Vida: {enemigo.Vida}, Nivel: {enemigo.Nivel}");
        }

        Console.WriteLine("Juego terminado. ¡Gracias por jugar!");
    }
}
