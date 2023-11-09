using pseudocodeIde.interpreter.logging;
using pseudocodeIde.interpreter.parser;
using pseudocodeIde.interpreter.sequences;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static pseudocodeIde.interpreter.TokenType;

namespace pseudocodeIde.interpreter
{
    public class Parser
    {
        private const string FUNCTION_HEADER_TEMPLATE = "private %type% %identifier%(%insideParens%) {";

        private static readonly Dictionary<TokenType, string> TOKEN_TO_CSHARP = new Dictionary<TokenType, string>();

        private static readonly List<char> NO_SEMICOLON_AFTER = new List<char>();

        private LinkedList<Token> tokens;

        private CSharpCode cSharpCode = new CSharpCode();

        LinkedListNode<Token> currentToken;

        private bool isInConstructor = true;

        private string currentVarIdentifier;

        private int forVarCount = -1;

        static Parser()
        {
            TOKEN_TO_CSHARP.Add(TYPE_BOOL, "bool");
            TOKEN_TO_CSHARP.Add(TYPE_INT, "int");
            TOKEN_TO_CSHARP.Add(TYPE_DOUBLE, "double");
            TOKEN_TO_CSHARP.Add(TYPE_CHAR, "char");
            TOKEN_TO_CSHARP.Add(TYPE_STRING, "string");
            TOKEN_TO_CSHARP.Add(TYPE_VOID, "void");

            TOKEN_TO_CSHARP.Add(END_IF, "}");
            TOKEN_TO_CSHARP.Add(END_FOR, "}");
            TOKEN_TO_CSHARP.Add(END_WHILE, "}");
            TOKEN_TO_CSHARP.Add(END_SWITCH, "}");

            TOKEN_TO_CSHARP.Add(IF, "if");
            TOKEN_TO_CSHARP.Add(ELSE, "} else {");

            TOKEN_TO_CSHARP.Add(WHILE, "while");

            TOKEN_TO_CSHARP.Add(AND, "&&");
            TOKEN_TO_CSHARP.Add(OR, "||");

            TOKEN_TO_CSHARP.Add(TRUE, "true");
            TOKEN_TO_CSHARP.Add(FALSE, "false");

            TOKEN_TO_CSHARP.Add(BREAK, "break");

            TOKEN_TO_CSHARP.Add(RETURN, "return ");

            TOKEN_TO_CSHARP.Add(VAR_ASSIGN, "=");

            TOKEN_TO_CSHARP.Add(EQUAL, "==");


            NO_SEMICOLON_AFTER.Add('}');
            NO_SEMICOLON_AFTER.Add('{');
            NO_SEMICOLON_AFTER.Add('\n');
            NO_SEMICOLON_AFTER.Add('\r');
            NO_SEMICOLON_AFTER.Add(';');
            NO_SEMICOLON_AFTER.Add(default(char));
        }

        public Parser(LinkedList<Token> tokens)
        {
            this.tokens = tokens;

            for (int i = 0; i < 3; i++)
            {
                this.tokens.AddLast(Token.eof(this.tokens.Last.Value.line));
            }
        }

        public CSharpCode parseTokens()
        {
            while (!this.isAtEnd())
            {
                if (OutputForm.runTaskCancelToken.IsCancellationRequested)
                {
                    return this.cSharpCode;
                }

                // we are at the beginning of the next token
                this.advance();

                this.addCode(this.parseToken(this.isInConstructor
                                           ? this.cSharpCode.constructor.LastOrDefault()
                                           : this.cSharpCode.methods.LastOrDefault()));
            }

            this.cSharpCode.constructor += (NO_SEMICOLON_AFTER.Contains(this.cSharpCode.constructor.LastOrDefault()) ? "" : ";");

            if (!this.isInConstructor)
            {
                // function end
                this.addCode((NO_SEMICOLON_AFTER.Contains(this.cSharpCode.methods.LastOrDefault()) ? "" : ";") + "\n}\n");
            }

            return this.cSharpCode;
        }

        private string parseToken(char prevChar, bool ignoreSpecialCases=false, bool isInForLoopVarDef = false)
        {
            Token token = this.currentToken.Value;

            if (!ignoreSpecialCases)
            {
                switch (token.type)
                {
                    case IF:
                    case WHILE:
                        return this.handleSimpleHeader();
                    case FUNCTION:
                        return this.handleFunction();
                    case SWITCH_PREFIX:
                        return this.handleSwitchCase();
                    case DO:
                        return this.handleDoWhile();
                    case FOR:
                        return this.handleFor();
                }
            }

            switch (token.type)
            {
                case IDENTIFIER:
                    return this.tryHandleVarDef(ignoreSpecialCases, isInForLoopVarDef);

                case NEW_LINE:
                    return (NO_SEMICOLON_AFTER.Contains(prevChar) ? "" : ";") + "\n";

                default:
                    if (TOKEN_TO_CSHARP.ContainsKey(token.type))
                    {
                        return TOKEN_TO_CSHARP[token.type];
                    }
                    else
                    {
                        return token.lexeme;
                    }
            }
        }

