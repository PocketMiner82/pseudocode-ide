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
        public static Stack<T> trim<T>(this Stack<T> stack, int trimCount)
        {
            if (stack.Count <= trimCount)
            {
                return stack;
            }

            return new Stack<T>(stack.ToArray().Take(trimCount).Reverse());
        }

        // from https://stackoverflow.com/questions/2641326/finding-all-positions-of-substring-in-a-larger-string-in-c-sharp, modified
        public static List<int> allIndexesOf(this string str, string value, int startAt = 0, bool wrap = true)
        {
            List<int> indexes = new List<int>();
            if (String.IsNullOrEmpty(value))
                return indexes;

            for (int index = startAt; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    break;
                indexes.Add(index);
            }

            if (wrap)
            {
                for (int index = 0; index < startAt; index += value.Length)
                {
                    index = str.IndexOf(value, index);
                    if (index == -1)
                        break;
                    indexes.Add(index);
                }
            }
            
            return indexes;
        }

        public static int getIndentationLevel(this string line)
        {
            int indentationLevel = 0;
            foreach (char c in line)
            {
                if (c == '\t')
                {
                    indentationLevel++;
                }
                else
                {
                    break;
                }
            }
            return indentationLevel;
        }
    }
}
