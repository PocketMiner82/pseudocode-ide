using System;
using System.Diagnostics;
using pseudocodeIde.interpreter;
using ScintillaNET;

namespace pseudocode_ide.interpreter.scanner
{
    // mostly from https://github.com/jacobslusser/ScintillaNET/wiki/Custom-Syntax-Highlighting
    public static class SyntaxHighlightingLexer
    {
        public const int STYLE_DEFAULT= 0;
        public const int STYLE_KEYWORD = 1;
        public const int STYLE_IDENTIFIER = 2;
        public const int STYLE_NUMBER = 3;
        public const int STYLE_STRING = 4;
        public const int STYLE_ESCAPE = 5;

        private const int STATE_UNKNOWN = 0;
        private const int STATE_IDENTIFIER = 1;
        private const int STATE_NUMBER = 2;
        private const int STATE_STRING = 3;
        private const int STATE_CHAR = 4;

        private static int startPos;
        private static int endPos;


        public static void style(Scintilla scintilla, int _startPos, int _endPos)
        {
            startPos = _startPos;
            endPos = _endPos;

            // back up to the line start
            int line = scintilla.LineFromPosition(startPos);
            startPos = scintilla.Lines[line].Position;

            int length = 0;
            int state = STATE_UNKNOWN;

            // start styling
            scintilla.StartStyling(startPos);
            while (startPos < endPos)
            {
                char prevC = startPos == 0 ? (char)scintilla.GetCharAt(startPos) : (char)scintilla.GetCharAt(startPos - 1);
                char c = (char)scintilla.GetCharAt(startPos);

            REPROCESS:
                switch (state)
                {
                    case STATE_UNKNOWN:
                        if (c == '"' || c == '\'')
                        {
                            // start of "string"
                            scintilla.SetStyling(1, STYLE_STRING);
                            state = c == '"' ? STATE_STRING : STATE_CHAR;
                        }
                        else if (Char.IsDigit(c))
                        {
                            state = STATE_NUMBER;
                            goto REPROCESS;
                        }
                        else if (Char.IsLetter(c))
                        {
                            state = STATE_IDENTIFIER;
                            goto REPROCESS;
                        }
                        else
                        {
                            // everything else
                            scintilla.SetStyling(1, STYLE_DEFAULT);
                        }
                        break;

                    case STATE_CHAR:
                    case STATE_STRING:
                        if ((((c == '"' && state == STATE_STRING) || (c == '\'' && state == STATE_CHAR)) && prevC != '\\') || isAtEnd())
                        {
                            length++;
                            scintilla.SetStyling(length, STYLE_STRING);
                            length = 0;
                            state = STATE_UNKNOWN;
                        }
                        else
                        {
                            length++;

                            if (prevC == '\\')
                            {
                                length -= 2;
                                scintilla.SetStyling(length, STYLE_STRING);
                                scintilla.SetStyling(2, STYLE_ESCAPE);
                                length = 0;
                            }
                        }
                        break;

                    case STATE_NUMBER:
                        if (isNumber(c) && !isAtEnd())
                        {
                            length++;
                        }
                        else
                        {
                            if (isNumber(c) && isAtEnd())
                            {
                                length++;
                            }

                            scintilla.SetStyling(length, STYLE_NUMBER);
                            length = 0;
                            state = STATE_UNKNOWN;

                            if (!isAtEnd())
                            {
                                goto REPROCESS;
                            }
                        }
                        break;

                    case STATE_IDENTIFIER:
                        if (isIdentifier(c, scintilla, startPos, length) && !isAtEnd())
                        {
                            length++;
                        }
                        else
                        {
                            string identifier;
                            if (isIdentifier(c, scintilla, startPos, length) && isAtEnd())
                            {
                                length++;
                                identifier = scintilla.GetTextRange(startPos + 1 - length, length);
                            }
                            else
                            {
                                identifier = scintilla.GetTextRange(startPos - length, length);
                            }

                            Debug.WriteLine($"'{identifier}'");

                            int style = STYLE_IDENTIFIER;
                            if (Scanner.KEYWORDS.ContainsKey(identifier))
                                style = STYLE_KEYWORD;

                            scintilla.SetStyling(length, style);
                            length = 0;
                            state = STATE_UNKNOWN;

                            if (!isAtEnd())
                            {
                                goto REPROCESS;
                            }
                        }
                        break;
                }

                startPos++;
            }
        }

        private static bool isIdentifier(char c, Scintilla scintilla, int startPos, int length)
        {
            return Char.IsLetterOrDigit(c) || (scintilla.GetTextRange(startPos - length, length).Equals("ENDE") && c == ' ');
        }

        private static bool isNumber(char c)
        {
            return Char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') || c == 'x' || c == '.';
        }

        private static bool isAtEnd()
        {
            return startPos == endPos - 1;
        }
    }
}
