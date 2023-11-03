using pseudocode_ide.interpreter.log;
using System.Collections.Generic;
using static pseudocode_ide.interpreter.TokenType;

namespace pseudocode_ide.interpreter
{
    public class Scanner
    {
        public static bool singleEqualIsCompareOperator { get; set; } = false;

        private readonly string code;
        private List<Token> tokens = new List<Token>();

        private int start = 0;
        private int current = 0;
        private int line = 1;

        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>();

        static Scanner()
        {
            keywords.Add("WENN",            IF);
            keywords.Add("SONST",           ELSE);
            keywords.Add("ENDE WENN",       END_IF);
                                            
            keywords.Add("FALLS",           SWITCH_PREFIX);
            keywords.Add("GLEICH",          SWITCH_SUFFIX);
            keywords.Add("ENDE FALLS",      END_SWITCH);

            keywords.Add("SOLANGE",         WHILE);
            keywords.Add("ENDE SOLANGE",    END_WHILE);
            keywords.Add("WIEDERHOLE",      DO);

            keywords.Add("FÜR",             FOR);
            keywords.Add("BIS",             FOR_TO);
            keywords.Add("SCHRITT",         FOR_STEP);
            keywords.Add("IN",              FOR_IN);
            keywords.Add("ENDE FÜR",        END_FOR);

            keywords.Add("ABBRUCH",         BREAK);
                                            
            keywords.Add("OPERATION",       FUNCTION);
            keywords.Add("RÜCKGABE",        RETURN);

            keywords.Add("wahr",            TRUE);
            keywords.Add("true",            TRUE);
            keywords.Add("falsch",          FALSE);
            keywords.Add("false",           FALSE);

            keywords.Add("UND",             AND);
            keywords.Add("ODER",            OR);

            keywords.Add("Boolean",         TYPE_BOOL);
            keywords.Add("boolean",         TYPE_BOOL);
            keywords.Add("bool",            TYPE_BOOL);
                                            
            keywords.Add("GZ",              TYPE_INT);
            keywords.Add("Integer",         TYPE_INT);
            keywords.Add("int",             TYPE_INT);
                                            
            keywords.Add("FKZ",             TYPE_DOUBLE);
            keywords.Add("Real",            TYPE_DOUBLE);
            keywords.Add("double",          TYPE_DOUBLE);
                                            
            keywords.Add("Zeichen",         TYPE_CHAR);
            keywords.Add("char",            TYPE_CHAR);
                                            
            keywords.Add("Text",            TYPE_STRING);
            keywords.Add("String",          TYPE_STRING);
            keywords.Add("string",          TYPE_STRING);
                                            
            keywords.Add("NICHTS",          NULL);
        }

        public Scanner(string code)
        {
            this.code = code;
        }

        public List<Token> scanTokens()
        {
            while (!this.isAtEnd())
            {
                if (OutputForm.runTaskCancelToken.IsCancellationRequested)
                {
                    return tokens;
                }

                // we are at the beginning of the next lexeme
                this.start = this.current;
                this.scanToken();
            }

            this.tokens.Add(new Token(EOF, "", null, line));
            return tokens;
        }

        private void scanToken()
        {
            char c = this.advance();
            switch (c)
            {
                // single char lexems
                case '(': this.addToken(LEFT_PAREN); break;
                case ')': this.addToken(RIGHT_PAREN); break;
                //case '{': this.addToken(LEFT_BRACE); break;
                //case '}': this.addToken(RIGHT_BRACE); break;
                //case '[': this.addToken(LEFT_BRACKET); break;
                //case ']': this.addToken(RIGHT_BRACKET); break;
                case ',': this.addToken(COMMA); break;
                case '.': this.addToken(DOT); break;
                case '-': this.addToken(MINUS); break;
                case '+': this.addToken(PLUS); break;
                case ';': this.addToken(SEMICOLON); break;
                case '*': this.addToken(STAR); break;
                case '←': this.addToken(VAR_ASSIGN); break;

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
                        Logger.error(line, "Unexpected character.");
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
                    // Ignore more than one whitespace
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
                this.addToken(WHITESPACE);
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
            }

            TokenType type;
            try
            {
                type = keywords[text];
            }
            catch (KeyNotFoundException)
            {
                if (!textEmpty)
                {
                    Logger.error(this.line, "Expected closing type after 'ENDE'");
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
                Logger.error(line, "Too many characters in character literal.");
                return;
            }
            else if (this.isAtEnd())
            {
                Logger.error(line, "Unterminated char.");
                return;
            }

            // the next char must be the closing "
            this.advance();

            // trim the surrounding quotes
            string value = this.code.Substring(this.start + 1, this.current - this.start - 2);
            addToken(CHAR, value);
        }

        private void handleString()
        {
            while (this.peek() != '"' && !this.isAtEnd())
            {
                if (peek() == '\n') line++;
                advance();
            }

            if (this.isAtEnd())
            {
                Logger.error(line, "Unterminated string.");
                return;
            }

            // the next char must be the closing "
            advance();

            // trim the surrounding quotes
            string value = this.code.Substring(this.start + 1, this.current - this.start - 2);
            addToken(STRING, value);
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
            return isAlpha(c) || isDigit(c);
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
            this.tokens.Add(new Token(type, text, literal, this.line));
        }
    }
}
