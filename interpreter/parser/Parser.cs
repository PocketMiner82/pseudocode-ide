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
        private LinkedList<Token> tokens;

        private CSharpCode cSharpCode = new CSharpCode();

        LinkedListNode<Token> currentToken;
        private int line = 1;

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
                this.parseToken();
            }

            this.cSharpCode.constructor += ";";

            return this.cSharpCode;
        }

        private void parseToken()
        {
            Token token = this.currentToken.Value;

            switch (token.type)
            {
                case FUNCTION:
                    this.isInConstructor = false;
                    this.handleFunction();
                    break;

                case IDENTIFIER:
                    this.tryHandleVarDef();
                    break;

                case NEW_LINE:
                    this.line++;
                    this.addCode(";\n");
                    break;

                default:
                    if (tokenToCSharp.ContainsKey(token.type))
                    {
                        this.addCode(tokenToCSharp[token.type]);
                    }
                    else
                    {
                        this.addCode(token.lexeme);
                    }
                    
                    break;
            }
        }

        private void tryHandleVarDef()
        {
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
                            this.addCode($"_{currentToken.lexeme}");
                        }

                        this.advance();
                        this.advance();
                    }
                    else
                    {
                        this.addCode($"{tokenToCSharp[possibleVarType.type]} _{currentToken.lexeme}");

                        this.advance();
                        this.advance();
                    }
                    
                    return;
                }
                else
                {
                    Logger.error(this.line, $"Expected type for variable definition, not '{possibleVarType.lexeme}'");
                }
            }

            this.addCode("_" + currentToken.lexeme);
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

        private void handleFunction()
        {
            
        }

        private Token advance()
        {
            this.currentToken = this.peekLinkedList();

            return this.currentToken.Value;
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
