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

namespace pseudocodeIde.interpreter
{
    /// <summary>
    /// The type of a token that was lexed
    /// </summary>
    public enum TokenType
    {
        // Single-character tokens.
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACKET, RIGHT_BRACKET, HASH,
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
