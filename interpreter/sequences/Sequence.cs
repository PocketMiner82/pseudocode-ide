using System.Collections.Generic;
using System.Diagnostics;

namespace pseudocode_ide.interpreter.sequences
{
    public class Sequence
    {
        public List<Variable> variables = new List<Variable>();

        public List<Sequence> sequences;

        protected int line;

        protected List<Token> tokens;


        public Sequence(List<Token> tokens, int line, bool doNotParse = false)
        {
            this.tokens = tokens;
            this.line = line;

            if (!doNotParse)
            {
                this.parse();
            }
        }

        public virtual void parse()
        {
            this.sequences = new Parser(ref this.tokens).parseFunctionTokens(out this.variables);
        }

        public virtual void execute()
        {
            foreach (Sequence sequence in this.sequences)
            {
                sequence.execute();
            }
        }
    }
}
