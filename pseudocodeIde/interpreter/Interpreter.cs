// Pseudocode IDE - Execute Pseudocode for the German (BW) 2024 Abitur
// Copyright (C) 2024  PocketMiner82
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using pseudocode_ide.interpreter.csharpcode;
using pseudocode_ide.interpreter.pseudocode;
using pseudocodeIde.interpreter.logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace pseudocodeIde.interpreter
{
    public class Interpreter
    {
        /// <summary>
        /// Did we have any errors during compiling?
        /// </summary>
        public static bool HadError { get; set; } = false;

        /// <summary>
        /// Did we have any errors during runtime?
        /// </summary>
        public static bool HadRuntimeError { get; set; } = false;

        /// <summary>
        /// The output form
        /// </summary>
        private static OutputForm _outputForm;

        /// <summary>
        /// The thread where the program will be compiled and executed on
        /// </summary>
        private Thread _thread = null;

        /// <summary>
        /// The code generator that will fill in the template code, compile and run it
        /// </summary>
        public CSharpCodeManager CodeManager { get; private set; } = null;

        /// <summary>
        /// The text in the scintilla code textbox
        /// </summary>
        private string Code
        {
            get
            {
                return _outputForm.MainForm.Code;
            }
        }

        /// <summary>
        /// This class contains starting and stopping functions for the interpreter system
        /// </summary>
        /// <param name="outputForm"></param>
        public Interpreter(OutputForm outputForm)
        {
            Interpreter._outputForm = outputForm;
        }

        /// <summary>
        /// Start the interpreter thread
        /// </summary>
        public void Run()
        {
            _thread = new Thread(InterpreterThreadMethod);
            _thread.Start();
        }

        /// <summary>
        /// Method that gets called by the thread
        /// </summary>
        private void InterpreterThreadMethod()
        {
            if (!TryRun())
            {
                OnStop();
            }
        }

        /// <summary>
        /// Try to generate tokens, parse the tokens, put the parsed code in the template,
        /// compile and execute the code.
        /// </summary>
        private bool TryRun()
        {
            try
            {
                HadError = false;

                Logger.Info(LogMessage.GENERATING_C_SHARP_CODE);

                // convert pseudocode to token list
                Scanner scanner = new Scanner(Code);
                LinkedList<Token> tokens = scanner.ScanTokens();

                if (HadError)
                {
                    return false;
                }

                string tokensString = string.Join("\n", tokens);
                Logger.Info($"Generierte Tokens:\n{tokensString}\n\n");

                // parse the tokens into C# code
                Parser parser = new Parser(tokens);
                CodeManager = parser.ParseTokens();
                _outputForm.ShowCopyButton();

                if (HadError)
                {
                    return false;
                }

                // put the parsed tokens in the template file and try to compile it
                CodeManager.Compile();

                if (HadError)
                {
                    return false;
                }

                Logger.Info(LogMessage.RUNNING_PROGRAM);
                Logger.Print("");

                // execute the program
                CodeManager.Execute();
                return true;
            }
            catch (ThreadAbortException)
            {
                return false;
            }
        }

        /// <summary>
        /// Stop the interpreter thread.
        /// </summary>
        public void Stop()
        {
            if (_thread != null && _thread.IsAlive)
            {
                _thread.Abort();
            }
        }

        /// <summary>
        /// This method gets called, when the thread got stopped for whatever reason
        /// </summary>
        public static void OnStop()
        {
            _outputForm?.Invoke(new Action(() => _outputForm.StopMenuItem_Click(null, null)));
        }
    }
}
