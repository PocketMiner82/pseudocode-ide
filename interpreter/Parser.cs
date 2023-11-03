using pseudocode_ide.interpreter.log;
using pseudocode_ide.interpreter.sequences;
using System;
using System.Collections.Generic;
using static pseudocode_ide.interpreter.TokenType;

namespace pseudocode_ide.interpreter
{
    public class Parser
    {
        private List<Token> tokens;

        public Parser(ref List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public MainSequence parseMain()
        {
            return new MainSequence(tokens);
        }

        public List<Sequence> parseFunctionTokens(out List<Variable> variables)
        {
            List<Sequence> sequences = new List<Sequence>();
            variables = new List<Variable>();

            while (tokens.Count > 0)
            {
                Token token = tokens[0];

                switch (token.type)
                {
                    // new function starts, we are finished.
                    case FUNCTION:
                        if (!this.isAtEnd())
                        {
                            this.advance();
                        }
                        return sequences;
                    case IDENTIFIER:
                        // todo function assigned with null
                        removeNextWhitespace();
                        if (!isAtEnd() && peek().type == LEFT_PAREN)
                        {
                            sequences.Add(handleFunctionCall());
                        }
                        break;
                    default:
                        break;
                }

                if (this.isAtEnd())
                {
                    break;
                }
                advance();
            }

            return sequences;
        }

        public List<Variable> parseArguments()
        {
            List<Variable> variables = new List<Variable>();

            while (tokens.Count > 0)
            {
                Token token = tokens[0];

                switch (token.type)
                {
                    case COMMA:
                    case WHITESPACE:
                        break;

                    case STRING:
                        variables.Add(new Variable(STRING, token.lexeme));
                        advance();
                        continue;
                    default:
                        Logger.error("Not implemented.");
                        break;
                }
                advance();
            }

            return variables;
        }

        private FunctionCallSequence handleFunctionCall()
        {
            List<Token> argumentTokens = new List<Token>();

            int line = peek().line;
            while (!this.isAtEnd())
            {
                Token token = advance();


                if (token.type == RIGHT_PAREN)
                {
                    advance();
                    break;
                }

                if (this.isAtEnd() || token.type == NEW_LINE)
                {
                    Logger.error(token.line, "Expected ) for function call");
                    break;
                }
            }

            return new FunctionCallSequence(argumentTokens, line);
        }

        private void removeNextWhitespace()
        {
            if (peek().type == WHITESPACE)
            {
                advance();
            }
        }

        private Token peek()
        {
            return tokens[1];
        }

        private Token advance()
        {
            tokens.RemoveAt(0);
            return tokens.Count > 0 ? tokens[0] : null;
        }

        private bool isAtEnd()
        {
            return tokens.Count <= 1;
        }

        public Dictionary<Token, Sequence> parseFunctions()
        {
            Dictionary<Token, Sequence> sequences = new Dictionary<Token, Sequence>();

            while (tokens.Count > 0)
            {
                Token token = tokens[0];
                // token indicates new function
                if (token.type == FUNCTION)
                {
                    removeNextWhitespace();
                    if (peek().type != IDENTIFIER)
                    {
                        Logger.error(token.line, "Expected identifier after OPERATION");
                    }

                    sequences.Add(advance(), new Sequence(tokens, token.line));
                }

                advance();
            }

            foreach (Sequence sequence in sequences.Values)
            {
                sequence.parse();
            }

            return sequences;
        }
    }
}
