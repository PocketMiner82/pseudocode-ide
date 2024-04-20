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
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using pseudocode_ide;
using pseudocode_ide.interpreter.pseudocode;
using pseudocode_ide.interpreter.scanner;
using pseudocodeIde.interpreter;
using ScintillaNET;
using ScintillaNET_FindReplaceDialog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace pseudocodeIde
{
    public partial class PseudocodeIDEForm : Form
    {
        /// <summary>
        /// the maximum steps to save in the undo stack
        /// </summary>
        private const int MAX_UNDO_SIZE = 250;

        // Indicators 0-7 could be in use by a lexer so we'll use indicator 8
        /// <summary>
        /// Indicator ID for highlighting words.
        /// </summary>
        public const int WORD_HIGHLIGHT_INDICATOR_ID = 8;

        /// <summary>
        /// Indicator ID for selecting the next word via tab.
        /// </summary>
        public const int TAB_SELECTION_INDICATOR_ID = 9;

        /// <summary>
        /// do not update the undo stack when the last char is a letter from A-Z (case ignored), a number from 0-9 or a underscore
        /// </summary>
        private readonly Regex _noUpdateAfter = new Regex(@"^[a-zA-Z0-9_äöüÄÖÜß]$", RegexOptions.Multiline);

        /// <summary>
        /// the code currently in the textbox
        /// </summary>
        public string Code
        {
            get
            {
                return (string)Invoke((Func<string>)delegate
                {
                    return codeTextBox.Text ?? "";
                });
            }
        }

        /// <summary>
        /// if the textbox update event should be ignored the next time
        /// </summary>
        public bool IgnoreTextChange { get; set; } = false;

        /// <summary>
        /// if the undo events should be ignored
        /// </summary>
        public bool NoNewUndoPoint { get; set; } = false;

        /// <summary>
        /// last cursor position before cursor move
        /// </summary>
        private int _lastCursorPosition = 0;

        // undo and redo stacks
        private Stack<UndoPoint> _undoStack = new Stack<UndoPoint>();
        private readonly Stack<UndoPoint> _redoStack = new Stack<UndoPoint>();

        /// <summary>
        /// the path where this file is saved
        /// </summary>
        private string _filePath;

        /// <summary>
        /// is the code saved?
        /// </summary>
        private bool _isSaved = true;

        /// <summary>
        /// The find and replace forms for the code textbox.
        /// </summary>
        private readonly FindReplace _findReplace;

        /// <summary>
        /// The output form where the pseudocode execution logs are displayed.
        /// </summary>
        private readonly OutputForm _outputForm;

        /// <summary>
        /// The maximum length of the line number characters.
        /// </summary>
        private int _maxLineNumberCharLength;

        /// <summary>
        /// Contains the instance of this form
        /// </summary>
        public static PseudocodeIDEForm Instance;

        public PseudocodeIDEForm()
        {
            Instance = this;

            InitializeComponent();
            _outputForm = new OutputForm(this);
            _findReplace = new FindReplace(codeTextBox);
            _findReplace.KeyPressed += CodeTextBox_KeyDown;

            ResetUndoRedo();

            // disable right click menu
            codeTextBox.UsePopup(false);

            CodeTextBox_TextChanged(null, null);
            // on first start, the code is always saved
            SetFileSaved();
        }

        /// <summary>
        /// Builds the autocomplete menu with keywords from the scanner.
        /// </summary>
        private void BuildAutocompleteMenu()
        {
            autoCompleteMenu.TargetControlWrapper = new ScintillaWrapper(codeTextBox);

            List<AutocompleteItem> items = new List<AutocompleteItem>();

            foreach (List<PseudocodeAutocompleteItem> keywordItems in PseudocodeKeywords.KEYWORDS.Values)
            {
                items.AddRange(keywordItems);
            }

            autoCompleteMenu.SetAutocompleteItems(items);
        }

        // ---------------------------------------------
        // COMMON EVENT LISTENERS
        // ---------------------------------------------

        private void PseudocodeIDEForm_Load(object sender, EventArgs e)
        {
            // on start, copy the updater to the local drive to allow updating on SMB1 network shares
            string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string tempExeDir = Path.Combine(Path.GetTempPath(), "pseudocode-ide\\updater");

            FileSystem.CopyDirectory(Path.Combine(currentDir, "updater"), tempExeDir, true);

            // show remind later
            CheckForUpdate(true, false);

            // set font
            ConfigureCodeTextBox();
            BuildAutocompleteMenu();
        }

        /// <summary>
        /// Configures the code text box with default styles and settings.
        /// </summary>
        private void ConfigureCodeTextBox()
        {
            codeTextBox.StyleResetDefault();
            codeTextBox.Styles[Style.Default].Font = "Courier New";
            codeTextBox.StyleClearAll();

            codeTextBox.Styles[SyntaxHighlightingScanner.STYLE_DEFAULT].ForeColor = Color.DarkGray;

            codeTextBox.Styles[SyntaxHighlightingScanner.STYLE_KEYWORD].ForeColor = Color.Blue;
            codeTextBox.Styles[SyntaxHighlightingScanner.STYLE_KEYWORD].Bold = true;

            codeTextBox.Styles[SyntaxHighlightingScanner.STYLE_IDENTIFIER].ForeColor = Color.Black;

            codeTextBox.Styles[SyntaxHighlightingScanner.STYLE_NUMBER].ForeColor = Color.Peru;

            codeTextBox.Styles[SyntaxHighlightingScanner.STYLE_STRING].ForeColor = Color.OrangeRed;

            codeTextBox.Styles[SyntaxHighlightingScanner.STYLE_ESCAPE].ForeColor = Color.Orange;
            codeTextBox.Styles[SyntaxHighlightingScanner.STYLE_ESCAPE].Bold = true;

            codeTextBox.Styles[SyntaxHighlightingScanner.STYLE_COMMENT].ForeColor = Color.Green;

            codeTextBox.LexerName = "";
            codeTextBox.StyleNeeded += CodeTextBox_StyleNeeded;
        }

        private void CodeTextBox_StyleNeeded(object sender, StyleNeededEventArgs e)
        {
            int startPos = codeTextBox.GetEndStyled();
            int endPos = e.Position;

            SyntaxHighlightingScanner.Style(codeTextBox, startPos, endPos);
        }

        private void WordWrapMenuItem_Click(object sender, EventArgs e)
        {
            codeTextBox.WrapMode = wordWrapMenuItem.Checked ? WrapMode.Word : WrapMode.None;
        }

        private void SingleEqualIsCompareOperatorMenuItem_Click(object sender, EventArgs e)
        {
            Scanner.SingleEqualIsCompareOperator = singleEqualIsCompareOperatorMenuItem.Checked;

            SetFileNotSaved();
        }

        private void PseudocodeIDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            // main form won't close if child form is not disposed
            if (!_outputForm.IsDisposed)
            {
                _outputForm.Close();
                Close();
                return;
            }

            if (!_isSaved)
            {
                if (MessageBox.Show("Möchtest du deine ungespeicherten Änderungen speichern?", "Ungespeicherte Änderungen", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveMenuItem_Click(null, null);
                }
            }
        }

        // ---------------------------------------------
        // CODE TEXTBOX (Scintilla)
        // ---------------------------------------------

        /// <summary>
        /// Updates the undo stack, when user updated the code
        /// </summary>
        private void UserUpdatedText()
        {
            // only update undo stack if next event not ignored
            if (IgnoreTextChange)
            {
                IgnoreTextChange = false;
            }
            else
            {
                UpdateUndoStack(false);
            }

            TryHandleContentUpdate();
        }

        /// <summary>
        /// User changed selection
        /// </summary>
        private void UserChangedSelection()
        {
            HighlightWord(codeTextBox.SelectedText);
            RemovePreviousTabSelectionIndicators();

            if (_lastCursorPosition != codeTextBox.SelectionStart && codeTextBox.Text != _undoStack.Peek().Code)
            {
                UpdateUndoStack(true);
            }
        }

        /// <summary>
        /// Forcefully updates the undoStack, when the cursor moved more than 1 since last check or the user selected something
        /// </summary>
        private void TryHandleContentUpdate()
        {
            // ignore when we already undid something
            if (_redoStack.Count != 0)
            {
                return;
            }

            if (codeTextBox.SelectionEnd - codeTextBox.SelectionStart != 0 || Math.Abs(_lastCursorPosition - codeTextBox.SelectionStart) > 1)
            {
                UpdateUndoStack(true);
            }
        }

        private void CodeTextBox_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            switch (e.Change)
            {
                case UpdateChange.Content:
                    UserUpdatedText();
                    break;
                case UpdateChange.Selection:
                    UserChangedSelection();
                    break;
            }
            _lastCursorPosition = codeTextBox.SelectionStart;
        }

        private void CodeTextBox_TextChanged(object sender, EventArgs e)
        {
            // when the code is modified, the code is no longer saved in the file
            SetFileNotSaved();

            // Did the number of characters in the line number display change?
            int maxLineNumberCharLength = codeTextBox.Lines.Count.ToString().Length;
            if (maxLineNumberCharLength == _maxLineNumberCharLength)
            {
                return;
            }

            // calculate the width required to display the last line number
            // + include some padding for good measure
            const int PADDING = 2;
            codeTextBox.Margins[0].Width = codeTextBox.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + PADDING;
            _maxLineNumberCharLength = maxLineNumberCharLength;
        }

        private void CodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers == Keys.None)
            {
                e.SuppressKeyPress = TrySelectNextTabIndicator();
            }

            //// hack to allow enter to autocomplete even if down wasnt pressed before
            //if ((e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab) && e.Modifiers == Keys.None && autoCompleteMenu.SelectedItemIndex < 0)
            //{
            //    autoCompleteMenu.ProcessKey((char)Keys.Down, e.Modifiers);
            //}

            // ignore CTRL[+SHIFT]+(Z/Y/L/R/E/S)
            if ((e.KeyCode == Keys.Z || e.KeyCode == Keys.Y || e.KeyCode == Keys.L || e.KeyCode == Keys.R || e.KeyCode == Keys.E || e.KeyCode == Keys.S)
                && (Control.ModifierKeys == Keys.Control || Control.ModifierKeys == (Keys.Control | Keys.Shift)))
            {
                e.SuppressKeyPress = true;
            }
            // deleting always updates undo stack
            else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                IgnoreTextChange = true;
                UpdateUndoStack(true);
                CodeTextBox_UpdateUI(null, new UpdateUIEventArgs(UpdateChange.Content));
                e.SuppressKeyPress = false;
            }
            // find replace: find previous
            else if (e.Shift && e.KeyCode == Keys.F3)
            {
                _findReplace.Window.FindPrevious();
                e.SuppressKeyPress = true;
            }
            // find replace: find next
            else if (e.KeyCode == Keys.F3)
            {
                _findReplace.Window.FindNext();
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Add indicators to scintilla, where the variables are, so that the user can tab through them
        /// </summary>
        /// <param name="tabSelections"></param>
        /// <param name="scintilla"></param>
        public void AddTabSelectionIndicators(List<(int selectionStart, int selectionEnd)> tabSelections)
        {
            if (tabSelections.Count >= 1)
            {
                codeTextBox.SelectionStart = tabSelections.First().selectionStart;
                codeTextBox.SelectionEnd = tabSelections.First().selectionEnd;
                tabSelections.RemoveAt(0);
            }

            // remove all uses of our indicator
            codeTextBox.IndicatorCurrent = TAB_SELECTION_INDICATOR_ID;
            codeTextBox.IndicatorClearRange(0, codeTextBox.TextLength);

            if (tabSelections.Count == 0)
            {
                return;
            }

            // update indicator appearance
            codeTextBox.Indicators[TAB_SELECTION_INDICATOR_ID].Style = IndicatorStyle.StraightBox;
            codeTextBox.Indicators[TAB_SELECTION_INDICATOR_ID].Under = true;
            codeTextBox.Indicators[TAB_SELECTION_INDICATOR_ID].ForeColor = Color.LightBlue;
            codeTextBox.Indicators[TAB_SELECTION_INDICATOR_ID].OutlineAlpha = 255;
            codeTextBox.Indicators[TAB_SELECTION_INDICATOR_ID].Alpha = 100;

            foreach ((int selectionStart, int selectionEnd) in tabSelections)
            {
                codeTextBox.IndicatorFillRange(selectionStart, selectionEnd - selectionStart);
            }

            TryHandleContentUpdate();
        }

        /// <summary>
        /// Try to select the next tab indicator on tab press
        /// </summary>
        /// <returns>true if the next indicator was selected</returns>
        private bool TrySelectNextTabIndicator()
        {
            (int, int)? firstSelection = GetFirstTabSelectionTuple();
            if (firstSelection != null)
            {
                (int selectionStart, int selectionEnd) = firstSelection.Value;

                codeTextBox.IndicatorCurrent = TAB_SELECTION_INDICATOR_ID;
                codeTextBox.IndicatorClearRange(selectionStart, selectionEnd - selectionStart);
                codeTextBox.SelectionStart = selectionStart;
                codeTextBox.SelectionEnd = selectionEnd;
                UserChangedSelection();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the selection start and end of the first tab selection indicator
        /// </summary>
        /// <returns>null if there is no tab selection indicator</returns>
        private (int selectionStart, int selectionEnd)? GetFirstTabSelectionTuple()
        {
            int textLength = codeTextBox.TextLength;
            Indicator indicator = codeTextBox.Indicators[TAB_SELECTION_INDICATOR_ID];
            int bitmapFlag = (1 << indicator.Index);

            int endPos = 0;
            do
            {
                int startPos = indicator.Start(endPos);
                endPos = indicator.End(startPos);

                // Is this range filled with our indicator (TAB_SELECTION_INDICATOR_ID)?
                uint bitmap = codeTextBox.IndicatorAllOnFor(startPos);
                bool filled = ((bitmapFlag & bitmap) == bitmapFlag);
                if (filled)
                {
                    return (startPos, endPos);
                }
            } while (endPos != 0 && endPos < textLength);

            return null;
        }

        /// <summary>
        /// Removes all tab completion that are present before the selection end
        /// </summary>
        private void RemovePreviousTabSelectionIndicators()
        {
            (int, int)? firstSelection = GetFirstTabSelectionTuple();
            while (firstSelection != null)
            {
                (int selectionStart, int selectionEnd) = firstSelection.Value;

                if (selectionEnd <= codeTextBox.SelectionEnd)
                {
                    codeTextBox.IndicatorCurrent = TAB_SELECTION_INDICATOR_ID;
                    codeTextBox.IndicatorClearRange(selectionStart, selectionEnd - selectionStart);
                }
                else
                {
                    break;
                }

                firstSelection = GetFirstTabSelectionTuple();
            }
        }

        private void CodeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // keep tab indentation from previous line on new line
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;

                int currentLineIndex = codeTextBox.CurrentLine - 1;
                string currentLine = codeTextBox.Lines[currentLineIndex].Text;
                int indentationLevel = currentLine.GetIndentationLevel();

                string indentation = new string('\t', indentationLevel);

                codeTextBox.AddText(indentation);
            }
        }

        // adapted from https://github.com/jacobslusser/ScintillaNET/wiki/Character-Autocompletion#finishing-touch
        private void CodeTextBox_CharAdded(object sender, CharAddedEventArgs e)
        {
            int caretPos = codeTextBox.CurrentPosition;
            bool docStart = caretPos == 1;

            char charPrev = (char)(docStart ? codeTextBox.GetCharAt(caretPos) : codeTextBox.GetCharAt(caretPos - 2));
            char charNext = (char)codeTextBox.GetCharAt(caretPos);

            bool addClosingQuotes = docStart || !Scanner.IsAlphaNumeric(charNext) && !Scanner.IsAlphaNumeric(charPrev);

            switch (e.Char)
            {
                case '(':
                    codeTextBox.InsertText(caretPos, ")");
                    break;
                case ')':
                    if (charPrev == '(' && charNext == ')')
                    {
                        codeTextBox.DeleteRange(caretPos, 1);
                        codeTextBox.GotoPosition(caretPos);
                        return;
                    }
                    break;
                case '{':
                    codeTextBox.InsertText(caretPos, "}");
                    break;
                case '}':
                    if (charPrev == '{' && charNext == '}')
                    {
                        codeTextBox.DeleteRange(caretPos, 1);
                        codeTextBox.GotoPosition(caretPos);
                        return;
                    }
                    break;
                case '[':
                    codeTextBox.InsertText(caretPos, "]");
                    break;
                case ']':
                    if (charPrev == '[' && charNext == ']')
                    {
                        codeTextBox.DeleteRange(caretPos, 1);
                        codeTextBox.GotoPosition(caretPos);
                        return;
                    }
                    break;
                case '"':
                    if (charPrev == '"' && charNext == '"')
                    {
                        codeTextBox.DeleteRange(caretPos, 1);
                        codeTextBox.GotoPosition(caretPos);
                        return;
                    }

                    if (addClosingQuotes)
                    {
                        codeTextBox.InsertText(caretPos, "\"");
                    }
                    break;
                case '\'':
                    if (charPrev == '\'' && charNext == '\'')
                    {
                        codeTextBox.DeleteRange(caretPos, 1);
                        codeTextBox.GotoPosition(caretPos);
                        return;
                    }

                    if (addClosingQuotes)
                    {
                        codeTextBox.InsertText(caretPos, "'");
                    }
                    break;
            }
        }

        // adapted from https://github.com/desjarlais/Scintilla.NET/wiki/Find-and-Highlight-Words
        private void HighlightWord(string text)
        {
            // Remove all uses of our indicator
            codeTextBox.IndicatorCurrent = WORD_HIGHLIGHT_INDICATOR_ID;
            codeTextBox.IndicatorClearRange(0, codeTextBox.TextLength);

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            // Update indicator appearance
            codeTextBox.Indicators[WORD_HIGHLIGHT_INDICATOR_ID].Style = IndicatorStyle.StraightBox;
            codeTextBox.Indicators[WORD_HIGHLIGHT_INDICATOR_ID].Under = true;
            codeTextBox.Indicators[WORD_HIGHLIGHT_INDICATOR_ID].ForeColor = Color.Lime;
            codeTextBox.Indicators[WORD_HIGHLIGHT_INDICATOR_ID].OutlineAlpha = 100;
            codeTextBox.Indicators[WORD_HIGHLIGHT_INDICATOR_ID].Alpha = 100;

            // Search the document
            codeTextBox.TargetStart = 0;
            codeTextBox.TargetEnd = codeTextBox.TextLength;
            codeTextBox.SearchFlags = SearchFlags.None;
            while (codeTextBox.SearchInTarget(text) != -1)
            {
                if (codeTextBox.TargetStart != codeTextBox.SelectionStart)
                {
                    // Mark the search results with the current indicator
                    codeTextBox.IndicatorFillRange(codeTextBox.TargetStart, codeTextBox.TargetEnd - codeTextBox.TargetStart);
                }

                // Search the remainder of the document
                codeTextBox.TargetStart = codeTextBox.TargetEnd;
                codeTextBox.TargetEnd = codeTextBox.TextLength;
            }
        }

        // ---------------------------------------------
        // NEW/OPEN/SAVE
        // ---------------------------------------------

        private void NewMenuItem_Click(object sender, EventArgs e)
        {
            if (!_isSaved)
            {
                if (MessageBox.Show("Möchtest du wirklich eine neue Datei erstellen?\n" +
                    "Alle ungespeicherten Änderungen gehen verloren!", "Ungespeicherte Änderungen",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            // create new file
            IgnoreTextChange = true;
            codeTextBox.ClearAll();
            ResetUndoRedo();
            Text = "Pseudocode IDE - Neue Datei";
            _filePath = "";
            SetFileSaved();
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            if (!_isSaved)
            {
                if (MessageBox.Show("Möchtest du wirklich eine andere Datei öffnen?\n" +
                    "Alle ungespeicherten Änderungen gehen verloren!", "Ungespeicherte Änderungen",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            OpenFileDialog();
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            // if this is a new file
            if (_filePath == null || _filePath.Trim() == "")
            {
                SaveFileDialog();
            }
            else
            {
                SaveFile();
            }
        }

        /// <summary>
        /// Sets the application state to indicate that the file is saved.
        /// </summary>
        private void SetFileSaved()
        {
            _isSaved = true;
            saveMenuItem.Enabled = false;
            if (Text.EndsWith("*"))
            {
                Text = Text.Substring(0, Text.Length - 1);
            }
        }

        /// <summary>
        /// Sets the application state to indicate that the file is not saved.
        /// </summary>
        private void SetFileNotSaved()
        {
            _isSaved = false;
            if (!Text.EndsWith("*"))
            {
                Text += "*";
            }
            saveMenuItem.Enabled = true;
        }

        /// <summary>
        /// Opens a save file dialog and saves the current pseudocode file.
        /// </summary>
        /// <returns>True if the file was saved successfully, false otherwise.</returns>
        public bool SaveFileDialog()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Pseudocode Datei (*.pseudocode)|*.pseudocode|Alle Dateien (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Title = "Pseudocode Datei speichern";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // if the file already exists, we need to overwrite it
                    if (File.Exists(saveFileDialog.FileName))
                    {
                        File.Delete(saveFileDialog.FileName);
                    }

                    _filePath = saveFileDialog.FileName;

                    // save the file
                    SaveFile();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Saves the current pseudocode file to the specified file path.
        /// </summary>
        private void SaveFile()
        {
            PseudocodeFile pFile = new PseudocodeFile(Scanner.SingleEqualIsCompareOperator, codeTextBox.Text.Split('\n'));

            // write to disk
            using (StreamWriter outputFile = new StreamWriter(_filePath))
            {
                outputFile.Write(JsonConvert.SerializeObject(pFile, Formatting.Indented));
            }

            SetFileSaved();
            Text = "Pseudocode IDE - " + _filePath;
        }

        /// <summary>
        /// Opens an open file dialog and loads the selected pseudocode file.
        /// </summary>
        public void OpenFileDialog()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Pseudocode Datei (*.pseudocode)|*.pseudocode|Alle Dateien (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Pseudocode Datei öffnen";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _filePath = openFileDialog.FileName;

                    // open the file
                    OpenFile();
                }
            }
        }

        /// <summary>
        /// Loads the specified pseudocode file into the editor.
        /// </summary>
        private void OpenFile()
        {
            // read from disk
            using (StreamReader file = new StreamReader(_filePath))
            {
                IgnoreTextChange = true;
                PseudocodeFile pFile;
                try
                {
                    pFile = JsonConvert.DeserializeObject<PseudocodeFile>(file.ReadToEnd());
                }
                catch
                {
                    MessageBox.Show("Konnte den JSON Code in '" + _filePath + "' nicht parsen.", "Fehler");
                    _filePath = "";
                    IgnoreTextChange = false;
                    return;
                }

                codeTextBox.Text = string.Join("\n", pFile.FileContent);
                singleEqualIsCompareOperatorMenuItem.Checked = pFile.SingleEqualIsCompareOperator;

                ResetUndoRedo();
            }

            SetFileSaved();
            Text = "Pseudocode IDE - " + _filePath;
        }

        // ---------------------------------------------
        // UNDO/REDO
        // ---------------------------------------------

        /// <summary>
        /// Resets the undo and redo system 
        /// </summary>
        private void ResetUndoRedo()
        {
            _undoStack.Clear();
            _redoStack.Clear();
            _undoStack.Push(new UndoPoint(codeTextBox.Text, codeTextBox.SelectionStart, codeTextBox.SelectionEnd));
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Updates the undo stack
        /// </summary>
        /// <param name="forceUpdate">if the update should be done without checking for whole word</param>
        public void UpdateUndoStack(bool forceUpdate)
        {
            // undo currently disabled?
            if (NoNewUndoPoint)
            {
                return;
            }

            // when user writes something new, the redo stack will be cleared
            undoToolStripMenuItem.Enabled = true;
            if (_redoStack.Count != 0)
            {
                _redoStack.Clear();
                redoToolStripMenuItem.Enabled = false;
            }

            string undoText = codeTextBox.Text;

            // remove the newly added char if this is an update caused by text change
            if (!forceUpdate)
            {
                try
                {
                    undoText = undoText.Remove(codeTextBox.SelectionStart - 1, 1);
                }
                catch (ArgumentOutOfRangeException)
                {
                    return;
                }
            }


            // don't update when the last char matches the NO_UPDATE_AFTER regex and the update is caused by text change.
            // also don't update, when the new undo text is already in the stack
            if ((codeTextBox.SelectionStart > 1 && !forceUpdate && _noUpdateAfter.IsMatch(undoText.ElementAt(codeTextBox.SelectionStart - 2).ToString()))
                || _undoStack.Peek().Code == undoText)
            {
                return;
            }

            // again, make sure that the redo stack is empty
            _redoStack.Clear();

            // and update the undo stack, limit max undo size
            _undoStack.Push(new UndoPoint(undoText, codeTextBox.SelectionStart, codeTextBox.SelectionEnd));
            _undoStack = _undoStack.Trim(MAX_UNDO_SIZE);
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // put current text in redo if not already in there
            if (_redoStack.Count == 0 || !_redoStack.Peek().Equals(codeTextBox.Text))
            {
                _redoStack.Push(new UndoPoint(codeTextBox.Text, codeTextBox.SelectionStart, codeTextBox.SelectionEnd));
                redoToolStripMenuItem.Enabled = true;
            }

            // the undo stack always has at least one item in it
            if (_undoStack.Count <= 1)
            {
                // set the textbox text to the item without removing it from the stack
                IgnoreTextChange = true;
                UpdateCodeTextBox(_undoStack.Peek());

                // no more things to undo
                undoToolStripMenuItem.Enabled = false;
            }
            else
            {
                string currentText = codeTextBox.Text;

                // set the textbox text to the first item that doesn't match the current text. also remove them from the undo stack
                do
                {
                    IgnoreTextChange = true;
                    UpdateCodeTextBox(_undoStack.Peek());
                } while (_undoStack.Count > 1 && currentText == _undoStack.Pop().Code);

                // the undo stack always has the current text in it
                _undoStack.Push(new UndoPoint(codeTextBox.Text, codeTextBox.SelectionStart, codeTextBox.SelectionEnd));
            }
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = true;
            // put current text in undo if not already in there
            if (!_undoStack.Peek().Equals(codeTextBox.Text))
            {
                _undoStack.Push(new UndoPoint(codeTextBox.Text, codeTextBox.SelectionStart, codeTextBox.SelectionEnd));
            }

            // set the textbox text to the top item of the redo stack. also remove it from the stack
            if (_redoStack.Count > 0)
            {
                IgnoreTextChange = true;
                UpdateCodeTextBox(_redoStack.Pop());
            }

            // no more things to redo
            if (_redoStack.Count == 0)
            {
                redoToolStripMenuItem.Enabled = false;
            }
        }

        private void UpdateCodeTextBox(UndoPoint point)
        {
            codeTextBox.Text = point.Code;
            codeTextBox.SelectionStart = point.SelectionStart;
            codeTextBox.SelectionEnd = point.SelectionEnd;
            TryHandleContentUpdate();
        }

        // ---------------------------------------------
        // FIND/REPLACE
        // ---------------------------------------------

        private void FindMenuItem_Click(object sender, EventArgs e)
        {
            _findReplace.ShowFind();
        }

        private void ReplaceMenuItem_Click(object sender, EventArgs e)
        {
            _findReplace.ShowReplace();
        }

        private void GoToMenuItem_Click(object sender, EventArgs e)
        {
            new GoTo(codeTextBox).ShowGoToDialog();
        }

        // ---------------------------------------------
        // RUN PROGRAM
        // ---------------------------------------------

        private void RunProgramMenuItem_Click(object sender, EventArgs e)
        {
            _outputForm.ShowAndRun();
        }

        private void OpenOutputFormMenuItem_Click(object sender, EventArgs e)
        {
            _outputForm.Show();
        }

        // ---------------------------------------------
        // SHOW HELP
        // ---------------------------------------------

        private void ShowHelpMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Grundlegende Verwendung:\n" +
                            "Dieses Programm ermöglicht die Ausführung von Pseudocode nach der \"Formelsammlung 1.5.2 TG Informationstechnik\" für das Abitur 2024 in Baden Württemberg.\n\n" +
                            "Zusätzlich zu der Definition in der Formelsammlung wurden folgende Operationen definiert:\n" +
                            "UND - Und-Vergleich\n" +
                            "ODER - Oder-Vergleich\n" +
                            "schreibe(text) - Schreibt den gegeben Text in das Ausgabefenster\n" +
                            "warte(zeitMs) - Delay für eine bestimmte Zeit in Millisekunden\n" +
                            "benutzereingabe<Typ>(text, titel):Typ - Öffnet ein Dialogfenster. Gibt den eingegebenen Text als gegeben Typ zurück.\n\n" +
                            "Nicht implementiert:\n" +
                            "- Unterstützung für mehrere Dateien\n" +
                            "- Klassen/Objekte - erfordert Unterstützung für mehrere Dateien\n",

                            "Pseudocode IDE - Hilfe"
            );
        }

        // ---------------------------------------------
        // (AUTO) UPDATE
        // ---------------------------------------------

        private void UpdateMenuItem_Click(object sender, EventArgs e)
        {
            // dont show remind later
            CheckForUpdate(false, false);
        }

        /// <summary>
        /// Checks for updates to the application.
        /// </summary>
        /// <param name="firstRun">Indicates if this is the first run of the application.</param>
        /// <param name="beta">Indicates if beta updates should be checked.</param>
        private void CheckForUpdate(bool firstRun, bool beta)
        {
            string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string tempExeDir = Path.Combine(Path.GetTempPath(), "pseudocode-ide\\updater");
            string tempExePath = Path.Combine(tempExeDir, "pseudocodeIdeUpdater.exe");

            // start the local copied exe
            using (Process compiler = new Process())
            {
                compiler.StartInfo.FileName = tempExePath;
                compiler.StartInfo.WorkingDirectory = tempExeDir;
                compiler.StartInfo.Arguments = $"\"{currentDir}\" \"{firstRun}\" \"{beta}\"";
                compiler.StartInfo.UseShellExecute = true;
                compiler.Start();
            }
        }

        private void UpdateBetaMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdate(false, true);
        }
    }
}
