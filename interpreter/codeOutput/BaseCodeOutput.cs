using System;
using System.Collections.Generic;

namespace pseudocodeIde.interpreter.codeOutput
{
    public class BaseCodeOutput
    {
        private Action<string> printMethod;

        public BaseCodeOutput(Action<string> printMethod)
        {
            this.printMethod = printMethod;
        }

        protected virtual void schreibe(string msg)
        {
            this.printMethod(msg);
        }
    }
}