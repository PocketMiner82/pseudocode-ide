using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pseudocode_ide.interpreter.sequences
{
    public class MainSequence : Sequence
    {
        public static Dictionary<Token, Sequence> functions { get; set; } = new Dictionary<Token, Sequence>();

        public MainSequence(List<Token> tokens) : base(tokens, 0)
        {
            functions = new Parser(ref tokens).parseFunctions();
        }
    }
}
