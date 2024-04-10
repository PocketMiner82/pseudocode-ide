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
using pseudocodeIde.interpreter.parser;
using System.Collections.Generic;
using System.Linq;
using static pseudocodeIde.interpreter.TokenType;

namespace pseudocodeIde.interpreter
{
    public class Parser
    {
        // Template for function header in C# code generation
        private const string FUNCTION_HEADER_TEMPLATE = "%visibility% %type% %identifier%(%insideParens%) {";

        // Mapping of pseudocode tokens to C# code
        private static readonly Dictionary<TokenType, string> TOKEN_TO_C_SHARP = new Dictionary<TokenType, string>();

        // Mapping of visibility tokens to C# visibility modifiers
        private static readonly Dictionary<TokenType, string> VISIBILITY = new Dictionary<TokenType, string>();

        // Characters that do not require a semicolon after them
        private static readonly List<char> NO_SEMICOLON_AFTER = new List<char>();

        // Linked list of tokens to be parsed
        private readonly LinkedList<Token> TOKENS;

        // Manager for generated C# code
        private readonly CSharpCodeManager C_SHARP_CODE = new CSharpCodeManager();

        // Current token being processed
        LinkedListNode<Token> _currentToken;

        // Flag indicating if the parser is currently in a constructor
        private bool _isInConstructor = true;

        // Identifier for the current variable being processed
        private string _currentVarIdentifier;

        // Counter for variables in 'for' loops
        private int _forVarCount = -1;

