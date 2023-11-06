using System;

namespace pseudocode_ide.interpreter.sequences
{
    public class RuntimeError : Exception
    {
        public int line { get; set; }

        public RuntimeError(int line, string message) : base(message)
        {
            this.line = line;
        }
    }
}