        private string handleFor()
        {
            string output = "";

            Token currentToken;

            do
            {
                this.advance();
                output += this.parseToken(output.LastOrDefault(), true, true);
                currentToken = this.peek();
            }
            while (!this.isAtEnd() && currentToken.type != NEW_LINE && currentToken.type != FOR_TO && currentToken.type != FOR_IN);

            if (this.isAtEnd() || currentToken.type == NEW_LINE)
            {
                Logger.error(currentToken.line, "Expected IN or BIS in FÜR definition before end of line.");
                return "";
            }

            // skip the to/in
            this.advance();

            if (currentToken.type == FOR_TO)
            {
                output += $"; {this.currentVarIdentifier} != ((";

                do
                {
                    this.advance();
                    output += this.parseToken(output.LastOrDefault(), true);
                    currentToken = this.peek();
                }
                while (!this.isAtEnd() && currentToken.type != NEW_LINE && currentToken.type != FOR_STEP);

                if (this.isAtEnd() || currentToken.type == NEW_LINE)
                {
                    Logger.error(currentToken.line, "Expected SCHRITT in FÜR definition before end of line.");
                    return "";
                }

                // skip the step
                this.advance();

                output += $") + forStep{++this.forVarCount}); {this.currentVarIdentifier} += forStep{this.forVarCount}";

                string step = "";
                do
                {
                    this.advance();
                    step += this.parseToken(output.LastOrDefault(), true);
                    currentToken = this.peek();
                }
                while (!this.isAtEnd() && currentToken.type != NEW_LINE);

                output = $"var forStep{this.forVarCount} = {step};\nfor ({output}) {{";
            }
            else // currentToken.type == FOR_IN
            {
                output += " in ";

                do
                {
                    this.advance();
                    output += this.parseToken(output.LastOrDefault(), true);
                    currentToken = this.peek();
                }
                while (!this.isAtEnd() && currentToken.type != NEW_LINE);

                output = $"foreach ({output}) {{";

                if (output.StartsWith($"foreach (var forEachVar{this.forVarCount}"))
                {
                    output += $"\n{this.currentVarIdentifier} = forEachVar{this.forVarCount};";
                }
            }

            return output;
        }

        private string handleSimpleHeader()
        {
            string output = "";

            Token currentToken = this.currentToken.Value;
            string keyword = TOKEN_TO_CSHARP[currentToken.type];

            do
            {
                this.advance();
                output += this.parseToken(output.LastOrDefault(), true);
                currentToken = this.peek();
            }
            while (!this.isAtEnd() && currentToken.type != NEW_LINE);

            return $"{keyword} ({output}) {{";
        }

        private string handleDoWhile()
        {
            string output = "";
            Token currentToken;

            do
            {
                this.advance();
                output += this.parseToken(output.LastOrDefault(), true);
                currentToken = this.peek();
            }
            while (!this.isAtEnd() && currentToken.type != WHILE);

            if (this.isAtEnd())
            {
                Logger.error(currentToken.line, "Expected SOLANGE before end of file.");
                return "";
            }

            // skip while keyword
            this.advance();

            output += "} while (";

            do
            {
                this.advance();
                output += this.parseToken(output.LastOrDefault(), true);
                currentToken = this.peek();
            }
            while (!this.isAtEnd() && currentToken.type != NEW_LINE);

            return "do {" + output + ")";
        }

