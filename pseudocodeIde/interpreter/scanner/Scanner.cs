using pseudocodeIde.interpreter.logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static pseudocodeIde.interpreter.TokenType;

namespace pseudocodeIde.interpreter
{
    public class Scanner
    {
        public static readonly Dictionary<string, TokenType> KEYWORDS = new Dictionary<string, TokenType>();

        public static bool singleEqualIsCompareOperator { get; set; } = false;

        private readonly string code;

        private LinkedList<Token> tokens = new LinkedList<Token>();

        private int start = 0;
        private int current = 0;
        private int line = 1;


        static Scanner()
        {
            KEYWORDS.Add("WENN",            IF);
            KEYWORDS.Add("SONST",           ELSE);
            KEYWORDS.Add("ENDE WENN",       END_IF);
                                            
            KEYWORDS.Add("FALLS",           SWITCH_PREFIX);
            KEYWORDS.Add("GLEICH",          SWITCH_SUFFIX);
            KEYWORDS.Add("ENDE FALLS",      END_SWITCH);

            KEYWORDS.Add("SOLANGE",         WHILE);
            KEYWORDS.Add("ENDE SOLANGE",    END_WHILE);
            KEYWORDS.Add("WIEDERHOLE",      DO);

            KEYWORDS.Add("FÜR",             FOR);
            KEYWORDS.Add("BIS",             FOR_TO);
            KEYWORDS.Add("SCHRITT",         FOR_STEP);
            KEYWORDS.Add("IN",              FOR_IN);
            KEYWORDS.Add("ENDE FÜR",        END_FOR);

            KEYWORDS.Add("ABBRUCH",         BREAK);
                                            
            KEYWORDS.Add("OPERATION",       FUNCTION);
            KEYWORDS.Add("RÜCKGABE",        RETURN);

            KEYWORDS.Add("wahr",            TRUE);
            KEYWORDS.Add("true",            TRUE);
            KEYWORDS.Add("falsch",          FALSE);
            KEYWORDS.Add("false",           FALSE);

            KEYWORDS.Add("UND",             AND);
            KEYWORDS.Add("ODER",            OR);

            KEYWORDS.Add("Boolean",         TYPE_BOOL);
            KEYWORDS.Add("boolean",         TYPE_BOOL);
            KEYWORDS.Add("bool",            TYPE_BOOL);
                                            
            KEYWORDS.Add("GZ",              TYPE_INT);
            KEYWORDS.Add("Integer",         TYPE_INT);
            KEYWORDS.Add("int",             TYPE_INT);
                                            
            KEYWORDS.Add("FKZ",             TYPE_DOUBLE);
            KEYWORDS.Add("Real",            TYPE_DOUBLE);
            KEYWORDS.Add("double",          TYPE_DOUBLE);
                                            
            KEYWORDS.Add("Zeichen",         TYPE_CHAR);
            KEYWORDS.Add("char",            TYPE_CHAR);
                                            
            KEYWORDS.Add("Text",            TYPE_STRING);
            KEYWORDS.Add("String",          TYPE_STRING);
            KEYWORDS.Add("string",          TYPE_STRING);

            KEYWORDS.Add("Liste",           TYPE_LIST);
            KEYWORDS.Add("NEU",             NEW);
                                            
            KEYWORDS.Add("NICHTS",          NULL);
        }

        public Scanner(string code)
        {
            this.code = code;
        }

        public LinkedList<Token> scanTokens()
        {
            while (!this.isAtEnd())
            {
                if (OutputForm.runTaskCancelToken.IsCancellationRequested)
                {
                    return this.tokens;
                }

                // we are at the beginning of the next lexeme
                this.start = this.current;
                this.scanToken();
            }

            this.tokens.AddLast(Token.eof(this.line));
            return this.tokens;
        }

