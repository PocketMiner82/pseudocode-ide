using System.Collections.Generic;

namespace pseudocode_ide.interpreter
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

        public static readonly string[] OPERATOR_SMALLER = { "<" };
        public static readonly string[] OPERATOR_SMALLER_OR_EQUAL = { "<=" };
        public static readonly string[] OPERATOR_GREATER = { ">" };
        public static readonly string[] OPERATOR_GREATER_OR_EQUAL = { ">=" };
        public static readonly string[] OPERATOR_EQUAL = { null, "==" };

        public static void setEqualOperatorForCompare(bool val)
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
