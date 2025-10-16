using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    // Исключение разбора/валидации карты.
    public sealed class MapParseException : Exception
    {
        // Создать исключение разбора/валидации карты.
        public MapParseException(string message) : base(message) { }
    }

    // Прямоугольная карта со стенами ('#'), свободными клетками (' ' или '.'), и необязательным стартом 'S'.
    public sealed class Map
    {
        private readonly char[,] _cells;
        public int Width { get; }
        public int Height { get; }

        private Map(char[,] cells)
        {
            _cells = cells;
            Width = cells.GetLength(0);
            Height = cells.GetLength(1);
        }

        // Создать карту из текстового представления.
        public static Map FromText(string text)
        {
            if (text is null) throw new ArgumentNullException(nameof(text));
            var lines = SplitLines(text).ToList();
            if (lines.Count == 0) throw new MapParseException("Карта пуста.");

            int width = lines.Max(l => l.Length);
            int height = lines.Count;

            var cells = new char[width, height];
            for (int y = 0; y < height; y++)
            {
                var line = lines[y];
                for (int x = 0; x < width; x++)
                {
                    char ch = x < line.Length ? line[x] : ' ';
                    cells[x, y] = Normalize(ch);
                }
            }
            return new Map(cells);
        }

        // Получить символ клетки.
        public char GetCell(int x, int y)
        {
            EnsureInBounds(x, y);
            return _cells[x, y];
        }

        // Проверить, является ли клетка проходимой.
        public bool IsFree(int x, int y)
        {
            EnsureInBounds(x, y);
            var c = _cells[x, y];
            return c == ' ' || c == '.';
        }

        // Найти стартовую позицию: 'S' или первую свободную.
        public (int x, int y) FindStartOrFirstFree()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (_cells[x, y] == 'S') return (x, y);

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (IsFree(x, y)) return (x, y);

            throw new MapParseException("На карте нет стартовой позиции и нет свободных клеток.");
        }

        private static char Normalize(char ch) =>
            ch switch
            {
                '#' => '#',
                '.' => '.',
                ' ' => ' ',
                'S' => 'S',
                _ => throw new MapParseException($"Недопустимый символ карты: '{ch}'. Допустимо: '#', '.', ' ', 'S'.")
            };

        private static IEnumerable<string> SplitLines(string text)
        {
            var sb = new StringBuilder();
            foreach (var ch in text)
            {
                if (ch == '\r') continue;
                if (ch == '\n') { yield return sb.ToString(); sb.Clear(); }
                else sb.Append(ch);
            }
            yield return sb.ToString();
        }

        private void EnsureInBounds(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException($"Координаты вне карты: ({x},{y}).");
        }
    }

    // Минимальный интерфейс рендеринга для дельта-перерисовки.
    public interface IRenderer
    {
        void SetCursor(int left, int top);
        void Write(char ch);
        void DrawFull(Map map);
    }

    // Рендерер через System.Console.
    public sealed class ConsoleRenderer : IRenderer
    {
        private ConsoleRenderer() { }

        // Создать рендерер для консоли.
        public static IRenderer Create() => new ConsoleRenderer();

        public void SetCursor(int left, int top) => Console.SetCursorPosition(left, top);
        public void Write(char ch) => Console.Write(ch);

        // Полная отрисовка карты один раз при старте.
        public void DrawFull(Map map)
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var c = map.GetCell(x, y);
                    Console.Write(c == 'S' ? ' ' : c);
                }
                if (y < map.Height - 1) Console.WriteLine();
            }
        }
    }

    // Игровая логика: позиция игрока, перемещение и минимальная перерисовка.
    public sealed class Game
    {
        private readonly Map _map;
        private readonly IRenderer _renderer;

        public int PlayerX { get; private set; }
        public int PlayerY { get; private set; }

        // Создать игру.
        public Game(Map map, int startX, int startY, IRenderer renderer)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            if (!_map.IsFree(startX, startY) && _map.GetCell(startX, startY) != 'S')
                throw new MapParseException("Стартовая позиция должна быть на свободной клетке или 'S'.");

            PlayerX = startX;
            PlayerY = startY;
        }

        // Отрисовать игрока в текущей позиции.
        public void RenderPlayerAtCurrent()
        {
            _renderer.SetCursor(PlayerX, PlayerY);
            _renderer.Write('@');
        }

        // Попытаться сдвинуть игрока, перерисовать только две клетки при успехе.
        public bool TryMove(int dx, int dy)
        {
            int nx = PlayerX + dx;
            int ny = PlayerY + dy;

            if (nx < 0 || ny < 0 || nx >= _map.Width || ny >= _map.Height) return false;
            if (!_map.IsFree(nx, ny) && _map.GetCell(nx, ny) != 'S') return false;

            _renderer.SetCursor(PlayerX, PlayerY);
            char under = _map.GetCell(PlayerX, PlayerY);
            _renderer.Write(under == 'S' ? ' ' : under);

            PlayerX = nx; PlayerY = ny;
            _renderer.SetCursor(PlayerX, PlayerY);
            _renderer.Write('@');

            return true;
        }
    }
}
