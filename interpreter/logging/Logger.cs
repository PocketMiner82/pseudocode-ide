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

        public static void error(int line, string msg)
        {
            error($"Zeile {line}: {msg}");
        }

        public static void print(string message, bool newLine = true)
        {
            if (outputForm != null)
            {
                outputForm.outputText += message + (newLine ? "\n" : "");
            }
        }
    }
}
