using System;
using System.Collections.Generic;

namespace Stack_Calculator
{
    
    public class StackCalculator
    {
        public static bool nearlyEqual(double a, double b) 
        {
            double absA = Math.Abs(a);
            double absB = Math.Abs(b);
            double diff = Math.Abs(a - b);
            double epsilon = 0.0000001;

            if (a == b) 
            {
                return true;
            } 
            else if (a == 0 || b == 0 || absA + absB < double.MinValue) 
            {
                return diff < (epsilon * double.MinValue);
            } 
            else 
            {
                return diff / (absA + absB) < epsilon;
            }
        }

        static double Add(double operand1, double operand2)
        {
            return operand1 + operand2;
        }

        static double Substract(double operand1, double operand2)
        {
            return operand1 - operand2;
        }
        
        static double Multiply(double operand1, double operand2)
        {
            return operand1 * operand2;
        }

        static double Divide(double operand1, double operand2)
        {
            if (nearlyEqual(operand2, 0))
            {
                throw new InvalidOperationException("Division by zero");
            }
            return operand1 / operand2;
        }

        static Dictionary<string, Func<double, double, double>> operands = new Dictionary<string, Func<double, double, double>>()
        {
            {"+", Add},
            {"-", Substract},
            {"*", Multiply},
            {"/", Divide}
        };


        public static double Calculate(string expression, Stack stack)
        {
            string[] tokens = expression.Split(' ');

            foreach (string token in tokens)
            {
                if (int.TryParse(token, out int number))
                {
                    stack.Push((double)number);
                }
                else
                {
                    double operand2 = stack.Pop();
                    double operand1 = stack.Pop();
                    
                    if (operands.ContainsKey(token))
                    {
                        var tmp = (operands[token]?.Invoke(operand1, operand2));
                        if (tmp is not null)
                        {
                            stack.Push((double)tmp);
                        }
                    }
                    else
                    {
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