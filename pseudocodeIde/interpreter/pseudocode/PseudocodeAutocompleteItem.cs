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
using System.Collections.Generic;
using System.Linq;

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
            Text = Text.Replace("\n", $"\n{indents}");

            return Text;
        }

        /// <summary>
        /// After this item got selected (the text is already pasted in scintilla), put all the variable locations in a list
        /// </summary>
        /// <param name="e"></param>
        public override void OnSelected(SelectedEventArgs e)
        {
            Scintilla scintilla = (Scintilla)Parent.TargetControlWrapper.TargetControl;
            List<(int selectionStart, int selectionEnd)> tabSelections = new List<(int selectionStart, int selectionEnd)>();

            bool inSelection = false;
            int textLengthWithoutMarkers = Text.Replace("\\^", " ").Replace("^", "").Length;
            for (int i = Parent.Fragment.Start; ; i++)
            {
                recheck:
                if (i >= Parent.Fragment.Start + textLengthWithoutMarkers) {
                    break;
                }

                if (scintilla.Text[i] == '^')
                {
                    if (i != 0 && scintilla.Text[i - 1] == '\\')
                    {
                        scintilla.Text = scintilla.Text.Remove(i - 1, 1);
                        goto recheck;
                    }

                    scintilla.Text = scintilla.Text.Remove(i, 1);

                    if (!inSelection) // && firstOccurrenceSelected
                    {
                        tabSelections.Add((selectionStart: i, selectionEnd: Parent.Fragment.Start + textLengthWithoutMarkers ));
                        inSelection = true;
                    }
                    else // inSelection && firstOccurrenceSelected
                    {
                        (int selectionStart, int selectionEnd) lastSelection = tabSelections.Last();
                        lastSelection.selectionEnd = i;
                        tabSelections[tabSelections.Count - 1] = lastSelection;

                        inSelection = false;
                    }
                }
            }

            PseudocodeIDEForm.Instance.AddTabSelectionIndicators(tabSelections);
        }
    }
}
