using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pseudocode_ide.interpreter.sequences
{
    public class FunctionCallSequence : Sequence
    {
        protected string name;

        public FunctionCallSequence(List<Token> argumentTokens, int line, string name = "") : base(argumentTokens, line)
        {
            this.name = name;
        }

        public override void parse()
        {
            this.variables = new Parser(ref tokens).parseArguments();
        }
    }
}
