using pseudocode_ide.interpreter.log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pseudocode_ide.interpreter.sequences
{
    public class PrintSequence : FunctionCallSequence
    {
        public PrintSequence(List<Token> argumentTokens, int line) : base(argumentTokens, line) { }

        public override void execute()
        {
            if (variables.Count == 0)
            {
                throw new RuntimeError(line, "schreibe needs at least one argument");
            }

            string text = "";
            foreach (object var in variables)
            {
                text += var.ToString();
            }

            Logger.print(text);
        }
    }
}
