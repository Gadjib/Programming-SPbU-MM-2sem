using System;
using System.IO;
using ParseTreeLang;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Использование: ParseTree <path-to-input.txt>");
            return 1;
        }

        try
        {
            var text = File.ReadAllText(args[0]);
            var parser = new Parser(text);
            var root = parser.Parse();

            Console.WriteLine(root.Print());
            Console.WriteLine(root.Evaluate());

            return 0;
        }
        catch (ParseException ex)
        {
            Console.Error.WriteLine("Ошибка разбора: " + ex.Message);
            return 2;
        }
        catch (EvaluationException ex)
        {
            Console.Error.WriteLine("Ошибка вычисления: " + ex.Message);
            return 3;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Неожиданная ошибка: " + ex.Message);
            return 10;
        }
    }
}
