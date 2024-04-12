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

using AutocompleteMenuNS;
using pseudocodeIde;
using ScintillaNET;

namespace pseudocode_ide.interpreter.pseudocode
{
    /// <summary>
    /// Represents an autocomplete item for pseudocode with custom behavior for replacing text.
    /// </summary>
    public class PseudocodeAutocompleteItem : SnippetAutocompleteItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PseudocodeAutocompleteItem"/> class with the specified snippet and optional menu text.
        /// </summary>
        /// <param name="snippet">The snippet to be used for autocomplete.</param>
        /// <param name="menuText">The text to be displayed in the autocomplete menu. If null, the snippet is used.</param>
        public PseudocodeAutocompleteItem(string snippet, string menuText = null) : base(snippet)
        {
            if (menuText != null)
            {
                MenuText = menuText;
            }
        }

        /// <summary>
        /// Gets the text to replace the current fragment with, adjusting for indentation based on the current line.
        /// </summary>
        /// <returns>The adjusted text for replacement.</returns>
        public override string GetTextForReplace()
        {
            Scintilla scintilla = (Scintilla)Parent.TargetControlWrapper.TargetControl;

            string line = scintilla.Lines[scintilla.LineFromPosition(Parent.Fragment.Start)].Text;
            string indents = new string('\t', line.GetIndentationLevel());

            return Text.Replace("\n", $"\n{indents}");
        }
    }
}
