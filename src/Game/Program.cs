using System;
using System.IO;

namespace Game
{
    // Точка входа консольного приложения.
    public static class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Использование: Game <map.txt>");
                return 1;
            }

            try
            {
                var mapText = File.ReadAllText(args[0]);
                var map = Map.FromText(mapText);

                var start = map.FindStartOrFirstFree();
                var renderer = ConsoleRenderer.Create();

                Console.CursorVisible = false;
                Console.Clear();
                renderer.DrawFull(map);

                var game = new Game(map, start.x, start.y, renderer);

                var loop = new EventLoop(ConsoleKeyReader.ReadKeyOrNull);
                loop.OnUp += () => game.TryMove(0, -1);
                loop.OnDown += () => game.TryMove(0, 1);
                loop.OnLeft += () => game.TryMove(-1, 0);
                loop.OnRight += () => game.TryMove(1, 0);
                loop.OnQuit += () => loop.Stop();

                game.RenderPlayerAtCurrent();
                loop.Run();
                return 0;
            }
            catch (MapParseException ex)
            {
                Console.Error.WriteLine("Ошибка карты: " + ex.Message);
                return 2;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Неожиданная ошибка: " + ex.Message);
                return 10;
            }
            finally
            {
                try { Console.CursorVisible = true; } catch { }
            }
        }
    }

    internal static class ConsoleKeyReader
    {
        public static ConsoleKey? ReadKeyOrNull()
        {
            var key = Console.ReadKey(intercept: true).Key;
            return key;
        }
    }
}
