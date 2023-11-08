using pseudocodeIde.interpreter.logging;
using pseudocodeIde.interpreter.parser;
using pseudocodeIde.interpreter.sequences;
using System;
using System.Collections.Generic;
using static pseudocodeIde.interpreter.TokenType;

namespace pseudocodeIde.interpreter
{
    public class Parser
    {
        private const string FUNCTION_HEADER_TEMPLATE = "protected {type} {identifier}(%insideParens%) {\n";

        private LinkedList<Token> tokens;

        private CSharpCode cSharpCode = new CSharpCode();

        LinkedListNode<Token> currentToken;

        private static readonly Dictionary<TokenType, string> tokenToCSharp = new Dictionary<TokenType, string>();

        private bool isInConstructor = true;

        static Parser()
        {
            tokenToCSharp.Add(WHITESPACE, " ");
            tokenToCSharp.Add(LEFT_PAREN, "(");
            tokenToCSharp.Add(RIGHT_PAREN, ")");

            tokenToCSharp.Add(TYPE_BOOL, "bool");
            tokenToCSharp.Add(TYPE_INT, "int");
            tokenToCSharp.Add(TYPE_DOUBLE, "double");
            tokenToCSharp.Add(TYPE_CHAR, "char");
            tokenToCSharp.Add(TYPE_STRING, "string");

            tokenToCSharp.Add(VAR_ASSIGN, "=");
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
                this.addCode(this.parseToken());
            }

            this.cSharpCode.constructor += ";";

            if (!this.isInConstructor)
            {
                // function end
                this.addCode("}");
            }

            return this.cSharpCode;
        }

        private string parseToken(bool insideFunctionParens=false)
        {
            Token token = this.currentToken.Value;

            if (!insideFunctionParens)
            {
                switch (token.type)
                {
                    case FUNCTION:
                        return this.handleFunction();
                }
            }

            switch (token.type)
            {
                case IDENTIFIER:
                    return this.tryHandleVarDef();

                case NEW_LINE:
                    return ";\n";

                default:
                    if (tokenToCSharp.ContainsKey(token.type))
                    {
                        return tokenToCSharp[token.type];
                    }
                    else
                    {
                        return token.lexeme;
                    }
            }
        }

        private string tryHandleVarDef()
        {
            string output = "";

            Token currentToken = this.currentToken.Value;
            Token possibleColon = this.currentToken.Next.Value;
            Token possibleVarType = this.currentToken.Next.Next.Value;
            Token possibleVarAssign = this.currentToken.Next.Next.Next.Value;

            if (possibleColon.type == COLON)
            {
                if(this.isVarType(possibleVarType.type))
                {
                    if (this.isInConstructor)
                    {
                        this.cSharpCode.fields += $"protected {tokenToCSharp[possibleVarType.type]} _{currentToken.lexeme};\n";

                        if (possibleVarAssign.type == VAR_ASSIGN)
                        {
                            output += $"_{currentToken.lexeme}";
                        }
                    }
                    else
                    {
                        output += $"{tokenToCSharp[possibleVarType.type]} _{currentToken.lexeme}";
                    }

                    this.advance(2);
                    return output;
                }
                else
                {
                    Logger.error(possibleVarType.line, $"Expected type for variable definition, not '{possibleVarType.lexeme}'");
                }
            }

            return "_" + currentToken.lexeme;
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
            Token operationKeyword = this.currentToken.Value;

            Token possibleIdentifier = this.advance();
            Token possibleLeftParen = this.advance();

            Token possibleRightParen = this.advance();
            Token possibleColon = this.currentToken.Next.Value;
            Token possibleType = this.currentToken.Next.Value;
            Token possibleNewLine = this.currentToken.Next.Value;

            if (possibleIdentifier.type != IDENTIFIER && possibleLeftParen.type != LEFT_PAREN)
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
                else if (possibleRightParen.type != RIGHT_PAREN
                      && possibleColon.type != COLON
                      && !this.isVarType(possibleType.type)
                      && possibleNewLine.type != NEW_LINE)
                {
                    insideParens += this.parseToken(true);
                    possibleRightParen = this.advance();
                    possibleColon = this.currentToken.Next.Value;
                    possibleType = this.currentToken.Next.Value;
                    possibleNewLine = this.currentToken.Next.Value;
                }
                else if (possibleRightParen.type == RIGHT_PAREN
                      && possibleColon.type == COLON
                      && this.isVarType(possibleType.type)
                      && possibleNewLine.type == NEW_LINE)
                {
                    break;
                }
                else
                {
                    Logger.error(operationKeyword.line, "Unexpected symbols after OPERATION keyword.");
                }
            }

            if (!this.isInConstructor)
            {
                output += "}\n\n";
            }

            this.isInConstructor = false;

            return output + FUNCTION_HEADER_TEMPLATE
                .Replace("%type%", tokenToCSharp[possibleType.type])
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
