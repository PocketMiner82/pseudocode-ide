using pseudocode_ide.interpreter.sequences;
using System.Collections.Generic;

namespace pseudocode_ide.interpreter
{
    public static class Parser
    {
        public static MainSequence parseMain(List<Token> tokens)
        {
            return new MainSequence(tokens);
        }

        public static Sequence parse(List<Token> tokens)
        {
            Sequence sequence = null;

            foreach(Token token in tokens)
            {

            }

            return sequence;
        }
    }
}
