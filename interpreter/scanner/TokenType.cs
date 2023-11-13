namespace pseudocodeIde.interpreter
{
    public enum TokenType
    {
        // Single-character tokens.
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACKET, RIGHT_BRACKET,
        SINGLE_AND, SINGLE_OR, COLON, COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR, NEW_LINE,

        // One or two character tokens.
        BANG, BANG_EQUAL,
        VAR_ASSIGN, EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        // Literals.
        IDENTIFIER, STRING, CHAR, NUMBER,

        // Keywords.
        NEW, AND, OR, BREAK, DO, END_IF, END_SWITCH, END_WHILE, END_FOR, TRUE, FALSE, FUNCTION, NULL,
        FOR, FOR_TO, FOR_STEP, FOR_IN, IF, RETURN, SWITCH_PREFIX, SWITCH_SUFFIX, WHILE, ELSE,
        TYPE_BOOL, TYPE_INT, TYPE_DOUBLE, TYPE_CHAR, TYPE_STRING, TYPE_LIST, TYPE_VOID,

        EOF
    }
}
