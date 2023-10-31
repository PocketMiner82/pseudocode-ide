using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pseudocode_ide
{
    public static class Extensions
    {
        public static void Trim<T>(this Stack<T> stack, int trimCount)
        {
            if (stack.Count <= trimCount)
                return;

            stack = new
                 Stack<T>
                 (
                     stack
                         .ToArray()
                         .Take(trimCount)
                 );
        }
    }
}
