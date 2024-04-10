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

using pseudocodeIde.interpreter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace pseudocodeIde
{
    public static class Extensions
    {
        /// <summary>
        /// Trims a stack of the Type T to a specified count
        /// </summary>
        public static Stack<T> Trim<T>(this Stack<T> stack, int trimCount)
        {
            return stack.Count <= trimCount
                ? stack
                : new Stack<T>(stack.ToArray().Take(trimCount).Reverse());
        }

        /// <summary>
        /// Get the indentation level of a text line
        /// </summary>
        /// <returns>how many tabs are at the start of the line</returns>
        public static int GetIndentationLevel(this string line)
        {
            int indentationLevel = 0;
            foreach (char c in line)
            {
                if (c == '\t')
                {
                    indentationLevel++;
                }
                else
                {
                    break;
                }
            }
            return indentationLevel;
        }

        /// <summary>
        /// Get the next Token in the list.
        /// </summary>
        /// <returns>next token or the last token in the list</returns>
        public static LinkedListNode<Token> NextOrLast(this LinkedListNode<Token> node)
        {
            return node.Next ?? node;
        }

        /// <summary>
        /// Reads an embedded ressource text file.
        /// </summary>
        /// <param name="assembly">the assembly where the text file is in</param>
        /// <param name="name">the filename of the text file, e.g. "TemplateCodeOutput.cs"</param>
        /// <returns></returns>
        public static string ReadResource(this Assembly assembly, string name)
        {
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
