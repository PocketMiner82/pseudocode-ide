using pseudocode_ide.interpreter.log;
using System.Collections.Generic;

namespace pseudocode_ide.interpreter
{
    public class Interpreter
    {
        public static bool hadError { get; set; } = false;

        public static bool hadRuntimeError { get; set; } = false;

        private OutputForm outputForm;

        private string code
        {
            get
            {
                return this.outputForm.mainForm.code;
            }
        }


        public Interpreter(OutputForm outputForm)
        {
            this.outputForm = outputForm;
        }

        public void run()
        {
            hadError = false;

            Logger.info(LogMessage.START_INTERPRETING);

            Scanner scanner = new Scanner(code);
            LinkedList<Token> tokens = scanner.scanTokens();

            if (OutputForm.runTaskCancelToken.IsCancellationRequested)
            {
                return;
            }

            foreach (Token token in tokens)
            {
                if (OutputForm.runTaskCancelToken.IsCancellationRequested)
                {
                    break;
                }
                Logger.info(token.ToString());
            }

            List<Token> tokensList = new List<Token>(tokens);

            Parser parser = new Parser(ref tokensList);
            parser.parseMain().execute();

            this.outputForm.stopMenuItem_Click(null, null);
            this.outputForm.outputText += "\n\n\n";
            this.outputForm.scrollRtbOutputToEnd();
        }
    }
}
