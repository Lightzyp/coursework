using AdventureGame;
using System;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            var game = new Game(width: 25, height: 21);

            while (true)
            {
                Console.Clear();
                Console.WriteLine(game.Render());

                if (game.Won || game.Lost)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press Q to quit.");
                    var endKey = Console.ReadKey(true).Key;
                    if (endKey == ConsoleKey.Q || endKey == ConsoleKey.Escape) break;
                    continue;
                }

                var key = Console.ReadKey(true).Key;
                int dx = 0, dy = 0;

                switch (key)
                {
                    case ConsoleKey.UpArrow: dy = -1; break;
                    case ConsoleKey.DownArrow: dy = 1; break;
                    case ConsoleKey.LeftArrow: dx = -1; break;
                    case ConsoleKey.RightArrow: dx = 1; break;
                    case ConsoleKey.W: dy = -1; break;
                    case ConsoleKey.S: dy = 1; break;
                    case ConsoleKey.A: dx = -1; break;
                    case ConsoleKey.D: dx = 1; break;
                    case ConsoleKey.Q:
                    case ConsoleKey.Escape:
                        return;
                }

                if (dx != 0 || dy != 0) game.TryMove(dx, dy);
            }
        }
    }
}