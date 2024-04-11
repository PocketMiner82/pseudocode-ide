// Pseudocode IDE - Execute Pseudocode for the German (BW) 2024 Abitur
// Copyright (C) 2024  PocketMiner82
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using pseudocodeIde.interpreter.logging;
using System;
using System.Collections.Generic;
using static pseudocodeIde.interpreter.TokenType;

namespace pseudocodeIde.interpreter
{
    public class Scanner
    {
        /// <summary>
        /// All the defined keywords in pseudocode and their corresponding token.
        /// </summary>
        public static readonly Dictionary<string, TokenType> KEYWORDS = new Dictionary<string, TokenType>();

        /// <summary>
        /// If '=' is used to compare values (e.g. 1=1 would be true)
        /// or if it is used to define variables (e.g. i:int = 1)
        /// </summary>
        public static bool SingleEqualIsCompareOperator { get; set; } = false;

        /// <summary>
        /// User-written Pseudocode
        /// </summary>
        private readonly string CODE;

        /// <summary>
        /// The list of tokens that will be generated from the CODE.
        /// </summary>
        private readonly LinkedList<Token> TOKENS = new LinkedList<Token>();

        /// <summary>
        /// start position of the current lexeme
        /// </summary>
        private int _start = 0;

        /// <summary>
        /// current position in the lexeme (relative to the whole text)
        /// </summary>
        private int _current = 0;

        /// <summary>
        /// current line of the CODE
        /// </summary>
        private int _line = 1;

        /// <summary>
        /// Add the keywords contents to the readonly static list
        /// </summary>
        static Scanner()
        {
            KEYWORDS.Add("WENN", IF);
            KEYWORDS.Add("SONST", ELSE);
            KEYWORDS.Add("ENDE WENN", END_IF);

            KEYWORDS.Add("FALLS", SWITCH_PREFIX);
            KEYWORDS.Add("GLEICH", SWITCH_SUFFIX);
            KEYWORDS.Add("ENDE FALLS", END_SWITCH);

            KEYWORDS.Add("SOLANGE", WHILE);
            KEYWORDS.Add("ENDE SOLANGE", END_WHILE);
            KEYWORDS.Add("WIEDERHOLE", DO);

            KEYWORDS.Add("FÜR", FOR);
            KEYWORDS.Add("BIS", FOR_TO);
            KEYWORDS.Add("SCHRITT", FOR_STEP);
            KEYWORDS.Add("IN", FOR_IN);
            KEYWORDS.Add("ENDE FÜR", END_FOR);

            KEYWORDS.Add("ABBRUCH", BREAK);

            KEYWORDS.Add("OPERATION", FUNCTION);
            KEYWORDS.Add("RÜCKGABE", RETURN);

            KEYWORDS.Add("wahr", TRUE);
            KEYWORDS.Add("true", TRUE);
            KEYWORDS.Add("falsch", FALSE);
            KEYWORDS.Add("false", FALSE);

            KEYWORDS.Add("UND", AND);
            KEYWORDS.Add("ODER", OR);

            KEYWORDS.Add("Boolean", TYPE_BOOL);
            KEYWORDS.Add("boolean", TYPE_BOOL);
            KEYWORDS.Add("bool", TYPE_BOOL);

            KEYWORDS.Add("GZ", TYPE_INT);
            KEYWORDS.Add("Integer", TYPE_INT);
            KEYWORDS.Add("int", TYPE_INT);

            KEYWORDS.Add("FKZ", TYPE_DOUBLE);
            KEYWORDS.Add("Real", TYPE_DOUBLE);
            KEYWORDS.Add("double", TYPE_DOUBLE);

            KEYWORDS.Add("Zeichen", TYPE_CHAR);
            KEYWORDS.Add("char", TYPE_CHAR);

            KEYWORDS.Add("Text", TYPE_STRING);
            KEYWORDS.Add("String", TYPE_STRING);
            KEYWORDS.Add("string", TYPE_STRING);

            KEYWORDS.Add("Liste", TYPE_LIST);
            KEYWORDS.Add("NEU", NEW);

            KEYWORDS.Add("NICHTS", NULL);

            KEYWORDS.Add("schreibe", IDENTIFIER);
            KEYWORDS.Add("warte", IDENTIFIER);
            KEYWORDS.Add("benutzereingabe", IDENTIFIER);
        }

        /// <summary>
        /// This class converts Pseudocode to Tokens
        /// </summary>
        /// <param name="code">the Pseudocode text string</param>
        public Scanner(string code)
        {
            this.CODE = code;
        }

        /// <summary>
        /// Scan the CODE to get the tokens
        /// </summary>
        /// <returns></returns>
        public LinkedList<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                // we are at the beginning of the next lexeme
                _start = _current;
                ScanToken();
            }

