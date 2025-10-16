using System;
using System.Globalization;

namespace Calculator
{
    // Поддерживаемые операторы.
    public enum CalcOp { None, Add, Sub, Mul, Div }

    // Движок калькулятора
    public sealed class CalculatorEngine
    {
        private enum State { Zero, EnteringFirst, OpPending, EnteringSecond, Result }

        private State _state = State.Zero;
        private decimal _left = 0m;
        private decimal _right = 0m;
        private CalcOp _op = CalcOp.None;
        private string _typed = "";
        private bool _error = false;

        private static readonly CultureInfo Ci = CultureInfo.InvariantCulture;

        // Текущее отображаемое значение.
        public string Display { get; private set; } = "0";

        // Нажать цифру 0..9.
        public void PressDigit(char d)
        {
            if (_error) return;
            if (d is < '0' or > '9') throw new ArgumentOutOfRangeException(nameof(d));

            if (_state is State.Zero or State.Result)
            {
                ResetEntry();
                _state = State.EnteringFirst;
            }
            if (_state is State.OpPending)
            {
                ResetEntry();
                _state = State.EnteringSecond;
            }

            if (_typed == "0") _typed = "";
            _typed += d;
            UpdateDisplayFromTyped();
        }

        // Нажать точку.
        public void PressDot()
        {
            if (_error) return;

            if (_state is State.Zero or State.Result)
            {
                ResetEntry();
                _state = State.EnteringFirst;
            }
            if (_state is State.OpPending)
            {
                ResetEntry();
                _state = State.EnteringSecond;
            }

            if (!_typed.Contains('.')) _typed = (_typed.Length == 0 ? "0" : _typed) + ".";
            UpdateDisplayFromTyped();
        }

        // Нажать оператор.
        public void PressOperator(CalcOp op)
        {
            if (_error) return;
            if (op == CalcOp.None) return;

            if (_state is State.EnteringFirst || _state is State.Zero || _state is State.Result)
            {
                _left = ParseTypedOrDisplay();
                _op = op;
                _state = State.OpPending;
            }
            else if (_state == State.OpPending)
            {
                _op = op;
            }
            else if (_state == State.EnteringSecond)
            {
                _right = ParseTypedOrDisplay();
                if (!TryApply()) return;
                _op = op;
                _state = State.OpPending;
                _typed = "";
                Display = TrimZeros(_left);
            }
        }

        // Нажать '='.
        public void PressEquals()
        {
            if (_error) return;

            if (_state == State.EnteringSecond)
            {
                _right = ParseTypedOrDisplay();
                if (!TryApply()) return;
                _state = State.Result;
                _typed = "";
                Display = TrimZeros(_left);
            }
            else if (_state == State.OpPending)
            {
                _right = _left;
                if (!TryApply()) return;
                _state = State.Result;
                _typed = "";
                Display = TrimZeros(_left);
            }
        }

        // Стереть последнюю цифру.
        public void PressBackspace()
        {
            if (_error) return;

            if (_state is State.EnteringFirst or State.EnteringSecond)
            {
                if (_typed.Length > 0)
                {
                    _typed = _typed[..^1];
                    if (_typed.Length == 0) _typed = "0";
                    UpdateDisplayFromTyped();
                }
            }
        }

        // Сменить знак.
        public void PressSign()
        {
            if (_error) return;

            if (_state is State.EnteringFirst or State.EnteringSecond or State.Zero or State.Result || _state == State.OpPending && _typed.Length > 0)
            {
                if (_typed.Length == 0) _typed = "0";
                _typed = _typed.StartsWith("-") ? _typed[1..] : "-" + _typed;
                UpdateDisplayFromTyped();
            }
        }

        // Очистить всё.
        public void PressClear()
        {
            _state = State.Zero;
            _left = 0m;
            _right = 0m;
            _op = CalcOp.None;
            _typed = "";
            _error = false;
            Display = "0";
        }

        private void ResetEntry()
        {
            _typed = "";
            Display = "0";
        }

        private void UpdateDisplayFromTyped()
        {
            if (_typed == "" || _typed == "-") Display = _typed == "-" ? "-0" : "0";
            else Display = _typed;
        }

        private decimal ParseTypedOrDisplay()
        {
            var s = _typed.Length > 0 ? _typed : Display;
            if (s == "-") s = "-0";
            return decimal.Parse(s, Ci);
        }

        private bool TryApply()
        {
            try
            {
                _left = _op switch
                {
                    CalcOp.Add => _left + _right,
                    CalcOp.Sub => _left - _right,
                    CalcOp.Mul => _left * _right,
                    CalcOp.Div => _right == 0m ? throw new DivideByZeroException() : _left / _right,
                    _ => _left
                };
                return true;
            }
            catch
            {
                _error = true;
                Display = "Error";
                return false;
            }
        }

        private static string TrimZeros(decimal value)
        {
            var s = value.ToString(CultureInfo.InvariantCulture);
            if (!s.Contains('.')) return s;
            s = s.TrimEnd('0').TrimEnd('.');
            return s.Length == 0 ? "0" : s;
        }
    }
}
