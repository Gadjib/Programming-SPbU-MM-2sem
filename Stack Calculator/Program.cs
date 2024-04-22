using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack_Calculator;

class Program
{
    static void Main() 
    {
        int stackSize = 100;
        ArrayStack stack = new ArrayStack(stackSize);
        string str = "1 2 + 0 /";
        try
        {
            double answer = StackCalculator.Calculate(str, stack);
            Console.WriteLine(answer);    
        }
        catch (InvalidOperationException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}