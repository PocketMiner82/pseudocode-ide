using pseudocodeIde.interpreter.logging;
using pseudocodeIde.interpreter.parser;
using pseudocodeIde.interpreter.sequences;
using System;
using System.Collections.Generic;
using static pseudocodeIde.interpreter.TokenType;

namespace pseudocodeIde.interpreter
{
    public class Parser
    {
        private LinkedList<Token> tokens;

        private CSharpCode cSharpCode = new CSharpCode();

        public Parser(LinkedList<Token> tokens)
        {
            this.tokens = tokens;
        }

        public CSharpCode parse()
        {


            this.cSharpCode.compile();
            return this.cSharpCode;
        }
    }
}
