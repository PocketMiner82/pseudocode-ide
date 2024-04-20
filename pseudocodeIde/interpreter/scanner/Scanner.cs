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

using pseudocode_ide.interpreter.pseudocode;
using pseudocodeIde.interpreter.logging;
using System;
using System.Collections.Generic;
using static pseudocode_ide.interpreter.pseudocode.TokenType;
using static pseudocode_ide.interpreter.pseudocode.PseudocodeKeywords;

namespace pseudocodeIde.interpreter
{
    public class Scanner
    {
        /// <summary>
        /// If '=' is used to compare values (e.g. 1=1 would be true)
        /// or if it is used to define variables (e.g. i:int = 1)
        /// </summary>
        public static bool SingleEqualIsCompareOperator { get; set; } = false;

        /// <summary>
        /// User-written Pseudocode
        /// </summary>
        private readonly string _code;

        /// <summary>
        /// The list of tokens that will be generated from the CODE.
        /// </summary>
        private readonly LinkedList<Token> _tokens = new LinkedList<Token>();

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
        /// This class converts Pseudocode to Tokens
        /// </summary>
        /// <param name="code">the Pseudocode text string</param>
        public Scanner(string code)
        {
            this._code = code;
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

            _tokens.AddLast(Token.CreateEofToken(_line));
            return _tokens;
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

            string text = _code.Substring(_start, _current - _start);

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
                AddToken(NUMBER, Convert.ToInt32(_code.Substring(_start + 2, _current - _start), 16));
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
                AddToken(NUMBER, Convert.ToInt32(_code.Substring(_start, _current - _start).Substring(2), 2));
                return;
            }

            AddToken(NUMBER, double.Parse(_code.Substring(_start, _current - _start)));
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
            string value = _code.Substring(_start + 1, _current - _start - 2);
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
            string value = _code.Substring(_start + 1, _current - _start - 2);
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

            if (_code[_current] != expected)
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
            return IsAtEnd() ? '\0' : _code[_current];
        }

        /// <summary>
        /// Peek at the next char
        /// </summary>
        /// <returns></returns>
        private char PeekNext()
        {
            return _current + 1 >= _code.Length ? '\0' : _code[_current + 1];
        }

        /// <summary>
        /// check if a char is a digit
        /// </summary>
        /// <returns></returns>
        public static bool IsDigit(char c)
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
        public static bool IsAlpha(char c)
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
        public static bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        /// <summary>
        /// Check if we reaced the end
        /// </summary>
        /// <returns></returns>
        private bool IsAtEnd()
        {
            return _current >= _code.Length;
        }

        /// <summary>
        /// Advance one and return the current char
        /// </summary>
        /// <returns></returns>
        private char Advance()
        {
            return _code[_current++];
        }

        /// <summary>
        /// Add a new token to the list
        /// </summary>
        /// <param name="type">the token type</param>
        /// <param name="literal">the literal object</param>
        private void AddToken(TokenType type, object literal = null)
        {
            string text = _code.Substring(_start, _current - _start);
            _tokens.AddLast(new Token(type, text, literal, _line));
        }
    }
}
