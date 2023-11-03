namespace pseudocode_ide.interpreter.sequences
{
    public class Variable
    {
        public TokenType type { get; set; }

        public object value { get; set; }

        public Variable(TokenType type, object value)
        {
            this.type = type;
            this.value = value;
        }
    }
}
