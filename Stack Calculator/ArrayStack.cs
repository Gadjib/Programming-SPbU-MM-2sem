using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack_Calculator
{
    public class ArrayStack : Stack
    {
        private double[] array;
        private int top;

        public ArrayStack(int capacity)
        {
            array = new double[capacity];
            top = -1;
        }

        public void Push(double value)
        {
            if (top == array.Length - 1)
            {
                throw new StackOverflowException("Stack overflow");
            }

            array[++top] = value;
        }

        public double Pop()
        {
            if (top == -1)
            {
                throw new InvalidOperationException("Stack underflow");
            }

            return array[top--];
        }

        public bool IsEmpty()
        {
            return top == -1;
        }
    }
}
