using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack_Calculator
{
    public class StackCalculator
    {
        public static double Calculate(string expression, Stack stack)
        {
            string[] tokens = expression.Split(' ');

            foreach (string token in tokens)
            {
                if (int.TryParse(token, out int number))
                {
                    stack.Push(number);
                }
                else
                {
                    int operand2 = stack.Pop();
                    int operand1 = stack.Pop();

                    switch (token)
                    {
                        case "+":
                            stack.Push(operand1 + operand2);
                            break;
                        case "-":
                            stack.Push(operand1 - operand2);
                            break;
                        case "*":
                            stack.Push(operand1 * operand2);
                            break;
                        case "/":
                            if (operand2 == 0)
                            {
                                throw new InvalidOperationException("Division by zero");
                            }
                            stack.Push(operand1 / operand2);
                            break;
                        default:
                            throw new InvalidOperationException("Invalid token: " + token);
                    }
                }
            }

            if (stack.IsEmpty())
            {
                throw new InvalidOperationException("Invalid expression");
            }

            return stack.Pop();
        }
    }
}