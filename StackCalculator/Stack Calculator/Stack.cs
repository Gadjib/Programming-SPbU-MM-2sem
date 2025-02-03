using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack_Calculator
{
    public interface Stack
    {
        void Push(int value);
        int Pop();
        bool IsEmpty();
    }
}
