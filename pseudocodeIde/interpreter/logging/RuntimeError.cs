﻿using System;

namespace pseudocodeIde.interpreter.sequences
{
    public class RuntimeError : Exception
    {
        public int line { get; set; }

        public RuntimeError(int line, string message) : base(message)
        {
            this.line = line;
        }
    }
}
