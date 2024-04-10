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

using pseudocodeIde.interpreter;
using ScintillaNET;
using System;
using System.Diagnostics;

namespace pseudocode_ide.interpreter.scanner
{
    // mostly from https://github.com/jacobslusser/ScintillaNET/wiki/Custom-Syntax-Highlighting
    public static class SyntaxHighlightingLexer
    {
        // all possible text styles
        public const int STYLE_DEFAULT = 0;
        public const int STYLE_KEYWORD = 1;
        public const int STYLE_IDENTIFIER = 2;
        public const int STYLE_NUMBER = 3;
        public const int STYLE_STRING = 4;
        public const int STYLE_ESCAPE = 5;
        public const int STYLE_COMMENT = 6;

        // the possible style states
        private const int STATE_UNKNOWN = 0;
        private const int STATE_IDENTIFIER = 1;
        private const int STATE_NUMBER = 2;
        private const int STATE_STRING = 3;
        private const int STATE_CHAR = 4;
        private const int STATE_COMMENT = 5;


        /// <summary>
        /// Style a portion of the content of the scintilla textbox
        /// </summary>
        /// <param name="scintilla">the scintilla textbox</param>
        /// <param name="startPos">start position of the styling</param>
        /// <param name="endPos">end position of the styling</param>
        public static void Style(Scintilla scintilla, int startPos, int endPos)
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
                char prevC = startPos == 0 ? (char)0x00 : (char)scintilla.GetCharAt(startPos - 1);
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
                        else if (char.IsDigit(c))
                        {
                            state = STATE_NUMBER;
                            goto REPROCESS;
                        }
                        else if (char.IsLetter(c))
                        {
                            state = STATE_IDENTIFIER;
                            goto REPROCESS;
                        }
                        else if (prevC == '/' && c == '/')
                        {
                            startPos = Math.Max(startPos - 1, 0);
                            scintilla.StartStyling(startPos);
                            state = STATE_COMMENT;
                            goto REPROCESS;
                        }
                        else
                        {
                            // everything else
                            scintilla.SetStyling(1, STYLE_DEFAULT);
                        }
                        break;

                    // strings or chars start/stop at " or '; they always stop when the end of the document is reached
                    case STATE_CHAR:
                    case STATE_STRING:
                        if ((((c == '"' && state == STATE_STRING) || (c == '\'' && state == STATE_CHAR)) && prevC != '\\') || IsAtEnd(startPos, endPos))
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

                    // numbers go until IsNumber is false or the end of the document is reached
                    case STATE_NUMBER:
                        if (IsNumber(c) && !IsAtEnd(startPos, endPos))
                        {
                            length++;
                        }
                        else
                        {
                            if (IsNumber(c) && IsAtEnd(startPos, endPos))
                            {
                                length++;
                            }

                            scintilla.SetStyling(length, STYLE_NUMBER);
                            length = 0;
                            state = STATE_UNKNOWN;

                            if (!IsAtEnd(startPos, endPos))
                            {
                                goto REPROCESS;
                            }
                        }
                        break;

                    // identifiers go until IsIdentifier is false or the end of the document is reached
                    case STATE_IDENTIFIER:
                        if (IsIdentifier(c, scintilla, startPos, length) && !IsAtEnd(startPos, endPos))
                        {
                            length++;
                        }
                        else
                        {
                            string identifier;
                            if (IsIdentifier(c, scintilla, startPos, length) && IsAtEnd(startPos, endPos))
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
                            // if the identifier is in the keyword list, then it will has a differnt color 
                            if (Scanner.KEYWORDS.ContainsKey(identifier))
                            {
                                style = STYLE_KEYWORD;
                            }

                            scintilla.SetStyling(length, style);
                            length = 0;
                            state = STATE_UNKNOWN;

                            if (!IsAtEnd(startPos, endPos))
                            {
                                goto REPROCESS;
                            }
                        }
                        break;

                    // comments go from start of // to the end of the line
                    case STATE_COMMENT:
                        if (c != '\n' && !IsAtEnd(startPos, endPos))
                        {
                            length++;
                        }
                        else
                        {
                            if (c != '\n' && IsAtEnd(startPos, endPos))
                            {
                                length++;
                            }

                            scintilla.SetStyling(length, STYLE_COMMENT);
                            length = 0;
                            state = STATE_UNKNOWN;

                            if (!IsAtEnd(startPos, endPos))
                            {
                                goto REPROCESS;
                            }
                        }
                        break;
                }

                startPos++;
            }
        }

        /// <summary>
        /// Check if a string is an identifier.
        /// It must be alphanumeric or be "ENDE <something>"
        /// </summary>
        /// <param name="c">current character</param>
        /// <param name="scintilla">the scintilla text box</param>
        /// <param name="startPos">start of the string to process</param>
        /// <param name="length">length of the string to process</param>
        private static bool IsIdentifier(char c, Scintilla scintilla, int startPos, int length)
        {
            return char.IsLetterOrDigit(c) || (scintilla.GetTextRange(startPos - length, length).Equals("ENDE") && c == ' ');
        }

        /// <summary>
        /// Check if a string is a number.
        /// It must be a digit (0-9), or a-f/A-F. Also can contain . (for decimals) or x (for hex)
        /// </summary>
        /// <param name="c">current character</param>
        private static bool IsNumber(char c)
        {
            return char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') || c == 'x' || c == '.';
        }

        /// <summary>
        /// End of the string to process reached?
        /// </summary>
        /// <param name="startPos">start of the string to process</param>
        /// <param name="endPos">end of the string to process</param>
        /// <returns></returns>
        private static bool IsAtEnd(int startPos, int endPos)
        {
            return startPos == endPos - 1;
        }
    }
}
