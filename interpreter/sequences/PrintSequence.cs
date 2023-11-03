using pseudocode_ide.interpreter.log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pseudocode_ide.interpreter.sequences
{
    public class PrintSequence : Sequence
    {
        public PrintSequence(List<Token> tokens, int line) : base(tokens, line)
        {
        }

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
