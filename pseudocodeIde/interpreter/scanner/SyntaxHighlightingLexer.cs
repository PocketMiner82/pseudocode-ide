using System;
using System.Collections.Generic;
using pseudocodeIde.interpreter;
using static pseudocodeIde.interpreter.TokenType;

namespace pseudocode_ide.interpreter.scanner
{
    public class SyntaxHighlightingLexer
    {
        public const int STYLE_DEFAULT= 0;
        public const int STYLE_KEYWORD = 1;
        public const int STYLE_IDENTIFIER = 2;
        public const int STYLE_NUMBER = 3;
        public const int STYLE_STRING = 4;
        public const int STYLE_IGNORE = 5;

        public static readonly Dictionary<TokenType, int> tokenToStyle = new Dictionary<TokenType, int>();


        static SyntaxHighlightingLexer()
        {
            //            tokenToStyle.Add(LEFT_PAREN, STYLE_DEFAULT);
            //            tokenToStyle.Add(RIGHT_PAREN, STYLE_DEFAULT);
            //            tokenToStyle.Add(LEFT_BRACKET, STYLE_DEFAULT);
            //            tokenToStyle.Add(RIGHT_BRACKET, STYLE_DEFAULT);
            //            tokenToStyle.Add(SINGLE_AND, STYLE_DEFAULT);
            //            tokenToStyle.Add(SINGLE_OR, STYLE_DEFAULT);
            //            tokenToStyle.Add(COLON, STYLE_DEFAULT);
            //            tokenToStyle.Add(COMMA, STYLE_DEFAULT);
            //            tokenToStyle.Add(DOT, STYLE_DEFAULT);
            //            tokenToStyle.Add(MINUS, STYLE_DEFAULT);
            //            tokenToStyle.Add(PLUS, STYLE_DEFAULT);
            //            tokenToStyle.Add(SEMICOLON, STYLE_DEFAULT);
            //            tokenToStyle.Add(SLASH, STYLE_DEFAULT);
            //            tokenToStyle.Add(STAR, STYLE_DEFAULT);
            //            tokenToStyle.Add(BANG, STYLE_DEFAULT);
            //            tokenToStyle.Add(BANG_EQUAL, STYLE_DEFAULT);
            //            tokenToStyle.Add(EQUAL, STYLE_DEFAULT);
            //            tokenToStyle.Add(GREATER, STYLE_DEFAULT);
            //            tokenToStyle.Add(GREATER_EQUAL, STYLE_DEFAULT);
            //            tokenToStyle.Add(LESS, STYLE_DEFAULT);
            //            tokenToStyle.Add(LESS_EQUAL, STYLE_DEFAULT);

            tokenToStyle.Add(STRING, STYLE_STRING);
            tokenToStyle.Add(CHAR, STYLE_STRING);

            tokenToStyle.Add(NUMBER, STYLE_NUMBER);

            tokenToStyle.Add(IDENTIFIER, STYLE_IDENTIFIER);

            tokenToStyle.Add(VAR_ASSIGN, STYLE_KEYWORD);
            tokenToStyle.Add(NEW, STYLE_KEYWORD);
            tokenToStyle.Add(AND, STYLE_KEYWORD);
            tokenToStyle.Add(OR, STYLE_KEYWORD);
            tokenToStyle.Add(BREAK, STYLE_KEYWORD);
            tokenToStyle.Add(DO, STYLE_KEYWORD);
            tokenToStyle.Add(END_IF, STYLE_KEYWORD);
            tokenToStyle.Add(END_SWITCH, STYLE_KEYWORD);
            tokenToStyle.Add(END_WHILE, STYLE_KEYWORD);
            tokenToStyle.Add(END_FOR, STYLE_KEYWORD);
            tokenToStyle.Add(TRUE, STYLE_KEYWORD);
            tokenToStyle.Add(FALSE, STYLE_KEYWORD);
            tokenToStyle.Add(FUNCTION, STYLE_KEYWORD);
            tokenToStyle.Add(NULL, STYLE_KEYWORD);
            tokenToStyle.Add(FOR, STYLE_KEYWORD);
            tokenToStyle.Add(FOR_TO, STYLE_KEYWORD);
            tokenToStyle.Add(FOR_STEP, STYLE_KEYWORD);
            tokenToStyle.Add(FOR_IN, STYLE_KEYWORD);
            tokenToStyle.Add(IF, STYLE_KEYWORD);
            tokenToStyle.Add(RETURN, STYLE_KEYWORD);
            tokenToStyle.Add(SWITCH_PREFIX, STYLE_KEYWORD);
            tokenToStyle.Add(SWITCH_SUFFIX, STYLE_KEYWORD);
            tokenToStyle.Add(WHILE, STYLE_KEYWORD);
            tokenToStyle.Add(ELSE, STYLE_KEYWORD);
            tokenToStyle.Add(TYPE_BOOL, STYLE_KEYWORD);
            tokenToStyle.Add(TYPE_INT, STYLE_KEYWORD);
            tokenToStyle.Add(TYPE_DOUBLE, STYLE_KEYWORD);
            tokenToStyle.Add(TYPE_CHAR, STYLE_KEYWORD);
            tokenToStyle.Add(TYPE_STRING, STYLE_KEYWORD);
            tokenToStyle.Add(TYPE_LIST, STYLE_KEYWORD);
            tokenToStyle.Add(TYPE_VOID, STYLE_KEYWORD);

            tokenToStyle.Add(EOF, STYLE_IGNORE);
            tokenToStyle.Add(NEW_LINE, STYLE_IGNORE);
        }
    }
}
