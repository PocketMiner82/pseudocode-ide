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

namespace pseudocode_ide
{
    /// <summary>
    /// Represents a point in the undo/redo stack for the code textbox.
    /// Each instance captures the state of the text and selection at a specific point in time.
    /// </summary>
    public class UndoPoint
    {
        /// <summary>
        /// Gets or sets the code (text) at this point in the undo/redo stack.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the starting index of the selection in the text at this point in the undo/redo stack.
        /// </summary>
        public int SelectionStart { get; set; }

        /// <summary>
        /// Gets or sets the ending index of the selection in the text at this point in the undo/redo stack.
        /// </summary>
        public int SelectionEnd { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoPoint"/> class with the specified code, selection start, and selection end.
        /// </summary>
        /// <param name="code">The code (text) at this point in the undo/redo stack.</param>
        /// <param name="selectionStart">The starting index of the selection in the text.</param>
        /// <param name="selectionEnd">The ending index of the selection in the text.</param>
        public UndoPoint(string code, int selectionStart, int selectionEnd)
        {
            Code = code;
            SelectionStart = selectionStart;
            SelectionEnd = selectionEnd;
        }
    }
}
