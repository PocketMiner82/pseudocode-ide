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

namespace pseudocodeIde.interpreter.logging
{
    public static class Logger
    {
        /// <summary>
        /// The output form, where log messages will be printed
        /// </summary>
        public static OutputForm OutputForm { get; set; }

        /// <summary>
        /// Log an information
        /// </summary>
        /// <param name="msg">the message to log</param>
        public static void Info(string msg)
        {
            Print(LogPrefix.Timestamp + LogPrefix.INFO + msg);
        }

        /// <summary>
        /// Log an error. This will prevent the compiler from running the pseudocode
        /// </summary>
        /// <param name="msg">the message to log</param>
        public static void Error(string msg)
        {
            Print(LogPrefix.Timestamp + LogPrefix.ERROR + msg);
            Interpreter.HadError = true;
        }

        /// <summary>
        /// log an error in a specific line
        /// </summary>
        /// <param name="line">the line where the error occured</param>
        /// <param name="msg">the message to log</param>
        public static void Error(int line, string msg)
        {
            Error($"Zeile {line}: {msg}");
        }

        /// <summary>
        /// Print a message to the log, without timestamp or prefix
        /// </summary>
        /// <param name="msg">the message to log</param>
        /// <param name="newLine">whether a new line should be appended to the message</param>
        public static void Print(string msg, bool newLine = true)
        {
            if (OutputForm != null)
            {
                OutputForm.OutputText += msg + (newLine ? "\n" : "");
            }
        }
    }
}