        private string handleSwitchCase()
        {
            string output = "";
            Token switchKeyword = this.currentToken.Value;

            Token currentToken = this.advance();

            do
            {
                output += this.parseToken(output.LastOrDefault(), true);
                currentToken = this.advance();

                if (this.isAtEnd() || currentToken.type == NEW_LINE)
                {
                    Logger.error(switchKeyword.line, "Expected GLEICH before end of line.");
                    return "";
                }
            }
            while (currentToken.type != SWITCH_SUFFIX);

            output = "switch (" + output + ") {\n";

            if (this.peek().type == NEW_LINE)
            {
                currentToken = this.advance();
            }

            string currentLine = "";
            bool firstCase = true;
            while (currentToken.type != END_SWITCH)
            {
                currentToken = this.advance();

                if (this.isAtEnd())
                {
                    Logger.error(currentToken.line, "Expected ENDE FALLS before end of file.");
                    return "";
                }

                switch (currentToken.type)
                {
                    case ELSE:
                        if (currentLine.Equals("") && this.peek().type == COLON)
                        {
                            this.advance();

                            if (!firstCase)
                            {
                                output += "break;\n\n";
                            }
                            firstCase = false;
                            output += "default: ";
                        }
                        else
                        {
                            currentLine += this.parseToken(currentLine.LastOrDefault());
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
                        output += currentLine + this.parseToken(currentLine.LastOrDefault());
                        currentLine = "";
                        break;
                    default:
                        currentLine += this.parseToken(currentLine.LastOrDefault());
                        break;
                }
            }

            if (!firstCase)
            {
                output += "break;\n\n";
            }

            return output + "}";
        }

        private string tryHandleVarDef(bool insideFunctionParens, bool isInForLoopVarDef = false)
        {
            string output = "";

            Token identifier = this.currentToken.Value;
            Token possibleColon = this.currentToken.Next.Value;
            Token possibleVarType = this.currentToken.Next.Next.Value;
            Token possibleVarAssign = this.currentToken.Next.Next.Next.Value;

            if (possibleColon.type == COLON)
            {
                if(this.isVarType(possibleVarType.type))
                {
                    if (this.isInConstructor && !insideFunctionParens)
                    {
                        this.cSharpCode.fields += $"private {TOKEN_TO_CSHARP[possibleVarType.type]} _{identifier.lexeme};\n";

                        if (possibleVarAssign.type == VAR_ASSIGN)
                        {
                            output += $"_{identifier.lexeme}";
                        }
                    }
                    else
                    {
                        output += $"{TOKEN_TO_CSHARP[possibleVarType.type]} _{identifier.lexeme}";
                    }

                    this.advance(2);
                    return output;
                }
                else
                {
                    Logger.error(possibleVarType.line, $"Expected type for variable definition, not '{possibleVarType.lexeme}'");
                }
            }
            // special case for for loop
            else if (possibleColon.type == VAR_ASSIGN && isInForLoopVarDef)
            {
                this.currentVarIdentifier = "_" + identifier.lexeme;
                return "var " + "_" + identifier.lexeme;
            }
            // special case for foreach loop
            else if (isInForLoopVarDef)
            {
                this.currentVarIdentifier = "_" + identifier.lexeme;
                return "var forEachVar" + ++this.forVarCount;
            }

            return "_" + identifier.lexeme;
        }

        private bool isVarType(TokenType type)
        {
            switch (type)
            {
                case TYPE_BOOL:
                case TYPE_INT:
                case TYPE_DOUBLE:
                case TYPE_CHAR:
                case TYPE_STRING:
                    return true;
                default:
                    return false;
            }
        }

        private string handleFunction()
        {
            string output = "";
            string insideParens = "";
            TokenType type = TYPE_VOID;

            Token operationKeyword = this.currentToken.Value;

            Token possibleIdentifier = this.advance();
            Token possibleLeftParen = this.advance();

            Token possibleRightParen = this.advance();
            Token possibleColon = this.currentToken.Next.Value;
            Token possibleType = this.currentToken.Next.Next.Value;

            if (possibleIdentifier.type != IDENTIFIER || possibleLeftParen.type != LEFT_PAREN)
            {
                Logger.error(operationKeyword.line, "Unexpected symbols after OPERATION keyword.");
                return "";
            }


            while (!this.isAtEnd())
            {
                if (possibleRightParen.type == NEW_LINE)
                {
                    Logger.error(operationKeyword.line, "New line before OPERATION definition end.");
                    return "\n";
                }
                else if (possibleRightParen.type == RIGHT_PAREN
                      && possibleColon.type == COLON
                      && this.isVarType(possibleType.type))
                {
                    type = possibleType.type;
                    this.advance(2);
                    break;
                }
                else if (possibleRightParen.type == RIGHT_PAREN)
                {
                    break;
                }
                else if (possibleRightParen.type != RIGHT_PAREN)
                {
                    insideParens += this.parseToken(insideParens.LastOrDefault(), true);
                    possibleRightParen = this.advance();
                    possibleColon = this.currentToken.Next.Value;
                    possibleType = this.currentToken.Next.Next.Value;
                }
                else
                {
                    Logger.error(operationKeyword.line, "Unexpected symbols in operation line.");
                    return "";
                }
            }

            if (!this.isInConstructor)
            {
                output += (NO_SEMICOLON_AFTER.Contains(output.LastOrDefault()) ? "" : ";")  + "}\n\n";
            }

            this.isInConstructor = false;

            return output + FUNCTION_HEADER_TEMPLATE
                .Replace("%type%", TOKEN_TO_CSHARP[type])
                .Replace("%identifier%", "_" + possibleIdentifier.lexeme)
                .Replace("%insideParens%", insideParens);
        }

        private Token advance()
        {
            this.currentToken = this.peekLinkedList();

            return this.currentToken.Value;
        }

        private Token advance(int count)
        {
            Token current = null;
            for (int i = 0; i < count; i++)
            {
                current = this.advance();
            }

            return current;
        }

        private Token peek()
        {
            return this.peekLinkedList().Value;
        }

        private LinkedListNode<Token> peekLinkedList()
        {
            return this.currentToken == null
                ? this.tokens.First
                : this.currentToken.Next;
        }

        private bool isAtEnd()
        {
            return this.currentToken != null && this.currentToken.Value.type == EOF;
        }

        private void addCode(string code)
        {
            if (this.isInConstructor)
            {
                this.cSharpCode.constructor += code;
            }
            else
            {
                this.cSharpCode.methods += code;
            }
        }
    }
}
