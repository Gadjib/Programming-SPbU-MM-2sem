using System;
using System.Windows.Forms;

namespace Calculator
{
    public static class Program
    {
        // Запуск приложения.
        [STAThread]
        public static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm(new CalculatorViewModel(new CalculatorEngine())));
        }
    }
}
