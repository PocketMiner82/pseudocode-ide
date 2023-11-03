using System.Collections.Generic;

namespace pseudocode_ide.interpreter.sequences
{
    public class Sequence
    {
        public List<Variable> variables = new List<Variable>();

        public List<Sequence> sequences = new List<Sequence>();

        protected int line;


        public Sequence(List<Token> tokens, int line)
        {

        }

        public virtual void execute()
        {
            foreach (Sequence sequence in sequences)
            {
                sequence.execute();
            }
        }
    }
}
