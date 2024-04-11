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

using System;

namespace pseudocodeIde.interpreter.logging
{
    /// <summary>
    /// Contains constant log messages and the current timestamp.
    /// </summary>
    public static class LogPrefix
    {
        public const string INFO = "[INFO ] ";

        public const string DEBUG = "[DEBUG] ";

        public const string ERROR = "[ERROR] ";

        public static string Timestamp
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss.fff") + "   ";
            }
        }
    }
}
