using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Calculator
{
    // ViewModel с INotifyPropertyChanged для биндинга Display.
    public sealed class CalculatorViewModel : INotifyPropertyChanged
    {
        private readonly CalculatorEngine _engine;

        public event PropertyChangedEventHandler? PropertyChanged;

        // Текущее значение для отображения.
        public string Display => _engine.Display;

        // Создать VM.
        public CalculatorViewModel(CalculatorEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        // Обработать нажатие цифры.
        public void Digit(char d) { _engine.PressDigit(d); Notify(); }

        // Обработать точку.
        public void Dot() { _engine.PressDot(); Notify(); }

        // Обработать оператор.
        public void Op(CalcOp op) { _engine.PressOperator(op); Notify(); }

        // Равно.
        public void Equals() { _engine.PressEquals(); Notify(); }

        // Стереть.
        public void Backspace() { _engine.PressBackspace(); Notify(); }

        // Смена знака.
        public void Sign() { _engine.PressSign(); Notify(); }

        // Очистить.
        public void Clear() { _engine.PressClear(); Notify(); }

        private void Notify([CallerMemberName] string? _ = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Display)));
    }
}
