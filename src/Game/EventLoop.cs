using System;

namespace Game
{
    // Цикл событий клавиатуры с инъекцией источника клавиш.
    public sealed class EventLoop
    {
        private readonly Func<ConsoleKey?> _readKey;
        private bool _running;

        public event Action? OnUp;
        public event Action? OnDown;
        public event Action? OnLeft;
        public event Action? OnRight;
        public event Action? OnQuit;

        // Создать цикл событий.
        public EventLoop(Func<ConsoleKey?> readKey)
        {
            _readKey = readKey ?? throw new ArgumentNullException(nameof(readKey));
        }

        // Запустить цикл обработки клавиш.
        public void Run()
        {
            _running = true;
            while (_running)
            {
                var key = _readKey();
                if (key is null) break;

                switch (key.Value)
                {
                    case ConsoleKey.UpArrow: OnUp?.Invoke(); break;
                    case ConsoleKey.DownArrow: OnDown?.Invoke(); break;
                    case ConsoleKey.LeftArrow: OnLeft?.Invoke(); break;
                    case ConsoleKey.RightArrow: OnRight?.Invoke(); break;
                    case ConsoleKey.Q:
                    case ConsoleKey.Escape:
                        OnQuit?.Invoke();
                        break;
                }
            }
        }

        // Остановить цикл обработки.
        public void Stop() => _running = false;
    }
}
