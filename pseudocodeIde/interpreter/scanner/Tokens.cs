namespace pseudocodeIde.interpreter
{
    public static class Tokens
    {
        public static readonly string[] INIT_VAR = { ":" };
        public static string[] SET_VAR { get; private set; } = { "=", "<-", ":=", "←" };

        public static readonly string[] IF = { "WENN" };
        public static readonly string[] ELSE = { "SONST" };
        public static readonly string[] END_IF = { "ENDE WENN" };

        public static readonly string[] SWITCH_PREFIX = { "FALLS" };
        public static readonly string[] SWITCH_SUFFIX = { "GLEICH" };
        public static readonly string[] CASE = { ":" };
        public static readonly string[] END_SWITCH = { "ENDE FALLS" };

        public static readonly string[] WHILE_OR_END_DO_WHILE = { "SOLANGE" };
        public static readonly string[] END_WHILE = { "ENDE SOLANGE" };
        public static readonly string[] DO_WHILE = { "WIEDERHOLE" };

        public static readonly string[] FOR = { "FÜR" };
        public static readonly string[] FOR_TO = { "BIS" };
        public static readonly string[] FOR_STEP = { "SCHRITT" };
        public static readonly string[] FOR_IN = { "IN" };
        public static readonly string[] END_FOR = { "ENDE FÜR" };

        public static readonly string[] LOOP_BREAK = { "ABBRUCH" };

        public static readonly string[] SEQUENCE = { "OPERATION" };
        public static readonly string[] END_SEQUENCE = { "RÜCKGABE" };

        public static readonly string[] OPERATOR_SMALLER = { "<" };
        public static readonly string[] OPERATOR_SMALLER_OR_EQUAL = { "<=" };
        public static readonly string[] OPERATOR_GREATER = { ">" };
        public static readonly string[] OPERATOR_GREATER_OR_EQUAL = { ">=" };
        public static readonly string[] OPERATOR_EQUAL = { null, "==" };

        public static readonly string[] TYPE_BOOL = { "Boolean", "boolean", "bool" };
        public static readonly string[] TYPE_INT = { "GZ", "Integer", "int" };
        public static readonly string[] TYPE_DOUBLE = { "FKZ", "Real", "double" };
        public static readonly string[] TYPE_CHAR = { "Zeichen", "char" };
        public static readonly string[] TYPE_STRING = { "Text", "String", "string" };
        public static readonly string[] TYPE_NULL = { "NICHTS" };

        //public static readonly string[] TYPE_LIST_PREFIX = { "Liste<", "[" };
        //public static readonly string[] TYPE_LIST_SUFFIX = { ">", "]" };

        public static void setEqualsIsCompareOperator(bool val)
        {
            if (val)
            {
                OPERATOR_EQUAL[0] = "=";
                SET_VAR[0] = null;
            }
            else
            {
                OPERATOR_EQUAL[0] = null;
                SET_VAR[0] = "=";
            }
        }
    }
}
