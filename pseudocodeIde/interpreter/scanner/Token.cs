using System.Collections.Generic;

namespace pseudocodeIde.interpreter
{
    public class Token
    {
        public readonly TokenType type;
        public readonly string lexeme;
        public readonly object literal;
        public readonly int line;

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public override string ToString()
        {
            return this.type + " "
                + (this.lexeme.Equals("\n") || this.lexeme.Equals("\r") ? "" : this.lexeme) + " "
                + this.literal;
        }

        public static Token eof(int line)
        {
            return new Token(TokenType.EOF, "", null, line);
        }
    }
}
