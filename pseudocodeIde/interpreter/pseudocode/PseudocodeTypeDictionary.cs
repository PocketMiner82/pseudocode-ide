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

using System.Collections.Generic;

namespace pseudocode_ide.interpreter.pseudocode
{
    /// <summary>
    /// Represents a collection for storing and retrieving PseudocodeType objects by their Keyword.
    /// </summary>
    public class PseudocodeTypeDictionary : Dictionary<string, PseudocodeType>
    {
        /// <summary>
        /// Adds a PseudocodeType to the dictionary.
        /// </summary>
        /// <param name="pseudocodeType">The PseudocodeType to add.</param>
        public void Add(PseudocodeType pseudocodeType)
        {
            Add(pseudocodeType.Keyword, pseudocodeType);
        }
    }
}
