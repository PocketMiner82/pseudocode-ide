namespace pseudocodeIde.interpreter
{
    public enum TokenType
    {
        // Single-character tokens.
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE, LEFT_BRACKET, RIGHT_BRACKET,
        COLON, COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR, WHITESPACE, NEW_LINE,

        // One or two character tokens.
        BANG, BANG_EQUAL,
        VAR_ASSIGN, EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        // Literals.
        IDENTIFIER, STRING, CHAR, NUMBER,

        // Keywords.
        AND, BREAK, /*CLASS,*/ DO, END_IF, END_SWITCH, END_WHILE, END_FOR, ELSE, FALSE,
        FUNCTION, FOR, FOR_TO, FOR_STEP, FOR_IN, IF, NULL, OR, RETURN, /*SUPER,*/ SWITCH_PREFIX,
        SWITCH_SUFFIX, /*THIS,*/ TRUE, /*VAR,*/ WHILE, TYPE_BOOL, TYPE_INT, TYPE_DOUBLE, TYPE_CHAR, TYPE_STRING, TYPE_VOID,

        EOF
    }
}
