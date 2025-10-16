using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Game;

namespace Game.Tests
{
    // Юнит-тесты игровой логики и карты с фейковым рендером и вводом.
    [TestClass]
    public class UnitTests
    {
        private sealed class FakeRenderer : IRenderer
        {
            public readonly List<(int x, int y, char ch)> Writes = new();
            private int _x, _y;

            public void SetCursor(int left, int top) { _x = left; _y = top; }
            public void Write(char ch) { Writes.Add((_x, _y, ch)); }
            public void DrawFull(Map map) { }
        }

        // Разобрать карту и найти старт.
        [TestMethod]
        public void Map_Parses_And_Start_Found()
        {
            var text = """
            #######
            #S .. #
            # ### #
            #     #
            #######
            """;
            var map = Map.FromText(text);
            var (x, y) = map.FindStartOrFirstFree();
            Assert.AreEqual(1, x);
            Assert.AreEqual(1, y);
            Assert.IsTrue(map.IsFree(2, 1));
            Assert.IsFalse(map.IsFree(0, 0));
        }

        // Перемещение рисует только дельту.
        [TestMethod]
        public void Game_Renders_Delta_On_Move()
        {
            var text = """
            #######
            #S .. #
            # ### #
            #     #
            #######
            """;
            var map = Map.FromText(text);
            var rnd = new FakeRenderer();
            var (sx, sy) = map.FindStartOrFirstFree();
            var game = new Game(map, sx, sy, rnd);

            game.RenderPlayerAtCurrent();
            var ok = game.TryMove(1, 0);
            Assert.IsTrue(ok);
            var blocked = game.TryMove(-1, -1);
            Assert.IsFalse(blocked);

            Assert.IsTrue(rnd.Writes.Count >= 3);
            Assert.AreEqual('@', rnd.Writes[0].ch);
            Assert.AreEqual('@', rnd.Writes[^1].ch);
        }

        // Инъекция клавиш управляет игрой.
        [TestMethod]
        public void EventLoop_Drives_Game_With_Injection()
        {
            var text = """
            #######
            #S .. #
            #     #
            #     #
            #######
            """;
            var map = Map.FromText(text);
            var rnd = new FakeRenderer();
            var (sx, sy) = map.FindStartOrFirstFree();
            var game = new Game(map, sx, sy, rnd);
            game.RenderPlayerAtCurrent();

            var keys = new Queue<ConsoleKey>(new[]
            {
                ConsoleKey.RightArrow, ConsoleKey.RightArrow, ConsoleKey.DownArrow, ConsoleKey.Escape
            });

            ConsoleKey? Reader() => keys.Count == 0 ? null : keys.Dequeue();

            var loop = new EventLoop(Reader);
            loop.OnRight += () => game.TryMove(1, 0);
            loop.OnDown += () => game.TryMove(0, 1);
            loop.OnQuit += () => loop.Stop();

            loop.Run();

            Assert.AreEqual(sx + 2, game.PlayerX);
            Assert.AreEqual(sy + 1, game.PlayerY);
        }


        // Недопустимый символ карты вызывает исключение.
        [TestMethod]
        public void Map_Invalid_Symbol_Throws()
        {
            var bad = "##\n#X";
            Assert.ThrowsException<MapParseException>(() => Map.FromText(bad));
        }
    }
}
