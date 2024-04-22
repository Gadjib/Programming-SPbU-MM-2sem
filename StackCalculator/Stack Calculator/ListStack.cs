using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack_Calculator
{
    public class ListStack : Stack
    {
        private List<double> list;

        public ListStack(int stackSize)
        {
            list = new List<double>();
        }

        public void Push(double value)
        {
            list.Add(value);
        }

        public double Pop()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack underflow");
            }

            double value = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return value;
        }

        public bool IsEmpty()
        {
            return list.Count == 0;
        }
    }

}
