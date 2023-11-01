using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pseudocode_ide
{
    public static class Extensions
    {
        public static Stack<T> Trim<T>(this Stack<T> stack, int trimCount)
        {
            if (stack.Count <= trimCount)
            {
                return stack;
            }

            return new Stack<T>(stack.ToArray().Take(trimCount).Reverse());
        }
    }
}
