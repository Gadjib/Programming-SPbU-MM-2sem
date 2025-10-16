using System;
using System.Drawing;
using System.Windows.Forms;

namespace Calculator
{
    // Главное окно калькулятора (создание UI кодом, с биндингом Display).
    public sealed class MainForm : Form
    {
        private readonly CalculatorViewModel _vm;
        private readonly Label _display;

        // Создать форму.
        public MainForm(CalculatorViewModel vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));

            Text = "Calculator";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(320, 420);
            Font = new Font("Segoe UI", 11);

            _display = new Label
            {
                Dock = DockStyle.Top,
                Height = 80,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Consolas", 28, FontStyle.Regular),
                BackColor = Color.Black,
                ForeColor = Color.White,
                Padding = new Padding(12),
            };
            Controls.Add(_display);

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 5,
                Padding = new Padding(8)
            };
            for (int i = 0; i < 4; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            for (int i = 0; i < 5; i++) grid.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            Controls.Add(grid);

            AddButton(grid, 0, 0, "C", (_, __) => _vm.Clear());
            AddButton(grid, 1, 0, "±", (_, __) => _vm.Sign());
            AddButton(grid, 2, 0, "⌫", (_, __) => _vm.Backspace());
            AddButton(grid, 3, 0, "÷", (_, __) => _vm.Op(CalcOp.Div));

            AddButton(grid, 0, 1, "7", (_, __) => _vm.Digit('7'));
            AddButton(grid, 1, 1, "8", (_, __) => _vm.Digit('8'));
            AddButton(grid, 2, 1, "9", (_, __) => _vm.Digit('9'));
            AddButton(grid, 3, 1, "×", (_, __) => _vm.Op(CalcOp.Mul));

            AddButton(grid, 0, 2, "4", (_, __) => _vm.Digit('4'));
            AddButton(grid, 1, 2, "5", (_, __) => _vm.Digit('5'));
            AddButton(grid, 2, 2, "6", (_, __) => _vm.Digit('6'));
            AddButton(grid, 3, 2, "−", (_, __) => _vm.Op(CalcOp.Sub));

            AddButton(grid, 0, 3, "1", (_, __) => _vm.Digit('1'));
            AddButton(grid, 1, 3, "2", (_, __) => _vm.Digit('2'));
            AddButton(grid, 2, 3, "3", (_, __) => _vm.Digit('3'));
            AddButton(grid, 3, 3, "+", (_, __) => _vm.Op(CalcOp.Add));

            AddButton(grid, 0, 4, "0", (_, __) => _vm.Digit('0'));
            AddButton(grid, 1, 4, ".",  (_, __) => _vm.Dot());
            var eq = AddButton(grid, 2, 4, "=", (_, __) => _vm.Equals());
            grid.SetColumnSpan(eq, 2);

            _display.DataBindings.Add("Text", _vm, nameof(CalculatorViewModel.Display), true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private Button AddButton(TableLayoutPanel grid, int col, int row, string text, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                BackColor = Color.WhiteSmoke
            };
            btn.Click += onClick;
            grid.Controls.Add(btn, col, row);
            return btn;
        }
    }
}
