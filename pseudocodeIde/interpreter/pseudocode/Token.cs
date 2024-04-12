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

namespace pseudocode_ide.interpreter.pseudocode
{
    public class Token
    {
        /// <summary>
        /// token type
        /// </summary>
        public readonly TokenType TYPE;
        /// <summary>
        /// the pseudocode string of this token
        /// </summary>
        public readonly string LEXEME;
        /// <summary>
        /// the literal object
        /// </summary>
        public readonly object LITERAL;
        /// <summary>
        /// the line of the token
        /// </summary>
        public readonly int LINE;

        /// <summary>
        /// A Token that was lexed by the scanner.
        /// </summary>
        /// <param name="type">token type</param>
        /// <param name="lexeme">the token as a string</param>
        /// <param name="literal">the token as an object</param>
        /// <param name="line">the line of the token</param>
        public Token(TokenType type, string lexeme, object literal, int line)
        {
            TYPE = type;
            LEXEME = lexeme;
            LITERAL = literal;
            LINE = line;
        }

        public override string ToString()
        {
            return TYPE + " "
                + (LEXEME.Equals("\n") || LEXEME.Equals("\r") ? "" : LEXEME) + " "
                + LITERAL;
        }

        /// <summary>
        /// Creates an end of file token
        /// </summary>
        /// <param name="line">the line of this token</param>
        public static Token CreateEofToken(int line)
        {
            return new Token(TokenType.EOF, "", null, line);
        }
    }
}