        /// <summary>
        /// Initializes static members of the class.
        /// </summary>
        static Parser()
        {
            TOKEN_TO_C_SHARP.Add(TYPE_BOOL, "bool?");
            TOKEN_TO_C_SHARP.Add(TYPE_INT, "int?");
            TOKEN_TO_C_SHARP.Add(TYPE_DOUBLE, "double?");
            TOKEN_TO_C_SHARP.Add(TYPE_CHAR, "char?");
            TOKEN_TO_C_SHARP.Add(TYPE_STRING, "string");
            TOKEN_TO_C_SHARP.Add(TYPE_VOID, "void");

            TOKEN_TO_C_SHARP.Add(END_IF, "}");
            TOKEN_TO_C_SHARP.Add(END_FOR, "}");
            TOKEN_TO_C_SHARP.Add(END_WHILE, "}");
            TOKEN_TO_C_SHARP.Add(END_SWITCH, "}");

            TOKEN_TO_C_SHARP.Add(IF, "if");
            TOKEN_TO_C_SHARP.Add(ELSE, "} else {");

            TOKEN_TO_C_SHARP.Add(WHILE, "while");

            TOKEN_TO_C_SHARP.Add(AND, "&&");
            TOKEN_TO_C_SHARP.Add(OR, "||");

            TOKEN_TO_C_SHARP.Add(TRUE, "true");
            TOKEN_TO_C_SHARP.Add(FALSE, "false");

            TOKEN_TO_C_SHARP.Add(BREAK, "break");

            TOKEN_TO_C_SHARP.Add(RETURN, "return ");

            TOKEN_TO_C_SHARP.Add(VAR_ASSIGN, "=");

            TOKEN_TO_C_SHARP.Add(EQUAL, "==");

            TOKEN_TO_C_SHARP.Add(TYPE_LIST, "Liste");
            TOKEN_TO_C_SHARP.Add(NEW, "new ");
            TOKEN_TO_C_SHARP.Add(NULL, "null");


            VISIBILITY.Add(PLUS, "public");
            VISIBILITY.Add(MINUS, "private");
            VISIBILITY.Add(HASH, "protected");


            NO_SEMICOLON_AFTER.Add('}');
            NO_SEMICOLON_AFTER.Add('{');
            NO_SEMICOLON_AFTER.Add('\n');
            NO_SEMICOLON_AFTER.Add('\r');
            NO_SEMICOLON_AFTER.Add(';');
            NO_SEMICOLON_AFTER.Add(default);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="tokens">The tokens to parse.</param>
        public Parser(LinkedList<Token> tokens)
        {
            TOKENS = tokens;

            for (int i = 0; i < 3; i++)
            {
                TOKENS.AddLast(Token.CreateEofToken(TOKENS.Last.Value.LINE));
            }
        }

        /// <summary>
        /// Parses the tokens and generate C# code.
        /// </summary>
        /// <returns>The code manager for the generated code</returns>
        public CSharpCodeManager ParseTokens()
        {
            while (!IsAtEnd())
            {
                // we are at the beginning of the next token
                Advance();

                AddCode(ParseToken(_isInConstructor
                                           ? C_SHARP_CODE.Constructor.LastOrDefault()
                                           : C_SHARP_CODE.Methods.LastOrDefault()));
            }

            C_SHARP_CODE.Constructor += (NO_SEMICOLON_AFTER.Contains(C_SHARP_CODE.Constructor.LastOrDefault()) ? "" : ";");

            if (!_isInConstructor)
            {
                // function end
                AddCode((NO_SEMICOLON_AFTER.Contains(C_SHARP_CODE.Methods.LastOrDefault()) ? "" : ";") + "\n}\n");
            }

            return C_SHARP_CODE;
        }

        /// <summary>
        /// Parses a single token based on its type and context.
        /// </summary>
        /// <param name="prevChar">The previous character.</param>
        /// <param name="ignoreSpecialCases">If set to <c>true</c>, special cases are ignored.</param>
        /// <param name="isInForLoopVarDef">If set to <c>true</c>, the token is being defined in a 'for' loop.</param>
        /// <returns>The parsed token as a c# code string.</returns>
        private string ParseToken(char prevChar, bool ignoreSpecialCases = false, bool isInForLoopVarDef = false)
        {
            Token token = _currentToken.Value;

            if (!ignoreSpecialCases)
            {
                switch (token.TYPE)
                {
                    case IF:
                    case WHILE:
                        return HandleSimpleHeader();
                    case FUNCTION:
                        return HandleFunction();
                    case SWITCH_PREFIX:
                        return HandleSwitchCase();
                    case DO:
                        return HandleDoWhile();
                    case FOR:
                        return HandleFor();
                }
            }

            switch (token.TYPE)
            {
                case IDENTIFIER:
                    return TryHandleVarDef(ignoreSpecialCases, isInForLoopVarDef);

                case NEW_LINE:
                    return (NO_SEMICOLON_AFTER.Contains(prevChar) ? "" : ";") + "\n";

                case VAR_ASSIGN:
                    string output = TOKEN_TO_C_SHARP[token.TYPE];
                    Token possibleLeftBracket = Peek();
                    if (possibleLeftBracket.TYPE == LEFT_BRACKET)
                    {
                        string arrayInitOutput = HandleArrayInit(possibleLeftBracket);

                        if (arrayInitOutput == null)
                        {
                            return "";
                        }

                        output += arrayInitOutput;
                    }
                    return output;

                default:
                    return TOKEN_TO_C_SHARP.ContainsKey(token.TYPE)
                        ? TOKEN_TO_C_SHARP[token.TYPE]
                        : token.LEXEME;
            }
        }

        /// <summary>
        /// Handles the initialization of pseudocode List.
        /// </summary>
        /// <param name="possibleLeftBracket">The token, which could be a left bracket.</param>
        /// <param name="noSemicolon">If set to <c>true</c>, no semicolon is added at the end.</param>
        /// <returns>The array initialization c# code.</returns>
        private string HandleArrayInit(Token possibleLeftBracket, bool noSemicolon = false)
        {
            Advance();
            string output = "new() {";

            while (!IsAtEnd() && Peek().TYPE != RIGHT_BRACKET)
            {
                // ignore new lines
                if (Peek().TYPE == NEW_LINE)
                {
                    Advance();
                    continue;
                }

                if (Peek().TYPE == LEFT_BRACKET)
                {
                    output += HandleArrayInit(possibleLeftBracket, true);

                    // the method advances to the next char at the end, which could be the ending bracket for this array definition
                    if (Peek().TYPE == RIGHT_BRACKET)
                    {
                        break;
                    }
                }

                output += ParseToken(Advance().LEXEME.Last(), true);
            }

            if (!IsAtEnd() && Peek().TYPE == RIGHT_BRACKET)
            {
                Advance();
                output += "}";

                if ((Peek().TYPE == NEW_LINE || _currentToken.Next.Value.TYPE == EOF) && !noSemicolon)
                {
                    output += ";";
                }
            }
            else
            {
                Logger.Error(possibleLeftBracket.LINE, $"']' erwartet, nicht '{_currentToken.Value.LEXEME}'.\n\n{output}");
                return null;
            }

            return output;
        }

        /// <summary>
        /// Handles the parsing of a 'for' loop.
        /// </summary>
        /// <returns>The 'for' loop c# code.</returns>
        private string HandleFor()
        {
            string output = "";

            Token currentToken;

            do
            {
                Advance();
                output += ParseToken(output.LastOrDefault(), true, true);
                currentToken = Peek();
            }
            while (!IsAtEnd() && currentToken.TYPE != NEW_LINE && currentToken.TYPE != FOR_TO && currentToken.TYPE != FOR_IN);

            if (IsAtEnd() || currentToken.TYPE == NEW_LINE)
            {
                Logger.Error(currentToken.LINE, "IN oder BIS in der FÜR-Definition erwartet.");
                return "";
            }

            // skip the to/in
            Advance();

            if (currentToken.TYPE == FOR_TO)
            {
                output += $"; {_currentVarIdentifier} != ((";

                do
                {
                    Advance();
                    output += ParseToken(output.LastOrDefault(), true);
                    currentToken = Peek();
                }
                while (!IsAtEnd() && currentToken.TYPE != NEW_LINE && currentToken.TYPE != FOR_STEP);

                if (IsAtEnd() || currentToken.TYPE == NEW_LINE)
                {
                    Logger.Error(currentToken.LINE, "SCHRITT in der FÜR-Definition erwartet.");
                    return "";
                }

                // skip the step
                Advance();

                output += $") + forStep{++_forVarCount}); {_currentVarIdentifier} += forStep{_forVarCount}";

                string step = "";
                do
                {
                    Advance();
                    step += ParseToken(output.LastOrDefault(), true);
                    currentToken = Peek();
                }
                while (!IsAtEnd() && currentToken.TYPE != NEW_LINE);

                output = $"var forStep{_forVarCount} = {step};\nfor ({output}) {{";
            }
            else // currentToken.type == FOR_IN
            {
                output += " in ";

                do
                {
                    Advance();
                    output += ParseToken(output.LastOrDefault(), true);
                    currentToken = Peek();
                }
                while (!IsAtEnd() && currentToken.TYPE != NEW_LINE);

                output = $"foreach ({output}) {{";

                if (output.StartsWith($"foreach (var forEachVar{_forVarCount}"))
                {
                    output += $"\n{_currentVarIdentifier} = forEachVar{_forVarCount};";
                }
            }

            return output;
        }

        /// <summary>
        /// Handles the parsing of simple control flow statements like 'WENN' and 'SOLANGE'.
        /// </summary>
        /// <returns>The control flow statement c# code.</returns>
        private string HandleSimpleHeader()
        {
            string output = "";

            Token currentToken = _currentToken.Value;
            string keyword = TOKEN_TO_C_SHARP[currentToken.TYPE];

            do
            {
                Advance();
                output += ParseToken(output.LastOrDefault(), true);
                currentToken = Peek();
            }
            while (!IsAtEnd() && currentToken.TYPE != NEW_LINE);

            return $"{keyword} ({output}) {{";
        }

        /// <summary>
        /// Handles the parsing of a 'WIEDERHOLE ... SOLANGE' loop.
        /// </summary>
        /// <returns>The do-while loop c# code.</returns>
        private string HandleDoWhile()
        {
            string output = "";
            Token currentToken;

            do
            {
                Advance();
                output += ParseToken(output.LastOrDefault());
                currentToken = Peek();
            }
            while (!IsAtEnd() && currentToken.TYPE != WHILE);

            if (IsAtEnd())
            {
                Logger.Error(currentToken.LINE, "SOLANGE vor Dateiende erwartet.");
                return "";
            }

            // skip while keyword
            Advance();

            output += "} while (";

            do
            {
                Advance();
                output += ParseToken(output.LastOrDefault(), true);
                currentToken = Peek();
            }
            while (!IsAtEnd() && currentToken.TYPE != NEW_LINE);

            return "do {" + output + ")";
        }

        /// <summary>
        /// Handles the parsing of a 'FALLS' statement.
        /// </summary>
        /// <returns>The switch statement c# code.</returns>
        private string HandleSwitchCase()
        {
            string output = "";
            Token switchKeyword = _currentToken.Value;

            Advance();
            Token currentToken;

            do
            {
                output += ParseToken(output.LastOrDefault(), true);
                currentToken = Advance();

                if (IsAtEnd() || currentToken.TYPE == NEW_LINE)
                {
                    Logger.Error(switchKeyword.LINE, "GLEICH vor Zeilenende erwartet.");
                    return "";
                }
            }
            while (currentToken.TYPE != SWITCH_SUFFIX);

            output = "switch (" + output + ") {\n";

            if (Peek().TYPE == NEW_LINE)
            {
                currentToken = Advance();
            }

            string currentLine = "";
            bool firstCase = true;
            while (currentToken.TYPE != END_SWITCH)
            {
                currentToken = Advance();

                if (IsAtEnd())
                {
                    Logger.Error(currentToken.LINE, "ENDE FALLS vor Dateiende erwartet.");
                    return "";
                }

                switch (currentToken.TYPE)
                {
                    case ELSE:
                        if (currentLine.Equals("") && Peek().TYPE == COLON)
                        {
                            Advance();

                            if (!firstCase)
                            {
                                output += "break;\n\n";
                            }
                            firstCase = false;
                            output += "default: ";
                        }
                        else
                        {
                            currentLine += ParseToken(currentLine.LastOrDefault());
                        }
                        break;
                    case COLON:
                        if (!firstCase)
                        {
                            output += "break;\n\n";
                        }
                        firstCase = false;

                        output += "case " + currentLine + ": ";
                        currentLine = "";
                        break;
                    case NEW_LINE:
                        output += currentLine + ParseToken(currentLine.LastOrDefault());
                        currentLine = "";
                        break;
                    default:
                        currentLine += ParseToken(currentLine.LastOrDefault());
                        break;
                }
            }

            if (!firstCase)
            {
                output += "break;\n\n";
            }

            return output + "}";
        }

        /// <summary>
        /// Attempts to handle variable definitions within the pseudocode.
        /// </summary>
        /// <param name="insideFunctionParens">If set to <c>true</c>, the variable is being defined inside function parentheses.</param>
        /// <param name="isInForLoopVarDef">If set to <c>true</c>, the variable is being defined in a 'FÜR' loop.</param>
        /// <returns>The variable definition c# code.</returns>
        private string TryHandleVarDef(bool insideFunctionParens, bool isInForLoopVarDef = false)
        {
            string output = "";

            Token identifier = _currentToken.Value;
            Token possibleColon = _currentToken.NextOrLast().Value;
            Token possibleVarType = _currentToken.NextOrLast().NextOrLast().Value;
            Token possibleVarAssign = _currentToken.NextOrLast().NextOrLast().NextOrLast().Value;

            if (possibleColon.TYPE == COLON)
            {
                if (isInForLoopVarDef)
                {
                    _currentVarIdentifier = "_" + identifier.LEXEME;
                }

                if (IsVarType(possibleVarType.TYPE))
                {
                    string type = TOKEN_TO_C_SHARP[possibleVarType.TYPE];
                    if (possibleVarAssign.TYPE == LESS)
                    {
                        Advance();
                        type = TryHandleTypedVarType();
                        possibleVarAssign = _currentToken.NextOrLast().NextOrLast().NextOrLast().Value;
                    }

                    if (_isInConstructor && !insideFunctionParens)
                    {
                        C_SHARP_CODE.Fields += $"private {type} _{identifier.LEXEME};\n";

                        if (possibleVarAssign.TYPE == VAR_ASSIGN)
                        {
                            output += $"_{identifier.LEXEME}";
                        }
                    }
                    else
                    {
                        output += $"{type} _{identifier.LEXEME}";
                    }

                    Advance(2);
                    return output;
                }
                else
                {
                    Logger.Error(possibleVarType.LINE, $"Für die Variablendefinition wurde die Angabe eines Typs erwartet, nicht '{possibleVarType.LEXEME}'");
                }
            }
            // special case for for loop
            else if (possibleColon.TYPE == VAR_ASSIGN && isInForLoopVarDef)
            {
                _currentVarIdentifier = "_" + identifier.LEXEME;
                return "var " + "_" + identifier.LEXEME;
            }
            // special case for foreach loop
            else if (isInForLoopVarDef)
            {
                _currentVarIdentifier = "_" + identifier.LEXEME;
                return "var forEachVar" + ++_forVarCount;
            }

            return "_" + identifier.LEXEME;
        }

        /// <summary>
        /// Tries to handle a typed variable type, including generic types.
        /// </summary>
        /// <returns>The C# code representation of the typed variable type.</returns>
        private string TryHandleTypedVarType()
        {
            string output = "";

            Token possibleVarType = _currentToken.NextOrLast().Value;
            Token possibleLessSign = _currentToken.NextOrLast().NextOrLast().Value;

            if (IsVarType(possibleVarType.TYPE))
            {
                string type = TOKEN_TO_C_SHARP[possibleVarType.TYPE];
                if (possibleLessSign.TYPE == LESS)
                {
                    output += type + "<";

                    Advance(2);
                    output += TryHandleTypedVarType();

                    Token possibleGreaterSign = _currentToken.NextOrLast().NextOrLast().Value;
                    if (possibleGreaterSign.TYPE != GREATER)
                    {
                        Logger.Error(possibleVarType.LINE, $"'>' erwartet, nicht '{possibleGreaterSign.LEXEME}'");
                        return "";
                    }

                    while (_currentToken.NextOrLast().NextOrLast().NextOrLast().Value.TYPE == GREATER)
                    {
                        // consume any more greater signs
                        Advance();
                    }

                    output += ">";
                }
                else
                {
                    return type;
                }
            }

            return output;
        }

        /// <summary>
        /// Determines whether the specified token type represents a variable type.
        /// </summary>
        /// <param name="type">The token type to check.</param>
        /// <returns><c>true</c> if the token type represents a variable type; otherwise, <c>false</c>.</returns>
        private bool IsVarType(TokenType type)
        {
            switch (type)
            {
                case TYPE_BOOL:
                case TYPE_INT:
                case TYPE_DOUBLE:
                case TYPE_CHAR:
                case TYPE_STRING:
                case TYPE_LIST:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Handles the parsing of a 'OPERATION' definition.
        /// </summary>
        /// <returns>The function definition C# code.</returns>
        private string HandleFunction()
        {
            string output = "";
            string insideParens = "";
            string visibility = "";
            TokenType type = TYPE_VOID;

            Token functionKeyword = _currentToken.Value;

            Token possibleVisibility = Advance();
            Token possibleIdentifier = possibleVisibility;
            if (Parser.VISIBILITY.ContainsKey(possibleVisibility.TYPE))
            {
                visibility = Parser.VISIBILITY[possibleIdentifier.TYPE];
                possibleIdentifier = Advance();
            }

            Token possibleLeftParen = Advance();

            Token possibleRightParen = Advance();
            Token possibleColon = _currentToken.NextOrLast().Value;
            Token possibleType = _currentToken.NextOrLast().NextOrLast().Value;

            if (possibleIdentifier.TYPE != IDENTIFIER || possibleLeftParen.TYPE != LEFT_PAREN)
            {
                Logger.Error(functionKeyword.LINE, "Unerwartete Zeichen nach dem OPERATION-Keyword.");
                return "";
            }


            while (!IsAtEnd())
            {
                if (possibleRightParen.TYPE == NEW_LINE)
                {
                    Logger.Error(functionKeyword.LINE, "Neue Zeile vor dem Ende der OPERATION-Definition.");
                    return "\n";
                }
                else if (possibleRightParen.TYPE == RIGHT_PAREN
                      && possibleColon.TYPE == COLON
                      && IsVarType(possibleType.TYPE))
                {
                    type = possibleType.TYPE;
                    Advance(2);
                    break;
                }
                else if (possibleRightParen.TYPE == RIGHT_PAREN)
                {
                    break;
                }
                else if (possibleRightParen.TYPE != RIGHT_PAREN)
                {
                    insideParens += ParseToken(insideParens.LastOrDefault(), true);
                    possibleRightParen = Advance();
                    possibleColon = _currentToken.NextOrLast().Value;
                    possibleType = _currentToken.NextOrLast().NextOrLast().Value;
                }
                else
                {
                    Logger.Error(functionKeyword.LINE, "Unerwartete Zeichen.");
                    return "";
                }
            }

            if (!_isInConstructor)
            {
                output += (NO_SEMICOLON_AFTER.Contains(output.LastOrDefault()) ? "" : ";") + "}\n\n";
            }

            _isInConstructor = false;

            return output + FUNCTION_HEADER_TEMPLATE
                .Replace("%visibility%", visibility)
                .Replace("%type%", TOKEN_TO_C_SHARP[type])
                .Replace("%identifier%", "_" + possibleIdentifier.LEXEME)
                .Replace("%insideParens%", insideParens);
        }

        /// <summary>
        /// Advances to the next token in the linked list.
        /// </summary>
        /// <returns>The next token.</returns>
        private Token Advance()
        {
            _currentToken = PeekLinkedList();

            return _currentToken.Value;
        }

        /// <summary>
        /// Advances a specified number of tokens in the linked list.
        /// </summary>
        /// <param name="count">The number of tokens to advance.</param>
        /// <returns>The token after advancing the specified number of tokens.</returns>
        private Token Advance(int count)
        {
            Token current = null;
            for (int i = 0; i < count; i++)
            {
                current = Advance();
            }

            return current;
        }

        /// <summary>
        /// Peeks at the next token in the linked list without advancing.
        /// </summary>
        /// <returns>The next token.</returns>
        private Token Peek()
        {
            return PeekLinkedList().Value;
        }

        /// <summary>
        /// Peeks at the next linked list node in the linked list without advancing.
        /// </summary>
        /// <returns>The next linked list node.</returns>
        private LinkedListNode<Token> PeekLinkedList()
        {
            return _currentToken == null
                ? TOKENS.First
                : _currentToken.NextOrLast();
        }

        /// <summary>
        /// Determines whether the parser has reached the end of the tokens.
        /// </summary>
        /// <returns><c>true</c> if the parser has reached the end of the tokens; otherwise, <c>false</c>.</returns>
        private bool IsAtEnd()
        {
            return _currentToken != null && _currentToken.Value.TYPE == EOF;
        }

        /// <summary>
        /// Adds the specified code to the appropriate section of the C# code manager.
        /// </summary>
        /// <param name="code">The code to add.</param>
        private void AddCode(string code)
        {
            if (_isInConstructor)
            {
                C_SHARP_CODE.Constructor += code;
            }
            else
            {
                C_SHARP_CODE.Methods += code;
            }
        }
    }
}
