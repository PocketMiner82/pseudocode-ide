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
        public const int STYLE_IGNORE = 5;

        private const int STATE_UNKNOWN = 0;
        private const int STATE_IDENTIFIER = 1;
        private const int STATE_NUMBER = 2;
        private const int STATE_STRING = 3;
        private const int STATE_CHAR = 4;


        public static void style(Scintilla scintilla, int startPos, int endPos)
        {
            // back up to the line start
            int line = scintilla.LineFromPosition(startPos);
            startPos = scintilla.Lines[line].Position;

            int length = 0;
            int state = STATE_UNKNOWN;

            // start styling
            scintilla.StartStyling(startPos);
            while (startPos < endPos)
            {
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
                        if ((c == '"' && state == STATE_STRING) || (c == '\'' && state == STATE_CHAR))
                        {
                            length++;
                            scintilla.SetStyling(length, STYLE_STRING);
                            length = 0;
                            state = STATE_UNKNOWN;
                        }
                        else
                        {
                            length++;
                        }
                        break;

                    case STATE_NUMBER:
                        if (Char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') || c == 'x')
                        {
                            length++;
                        }
                        else
                        {
                            scintilla.SetStyling(length, STYLE_NUMBER);
                            length = 0;
                            state = STATE_UNKNOWN;
                            goto REPROCESS;
                        }
                        break;

                    case STATE_IDENTIFIER:
                        if (Char.IsLetterOrDigit(c) || (scintilla.GetTextRange(startPos - length, length).Equals("ENDE") && c == ' ') && startPos != endPos - 1)
                        {
                            length++;
                        }
                        else
                        {
                            if (startPos == endPos - 1)
                            {
                                length++;
                            }
                            int style = STYLE_IDENTIFIER;
                            string identifier = scintilla.GetTextRange(startPos - length, length);
                            if (Scanner.KEYWORDS.ContainsKey(identifier))
                                style = STYLE_KEYWORD;

                            scintilla.SetStyling(length, style);
                            length = 0;
                            state = STATE_UNKNOWN;
                            goto REPROCESS;
                        }
                        break;
                }

                startPos++;
            }
        }
    }
}
