using pseudocodeIde.interpreter.logging;
using pseudocodeIde.interpreter.parser;
using System;
using System.Collections.Generic;
using System.Threading;

namespace pseudocodeIde.interpreter
{
    public class Interpreter
    {
        public static bool hadError { get; set; } = false;

        public static bool hadRuntimeError { get; set; } = false;

        private static OutputForm outputForm;

        private Thread programThread = null;

        public CSharpCode cSharpCode { get; private set; } = null;

        private string code
        {
            get
            {
                return outputForm.mainForm.code;
            }
        }


        public Interpreter(OutputForm outputForm)
        {
            Interpreter.outputForm = outputForm;
        }

        public void run()
        {
            if (!this.tryRun())
            {
                onStop();
            }
        }

        public static void onStop()
        {
            if (outputForm != null)
            {
                outputForm.Invoke(new Action(() =>
                {
                    outputForm.stopMenuItem_Click(null, null);
                }));
            }
        }

        private bool tryRun()
        {
            hadError = false;

            Logger.info(LogMessage.GENERATING_C_SHARP_CODE);

            Scanner scanner = new Scanner(code);
            LinkedList<Token> tokens = scanner.scanTokens();

            if (this.cancelRequestedOrError())
            {
                return false;
            }

            string tokensString = string.Join("\n", tokens);
            Logger.info($"Generierte Tokens:\n{tokensString}\n\n");

            Parser parser = new Parser(tokens);
            this.cSharpCode = parser.parseTokens();
            outputForm.showCopyButton();

            if (this.cancelRequestedOrError())
            {
                return false;
            }

            this.cSharpCode.compile();

            if (this.cancelRequestedOrError())
            {
                return false;
            }

            Logger.info(LogMessage.RUNNING_PROGRAM);
            Logger.print("");

            this.programThread = new Thread(this.cSharpCode.execute);
            this.programThread.Start();
            return true;
        }

        private bool cancelRequestedOrError()
        {
            return hadError || OutputForm.runTaskCancelToken.IsCancellationRequested;
        }

        public void stopProgram()
        {
            if (this.programThread != null && this.programThread.IsAlive)
            {
                this.programThread.Abort();
            }
        }
    }
}
