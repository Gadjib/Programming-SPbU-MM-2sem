using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack_Calculator
{
    public class ArrayStack : Stack
    {
        private int[] array;
        private int top;

        public ArrayStack(int capacity)
        {
            array = new int[capacity];
            top = -1;
        }

        public void Push(int value)
        {
            if (top == array.Length - 1)
            {
                throw new StackOverflowException("Stack overflow");
            }

            array[++top] = value;
        }

        public int Pop()
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
