using pseudocode_ide.interpreter.sequences;

namespace pseudocode_ide.interpreter.log
{
    public static class Logger
    {
        public static OutputForm outputForm { get; set; }

        public static void info(string msg)
        {
            print(LogPrefix.TIMESTAMP + LogPrefix.INFO + msg + "\n");
        }

        public static void error(string msg)
        {
            print(LogPrefix.TIMESTAMP + LogPrefix.ERROR + msg + "\n");
        }

        public static void error(int line, string msg, string what = "")
        {
            error($"{what}line {line}: {msg}");
            Interpreter.hadError = true;
        }

        public static void runtimeError(RuntimeError runtimeError)
        {
            error(runtimeError.line, runtimeError.Message, "while executing ");
            Interpreter.hadRuntimeError = true;
        }

        public static void print(string message)
        {
            if (outputForm != null)
            {
                outputForm.outputText += message;
                outputForm.scrollRtbOutputToEnd();
            }
        }
    }
}
