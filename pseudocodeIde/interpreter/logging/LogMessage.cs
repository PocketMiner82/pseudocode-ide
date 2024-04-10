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
    /// <summary>
    /// Contains constant log messages.
    /// </summary>
    public static class LogMessage
    {
        public const string GENERATING_C_SHARP_CODE = "Generiere C#-Code...";
        public const string GENERATED_C_SHARP_CODE = "Folgender C#-Code wurde generiert:";
        public const string COMPILING_C_SHARP_CODE = "Kompiliere C#-Code...";

        public const string RUNNING_PROGRAM = "Das Programm wird gestartet...";
        public const string STOPPED_PROGRAM = "Das Programm wurde gestoppt.";
        public const string COMPILE_ERRORS = "Beim Kompilieren sind folgende Fehler aufgetreten:";
    }
}
