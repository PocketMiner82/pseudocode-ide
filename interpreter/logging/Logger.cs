using pseudocodeIde.interpreter.sequences;

namespace pseudocodeIde.interpreter.logging
{
    public static class Logger
    {
        public static OutputForm outputForm { get; set; }

        public static void info(string msg)
        {
            print(LogPrefix.TIMESTAMP + LogPrefix.INFO + msg);
        }

        public static void error(string msg)
        {
            print(LogPrefix.TIMESTAMP + LogPrefix.ERROR + msg);
            Interpreter.hadError = true;
        }

        public static void error(int line, string msg, string what = "")
        {
            error($"{what}line {line}: {msg}");
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
                outputForm.outputText += message + "\n";
            }
        }
    }
}