            TOKENS.AddLast(Token.CreateEofToken(_line));
            return TOKENS;
        }

        /// <summary>
        /// Scan for a token
        /// </summary>
        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                // single char lexems
                case '(': AddToken(LEFT_PAREN); break;
                case ')': AddToken(RIGHT_PAREN); break;
                case '[': AddToken(LEFT_BRACKET); break;
                case ']': AddToken(RIGHT_BRACKET); break;
                case ',': AddToken(COMMA); break;
                case '.': AddToken(DOT); break;
                case '-': AddToken(MINUS); break;
                case '+': AddToken(PLUS); break;
                case ';': AddToken(SEMICOLON); break;
                case '*': AddToken(STAR); break;
                case '←': AddToken(VAR_ASSIGN); break;
                case '&': AddToken(SINGLE_AND); break;
                case '|': AddToken(SINGLE_OR); break;
                case '#': AddToken(HASH); break;

                // single or two char lexems
                case '!':
                    AddToken(Match('=') ? BANG_EQUAL : BANG);
                    break;
                case '=':
                    if (SingleEqualIsCompareOperator)
                    {
                        Match('=');
                        AddToken(EQUAL);
                    }
                    else
                    {
                        AddToken(Match('=') ? EQUAL : VAR_ASSIGN);
                    }

                    break;
                case '<':
                    if (Match('='))
                    {
                        AddToken(LESS_EQUAL);
                    }
                    else if (Match('-'))
                    {
                        AddToken(VAR_ASSIGN);
                    }
                    else
                    {
                        AddToken(LESS);
                    }
                    break;
                case '>':
                    AddToken(Match('=') ? GREATER_EQUAL : GREATER);
                    break;
                case ':':
                    AddToken(Match('=') ? VAR_ASSIGN : COLON);
                    break;

                // allow comments with '//'
                case '/':
                    if (Match('/'))
                    {
                        // a comment is until the end of a line
                        while (Peek() != '\n' && !IsAtEnd())
                        {
                            Advance();
                        }
                    }
                    else
                    {
                        AddToken(SLASH);
                    }
                    break;

                case ' ':
                case '\r':
                case '\t':
                case '\n':
                    HandleWhitspace(c == '\n');
                    break;

                case '"': HandleString(); break;
                case '\'': HandleChar(); break;

                default:
                    try
                    {
                        if (IsDigit(c))
                        {
                            HandleNumber();
                        }
                        else if (IsAlpha(c))
                        {
                            HandleIdentifier();
                        }
                        else
                        {
                            Logger.Error(_line, $"Unerwartetes Zeichen: '{c}'.");
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error(_line, $"Unerwartete Zeichen nach '{c}'. {e.GetType().Name}: {e.Message}");
                    }
                    break;
            }
        }

        /// <summary>
        /// Handle the occurence of whitespaces
        /// </summary>
        /// <param name="isNewLine">is the whitespace a new line character?</param>
        private void HandleWhitspace(bool isNewLine)
        {
            if (isNewLine)
            {
                AddToken(NEW_LINE);
                _line++;
            }

            if (!IsAtEnd())
            {
                switch (Peek())
                {
                    // Ignore whitespace
                    case ' ':
                    case '\r':
                    case '\t':
                    case '\n':
                        _start = _current;
                        HandleWhitspace(Advance() == '\n');
                        return;
                }
            }

            // currently, whitespaces often don't influence the behavior of the parser,
            // we don't need to create a token for it
            //if (!isNewLine)
            //{
            //    this.addToken(WHITESPACE);
            //}
        }

        /// <summary>
        /// Handle the occurence of an identifier literal
        /// </summary>
        /// <param name="textEmpty">false if we are checking for text after the ENDE keyword</param>
        private void HandleIdentifier(bool textEmpty = true)
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            string text = CODE.Substring(_start, _current - _start);

            // allow something like "ENDE WENN"; when text is ENDE and the next char is a space, allow the space in the identifier
            if (textEmpty && text.Equals("ENDE") && (IsAtEnd() || Peek() == ' ' || Peek() == '\t'))
            {
                if (!IsAtEnd())
                {
                    Advance();
                }
                HandleIdentifier(false);
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
                    Logger.Error(_line, "Nach 'ENDE' muss die zu schließende Anweisung stehen.");
                    return;
                }

                type = IDENTIFIER;
            }

            AddToken(type);
        }

        /// <summary>
        /// Handle the occurence of a number literal
        /// </summary>
        private void HandleNumber()
        {
            while (IsDigit(Peek()))
            {
                Advance();
            }

            // look for a decimal
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                // consume the '.'
                Advance();

                while (IsDigit(Peek()))
                {
                    Advance();
                }
            }
            else if (Peek() == 'x' && IsHexDigit(PeekNext()))
            {
                // consume the 'x'
                Advance();

                while (IsHexDigit(Peek()))
                {
                    Advance();
                }
                // start + 2 because the '0x' must be removed
                AddToken(NUMBER, Convert.ToInt32(CODE.Substring(_start + 2, _current - _start), 16));
                return;
            }
            else if (Peek() == 'b' && IsBinaryDigit(PeekNext()))
            {
                // consume the 'b'
                Advance();

                while (IsBinaryDigit(Peek()))
                {
                    Advance();
                }
                // start + 2 because the '0b' must be removed
                AddToken(NUMBER, Convert.ToInt32(CODE.Substring(_start, _current - _start).Substring(2), 2));
                return;
            }

            AddToken(NUMBER, double.Parse(CODE.Substring(_start, _current - _start)));
        }

        /// <summary>
        /// Handle the occurence of a char literal
        /// </summary>
        private void HandleChar()
        {
            if (Peek() != '\'' && Peek() != '\n' && !IsAtEnd())
            {
                Advance();
            }

            int i = 0;
            while (true)
            {
                // todo
                if (Peek() != '\'' && !IsAtEnd())
                {
                    i++;
                    Advance();
                }
                else
                {
                    break;
                }
            }

            if (i > 0)
            {
                Logger.Error(_line, "Zu viele Zeichen im Zeichenliteral.");
                return;
            }
            else if (IsAtEnd())
            {
                Logger.Error(_line, "Nicht abgeschlossenes Zeichenliteral.");
                return;
            }

            // the next char must be the closing "
            Advance();

            // trim the surrounding quotes
            string value = CODE.Substring(_start + 1, _current - _start - 2);
            AddToken(CHAR, value);
        }

        /// <summary>
        /// Handle occurence of a string literal
        /// </summary>
        private void HandleString()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n')
                {
                    _line++;
                }

                Advance();
            }

            if (IsAtEnd())
            {
                Logger.Error(_line, "Nicht abgeschlossene Zeichenkette.");
                return;
            }

            // the next char must be the closing "
            Advance();

            // trim the surrounding quotes
            string value = CODE.Substring(_start + 1, _current - _start - 2);
            AddToken(STRING, value);
        }

        /// <summary>
        /// Check if the current char matches the expected char
        /// </summary>
        /// <param name="expected">the expected char</param>
        /// <returns></returns>
        private bool Match(char expected)
        {
            if (IsAtEnd())
            {
                return false;
            }

            if (CODE[_current] != expected)
            {
                return false;
            }

            // if it matches, we need to increase the current scanning position
            _current++;
            return true;
        }

        /// <summary>
        /// Peek at the current char
        /// </summary>
        /// <returns></returns>
        private char Peek()
        {
            return IsAtEnd() ? '\0' : CODE[_current];
        }

        /// <summary>
        /// Peek at the next char
        /// </summary>
        /// <returns></returns>
        private char PeekNext()
        {
            return _current + 1 >= CODE.Length ? '\0' : CODE[_current + 1];
        }

        /// <summary>
        /// check if a char is a digit
        /// </summary>
        /// <returns></returns>
        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        /// <summary>
        /// Check if a char is a hex digit
        /// </summary>
        /// <returns></returns>
        private bool IsHexDigit(char c)
        {
            return IsDigit(c) ||
                (c >= 'a' && c <= 'f') ||
                (c >= 'A' && c <= 'F');
        }

        /// <summary>
        /// Check if a char is a binary digit
        /// </summary>
        /// <returns></returns>
        private bool IsBinaryDigit(char c)
        {
            return c == '0' || c == '1';
        }

        /// <summary>
        /// Check if a char is in the (German) alphabet
        /// </summary>
        /// <returns></returns>
        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                    c == '_' || c == 'ß' ||
                    c == 'ä' || c == 'ö' || c == 'ü' ||
                    c == 'Ä' || c == 'Ö' || c == 'Ü';
        }

        /// <summary>
        /// Check if a char is alphanumeric
        /// </summary>
        /// <returns></returns>
        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        /// <summary>
        /// Check if we reaced the end
        /// </summary>
        /// <returns></returns>
        private bool IsAtEnd()
        {
            return _current >= CODE.Length;
        }

        /// <summary>
        /// Advance one and return the current char
        /// </summary>
        /// <returns></returns>
        private char Advance()
        {
            return CODE[_current++];
        }

        /// <summary>
        /// Add a new token to the list
        /// </summary>
        /// <param name="type">the token type</param>
        /// <param name="literal">the literal object</param>
        private void AddToken(TokenType type, object literal = null)
        {
            string text = CODE.Substring(_start, _current - _start);
            TOKENS.AddLast(new Token(type, text, literal, _line));
        }
    }
}
