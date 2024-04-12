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
using System.Collections.Generic;

namespace pseudocode_ide.interpreter.pseudocode
{
    /// <summary>
    /// Represents a type of pseudocode with its keyword, token type, and autocomplete items.
    /// </summary>
    public class PseudocodeType
    {
        /// <summary>
        /// The keyword associated with this pseudocode type.
        /// </summary>
        public readonly string Keyword;

        /// <summary>
        /// The token type associated with this pseudocode type.
        /// </summary>
        public readonly TokenType TType;

        /// <summary>
        /// A list of autocomplete items for this pseudocode type.
        /// </summary>
        public readonly List<PseudocodeAutocompleteItem> ACItems = new List<PseudocodeAutocompleteItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PseudocodeType"/> class with the specified keyword, token type, and snippets.
        /// </summary>
        /// <param name="keyword">The keyword for this pseudocode type.</param>
        /// <param name="tType">The token type for this pseudocode type.</param>
        /// <param name="snippets">The snippets to be added as autocomplete items.</param>
        public PseudocodeType(string keyword, TokenType tType, params string[] snippets)
        {
            Keyword = keyword;
            TType = tType;

            if (snippets != null)
            {
                foreach (string snippet in snippets)
                {
                    ACItems.Add(new PseudocodeAutocompleteItem(snippet, $"{keyword} Block"));
                }
            }

            ACItems.Add(new PseudocodeAutocompleteItem(keyword));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PseudocodeType"/> class with the specified keyword, token type, and autocomplete items.
        /// </summary>
        /// <param name="keyword">The keyword for this pseudocode type.</param>
        /// <param name="tType">The token type for this pseudocode type.</param>
        /// <param name="acItems">The autocomplete items for this pseudocode type.</param>
        public PseudocodeType(string keyword, TokenType tType, List<PseudocodeAutocompleteItem> acItems)
        {
            Keyword = keyword;
            TType = tType;

            if (acItems != null)
            {
                ACItems.AddRange(acItems);
            }

            ACItems.Add(new PseudocodeAutocompleteItem(keyword));
        }

        /// <summary>
        /// Implicitly converts a <see cref="PseudocodeType"/> to a <see cref="TokenType"/>.
        /// </summary>
        /// <param name="pseudocodeType">The <see cref="PseudocodeType"/> to convert.</param>
        public static implicit operator TokenType(PseudocodeType pseudocodeType)
        {
            return pseudocodeType == null ? throw new ArgumentNullException(nameof(pseudocodeType)) : pseudocodeType.TType;
        }

        /// <summary>
        /// Implicitly converts a <see cref="PseudocodeType"/> to a list of <see cref="PseudocodeAutocompleteItem"/>.
        /// </summary>
        /// <param name="pseudocodeType">The <see cref="PseudocodeType"/> to convert.</param>
        public static implicit operator List<PseudocodeAutocompleteItem>(PseudocodeType pseudocodeType)
        {
            return pseudocodeType == null ? throw new ArgumentNullException(nameof(pseudocodeType)) : pseudocodeType.ACItems;
        }
    }
}