        private void scanToken()
        {
            char c = this.advance();
            switch (c)
            {
                // single char lexems
                case '(': this.addToken(LEFT_PAREN); break;
                case ')': this.addToken(RIGHT_PAREN); break;
                case '[': this.addToken(LEFT_BRACKET); break;
                case ']': this.addToken(RIGHT_BRACKET); break;
                case ',': this.addToken(COMMA); break;
                case '.': this.addToken(DOT); break;
                case '-': this.addToken(MINUS); break;
                case '+': this.addToken(PLUS); break;
                case ';': this.addToken(SEMICOLON); break;
                case '*': this.addToken(STAR); break;
                case '←': this.addToken(VAR_ASSIGN); break;
                case '&': this.addToken(SINGLE_AND); break;
                case '|': this.addToken(SINGLE_OR); break;

                // single or two char lexems
                case '!':
                    this.addToken(this.match('=') ? BANG_EQUAL : BANG);
                    break;
                case '=':
                    if (singleEqualIsCompareOperator)
                    {
                        this.match('=');
                        this.addToken(EQUAL);
                    }
                    else
                    {
                        this.addToken(this.match('=') ? EQUAL : VAR_ASSIGN);
                    }
                    
                    break;
                case '<':
                    if (this.match('='))
                    {
                        this.addToken(LESS_EQUAL);
                    }
                    else if (this.match('-'))
                    {
                        this.addToken(VAR_ASSIGN);
                    }
                    else
                    {
                        this.addToken(LESS);
                    }
                    break;
                case '>':
                    this.addToken(this.match('=') ? GREATER_EQUAL : GREATER);
                    break;
                case ':':
                    this.addToken(this.match('=') ? VAR_ASSIGN : COLON);
                    break;

                // allow comments with '//'
                case '/':
                    if (this.match('/'))
                    {
                        // a comment is until the end of a line
                        while (this.peek() != '\n' && !this.isAtEnd())
                        {
                            this.advance();
                        }
                    }
                    else
                    {
                        this.addToken(SLASH);
                    }
                    break;

                case ' ':
                case '\r':
                case '\t':
                case '\n':
                    this.handleWhitspace(c == '\n');
                    break;

                case '"': this.handleString(); break;
                case '\'': this.handleChar(); break;

                default:
                    try
                    {
                        if (this.isDigit(c))
                        {
                            this.handleNumber();
                        }
                        else if (this.isAlpha(c))
                        {
                            this.handleIdentifier();
                        }
                        else
                        {
                            Logger.error(this.line, $"Unerwartetes Zeichen: '{c}'.");
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.error(this.line, $"Unerwartete Zeichen nach '{c}'. {e.GetType().Name}: {e.Message}");
                    }
                    break;
            }
        }

        private void handleWhitspace(bool isNewLine)
        {
            if (isNewLine)
            {
                this.addToken(NEW_LINE);
                this.line++;
            }

            if (!this.isAtEnd())
            {
                switch (this.peek())
                {
                    // Ignore whitespace
                    case ' ':
                    case '\r':
                    case '\t':
                    case '\n':
                        this.start = this.current;
                        this.handleWhitspace(this.advance() == '\n');
                        return;
                }
            }

            if (!isNewLine)
            {
                //this.addToken(WHITESPACE);
            }
        }

        private void handleIdentifier(bool textEmpty = true)
        {
            while (this.isAlphaNumeric(this.peek()))
            {
                this.advance();
            }

            string text = this.code.Substring(this.start, this.current - this.start);

            // allow something like "ENDE WENN"; when text is ENDE and the next char is a space, allow the space in the identifier
            if (textEmpty && text.Equals("ENDE") && (this.isAtEnd() || this.peek() == ' ' || this.peek() == '\t') )
            {
                if (!this.isAtEnd())
                {
                    this.advance();
                }
                this.handleIdentifier(false);
                return;
            }

            TokenType type;
            try
            {
                type = KEYWORDS[text];
            }
            catch (KeyNotFoundException)
            {
                if (!textEmpty)
                {
                    Logger.error(this.line, "Nach 'ENDE' muss die zu schließende Anweisung stehen.");
                    return;
                }

                type = IDENTIFIER;
            }

            this.addToken(type);
        }

        private void handleNumber()
        {
            while (this.isDigit(this.peek()))
            {
                this.advance();
            }

            // look for a decimal
            if (this.peek() == '.' && this.isDigit(this.peekNext()))
            {
                // consume the '.'
                this.advance();

                while (this.isDigit(this.peek()))
                {
                    this.advance();
                }
            }
            else if (this.peek() == 'x' && this.isHexDigit(this.peekNext()))
            {
                // consume the 'x'
                this.advance();

                while (this.isHexDigit(this.peek()))
                {
                    this.advance();
                }
                // start + 2 because the '0x' must be removed
                this.addToken(NUMBER, Convert.ToInt32(this.code.Substring(this.start + 2, this.current - this.start), 16));
                return;
            }
            else if (this.peek() == 'b' && this.isBinaryDigit(this.peekNext()))
            {
                // consume the 'b'
                this.advance();

                while (this.isBinaryDigit(this.peek()))
                {
                    this.advance();
                }
                // start + 2 because the '0b' must be removed
                this.addToken(NUMBER, Convert.ToInt32(this.code.Substring(this.start, this.current - this.start).Substring(2), 2));
                return;
            }

            this.addToken(NUMBER, double.Parse(this.code.Substring(this.start, this.current - this.start)));
        }

        private void handleChar()
        {
            if (this.peek() != '\'' && this.peek() != '\n' && !this.isAtEnd())
            {
                this.advance();
            }

            int i = 0;
            while (true)
            {
                // todo
                if (this.peek() != '\'' && !this.isAtEnd())
                {
                    i++;
                    this.advance();
                }
                else
                {
                    break;
                }
            }

            if (i > 0)
            {
                Logger.error(this.line, "Zu viele Zeichen im Zeichenliteral.");
                return;
            }
            else if (this.isAtEnd())
            {
                Logger.error(this.line, "Nicht abgeschlossenes Zeichenliteral.");
                return;
            }

            // the next char must be the closing "
            this.advance();

            // trim the surrounding quotes
            string value = this.code.Substring(this.start + 1, this.current - this.start - 2);
            this.addToken(CHAR, value);
        }

        private void handleString()
        {
            while (this.peek() != '"' && !this.isAtEnd())
            {
                if (this.peek() == '\n') this.line++;
                this.advance();
            }

            if (this.isAtEnd())
            {
                Logger.error(line, "Nicht abgeschlossene Zeichenkette.");
                return;
            }

            // the next char must be the closing "
            this.advance();

            // trim the surrounding quotes
            string value = this.code.Substring(this.start + 1, this.current - this.start - 2);
            this.addToken(STRING, value);
        }

        private bool match(char expected)
        {
            if (this.isAtEnd())
            {
                return false;
            }

            if (this.code[current] != expected)
            {
                return false;
            }

            // if it matches, we need to increase the current scanning position
            this.current++;
            return true;
        }

        private char peek()
        {
            if (this.isAtEnd())
            {
                return '\0';
            }
            return this.code[current];
        }

        private char peekNext()
        {
            if (this.current + 1 >= this.code.Length)
            {
                return '\0';
            }

            return this.code[current + 1];
        }

        private bool isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool isHexDigit(char c)
        {
            return isDigit(c) ||
                (c >= 'a' && c <= 'f') ||
                (c >= 'A' && c <= 'F');
        }

        private bool isBinaryDigit(char c)
        {
            return c == '0' || c == '1';
        }

        private bool isAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                    c == '_' || c == 'ß' ||
                    c == 'ä' || c == 'ö' || c == 'ü' ||
                    c == 'Ä' || c == 'Ö' || c == 'Ü';
        }

        private bool isAlphaNumeric(char c)
        {
            return this.isAlpha(c) || this.isDigit(c);
        }

        private bool isAtEnd()
        {
            return this.current >= this.code.Length;
        }

        private char advance()
        {
            return this.code[current++];
        }

        private void addToken(TokenType type, object literal = null)
        {
            string text = this.code.Substring(this.start, this.current - this.start);
            this.tokens.AddLast(new Token(type, text, literal, this.line));
        }
    }
}
