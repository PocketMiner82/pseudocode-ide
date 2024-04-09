using pseudocodeIde.interpreter;
using System.Windows.Forms;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using pseudocodeIde.findReplace;
using pseudocode_ide;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using System.Reflection;
using ScintillaNET;
using pseudocode_ide.interpreter.scanner;
using System.Drawing;

namespace pseudocodeIde
{
    public partial class PseudocodeIDEForm : Form
    {
        /// <summary>
        /// the maximum steps to save in the undo stack
        /// </summary>
        private const int MAX_UNDO_SIZE = 250;

        /// <summary>
        /// do not update the undo stack when the last char is a letter from A-Z (case ignored), a number from 0-9 or a underscore
        /// </summary>
        private readonly Regex NO_UPDATE_AFTER = new Regex(@"^[a-zA-Z0-9_äöüÄÖÜß]$", RegexOptions.Multiline);

        /// <summary>
        /// the code currently in the textbox
        /// </summary>
        public string code
        {
            get
            {
                return (string)Invoke((Func<string>)delegate
                {
                    return codeTextBox.Text == null ? "" : codeTextBox.Text;
                });
            }
        }

        /// <summary>
        /// if the textbox update event should be ignored the next time
        /// </summary>
        public bool ignoreTextChange { get; set; } = false;

        /// <summary>
        /// if the undo events should be ignored
        /// </summary>
        public bool noNewUndoPoint { get; set; } = false;

        /// <summary>
        /// last cursor position before cursor move
        /// </summary>
        private int lastCursorPosition = 0;

        // undo and redo stacks
        private Stack<string> undoStack = new Stack<string>();
        private Stack<string> redoStack = new Stack<string>();

        /// <summary>
        /// the path where this file is saved
        /// </summary>
        private string filePath;

        /// <summary>
        /// is the code saved?
        /// </summary>
        private bool isSaved = true;

        private FindReplaceForm findReplaceForm;

        private OutputForm outputForm;

        private int maxLineNumberCharLength;


        public PseudocodeIDEForm()
        {
            this.findReplaceForm = new FindReplaceForm(this);
            this.outputForm = new OutputForm(this);

            InitializeComponent();
            this.resetUndoRedo();

            // disable right click menu
            codeTextBox.UsePopup(false);

            this.codeTextBox_TextChanged(null, null);
            // on first start, the code is always saved
            this.setFileSaved();
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
            this.checkForUpdate(true);

            // set font
            this.configureCodeTextBox();
        }

        private void configureCodeTextBox()
        {
            codeTextBox.StyleResetDefault();
            codeTextBox.Styles[Style.Default].Font = "Courier New";
            codeTextBox.StyleClearAll();

            codeTextBox.Styles[SyntaxHighlightingLexer.STYLE_DEFAULT].ForeColor = Color.DarkGray;

            codeTextBox.Styles[SyntaxHighlightingLexer.STYLE_KEYWORD].ForeColor = Color.Blue;
            codeTextBox.Styles[SyntaxHighlightingLexer.STYLE_KEYWORD].Bold = true;

            codeTextBox.Styles[SyntaxHighlightingLexer.STYLE_IDENTIFIER].ForeColor = Color.Black;

            codeTextBox.Styles[SyntaxHighlightingLexer.STYLE_NUMBER].ForeColor = Color.Peru;

            codeTextBox.Styles[SyntaxHighlightingLexer.STYLE_STRING].ForeColor = Color.OrangeRed;

            codeTextBox.Styles[SyntaxHighlightingLexer.STYLE_ESCAPE].ForeColor = Color.Orange;
            codeTextBox.Styles[SyntaxHighlightingLexer.STYLE_ESCAPE].Bold = true;

            codeTextBox.Styles[SyntaxHighlightingLexer.STYLE_COMMENT].ForeColor = Color.Green;

            codeTextBox.LexerName = "";
            codeTextBox.StyleNeeded += codeTextBox_StyleNeeded;
        }

        private void codeTextBox_StyleNeeded(object sender, StyleNeededEventArgs e)
        {
            int startPos = codeTextBox.GetEndStyled();
            int endPos = e.Position;

            SyntaxHighlightingLexer.style(codeTextBox, startPos, endPos);
        }

        private void wordWrapMenuItem_Click(object sender, EventArgs e)
        {
            codeTextBox.WrapMode = wordWrapMenuItem.Checked ? WrapMode.Word : WrapMode.None;
        }

        private void singleEqualIsCompareOperatorMenuItem_Click(object sender, EventArgs e)
        {
            Scanner.singleEqualIsCompareOperator = singleEqualIsCompareOperatorMenuItem.Checked;

            this.setFileNotSaved();
        }

        private void PseudocodeIDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            // main form won't close if child form is not disposed
            if (!this.findReplaceForm.IsDisposed)
            {
                this.findReplaceForm.Close();
                this.Close();
                return;
            }
            if (!this.outputForm.IsDisposed)
            {
                this.outputForm.Close();
                this.Close();
                return;
            }

            if (!this.isSaved)
            {
                if (MessageBox.Show("Möchtest du deine ungespeicherten Änderungen speichern?", "Ungespeicherte Änderungen", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.saveMenuItem_Click(null, null);
                }
            }
        }

        // ---------------------------------------------
        // CODE TEXTBOX EVENT LISTENERS
        // ---------------------------------------------

        private void userUpdatedText()
        {
            // only update undo stack if next event not ignored
            if (this.ignoreTextChange)
            {
                this.ignoreTextChange = false;
            }
            else
            {
                this.updateUndoStack(false);
            }
        }

        private void codeTextBox_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            switch (e.Change)
            {
                case UpdateChange.Content:
                    this.userUpdatedText();
                    break;
                case UpdateChange.Selection:
                    this.userChangedSelection();
                    break;
            }
        }

        private void codeTextBox_TextChanged(object sender, EventArgs e)
        {
            // when the code is modified, the code is no longer saved in the file
            this.setFileNotSaved();

            // Did the number of characters in the line number display change?
            int maxLineNumberCharLength = codeTextBox.Lines.Count.ToString().Length;
            if (maxLineNumberCharLength == this.maxLineNumberCharLength)
                return;

            // calculate the width required to display the last line number
            // + include some padding for good measure
            const int padding = 2;
            codeTextBox.Margins[0].Width = codeTextBox.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
            this.maxLineNumberCharLength = maxLineNumberCharLength;
        }

        private void codeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // ignore CTRL[+SHIFT]+(Z/Y/L/R/E/S)
            if ((e.KeyCode == Keys.Z || e.KeyCode == Keys.Y || e.KeyCode == Keys.L || e.KeyCode == Keys.R || e.KeyCode == Keys.E || e.KeyCode == Keys.S)
                && (Control.ModifierKeys == Keys.Control || Control.ModifierKeys == (Keys.Control | Keys.Shift)))
            {
                e.SuppressKeyPress = true;
                return;
            }

            // deleting always updates undo stack
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                this.ignoreTextChange = true;
                this.updateUndoStack(true);
                e.SuppressKeyPress = false;
                return;
            }
        }

        private void codeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // keep tab indentation from previous line on new line
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;

                int currentLineIndex = codeTextBox.CurrentLine - 1;
                string currentLine = codeTextBox.Lines[currentLineIndex].Text;
                int indentationLevel = currentLine.getIndentationLevel();

                string indentation = new string('\t', indentationLevel);

                codeTextBox.AddText(indentation);
            }
        }

        // adapted from https://github.com/jacobslusser/ScintillaNET/wiki/Character-Autocompletion#finishing-touch
        private void codeTextBox_CharAdded(object sender, CharAddedEventArgs e)
        {
            int caretPos = codeTextBox.CurrentPosition;
            bool docStart = caretPos == 1;
            bool docEnd = caretPos == codeTextBox.Text.Length;

            int charPrev = docStart ? codeTextBox.GetCharAt(caretPos) : codeTextBox.GetCharAt(caretPos - 2);
            int charNext = codeTextBox.GetCharAt(caretPos);

            bool isCharPrevBlank = charPrev == ' ' || charPrev == '\t' ||
                                    charPrev == '\n' || charPrev == '\r';

            bool isCharNextBlank = charNext == ' ' || charNext == '\t' ||
                                    charNext == '\n' || charNext == '\r' ||
                                    docEnd;

            bool isEnclosed = (charPrev == '(' && charNext == ')') ||
                                    (charPrev == '{' && charNext == '}') ||
                                    (charPrev == '[' && charNext == ']');

            bool isSpaceEnclosed = (charPrev == '(' && isCharNextBlank) || (isCharPrevBlank && charNext == ')') ||
                                    (charPrev == '{' && isCharNextBlank) || (isCharPrevBlank && charNext == '}') ||
                                    (charPrev == '[' && isCharNextBlank) || (isCharPrevBlank && charNext == ']');

            bool isCharOrString = docStart || (isCharPrevBlank && isCharNextBlank) || isEnclosed || isSpaceEnclosed;

            bool charNextIsCharOrString = charNext == '"' || charNext == '\'';

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

                    if (isCharOrString)
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

                    if (isCharOrString)
                    {
                        codeTextBox.InsertText(caretPos, "'");
                    }
                    break;
            }
        }

        private void userChangedSelection()
        {
            // ignore when we already undid something
            if (this.redoStack.Count != 0)
            {
                this.lastCursorPosition = codeTextBox.SelectionStart;
                return;
            }

            // update undo stack when user selected something or cursor moved by more then one since last time
            if (codeTextBox.SelectionEnd - codeTextBox.SelectionStart != 0 || Math.Abs(this.lastCursorPosition - codeTextBox.SelectionStart) > 1)
            {
                this.updateUndoStack(true);
            }
            this.lastCursorPosition = codeTextBox.SelectionStart;

            this.highlightWord(codeTextBox.SelectedText);
        }

        // adapted from https://github.com/desjarlais/Scintilla.NET/wiki/Find-and-Highlight-Words
        private void highlightWord(string text)
        {
            // Indicators 0-7 could be in use by a lexer
            // so we'll use indicator 8 to highlight words.
            const int NUM = 8;

            // Remove all uses of our indicator
            codeTextBox.IndicatorCurrent = NUM;
            codeTextBox.IndicatorClearRange(0, codeTextBox.TextLength);

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            // Update indicator appearance
            codeTextBox.Indicators[NUM].Style = IndicatorStyle.StraightBox;
            codeTextBox.Indicators[NUM].Under = true;
            codeTextBox.Indicators[NUM].ForeColor = Color.Lime;
            codeTextBox.Indicators[NUM].OutlineAlpha = 127;
            codeTextBox.Indicators[NUM].Alpha = 127;

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

        private void newMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.isSaved)
            {
                if (MessageBox.Show("Möchtest du wirklich eine neue Datei erstellen?\n" +
                    "Alle ungespeicherten Änderungen gehen verloren!", "Ungespeicherte Änderungen",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            // create new file
            this.ignoreTextChange = true;
            codeTextBox.ClearAll();
            this.resetUndoRedo();
            Text = "Pseudocode IDE - Neue Datei";
            this.filePath = "";
            this.setFileSaved();
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.isSaved)
            {
                if (MessageBox.Show("Möchtest du wirklich eine andere Datei öffnen?\n" +
                    "Alle ungespeicherten Änderungen gehen verloren!", "Ungespeicherte Änderungen",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            this.openFileDialog();
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            // if this is a new file
            if (this.filePath == null || this.filePath.Trim() == "")
            {
                saveFileDialog();
            }
            else
            {
                saveFile();
            }

        }

        private void setFileSaved()
        {
            this.isSaved = true;
            saveMenuItem.Enabled = false;
            if (Text.EndsWith("*"))
            {
                Text = Text.Substring(0, Text.Length - 1);
            }
        }

        private void setFileNotSaved()
        {
            this.isSaved = false;
            if (!Text.EndsWith("*"))
            {
                Text += "*";
            }
            saveMenuItem.Enabled = true;
        }

        public bool saveFileDialog()
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
                    
                    this.filePath = saveFileDialog.FileName;

                    // save the file
                    this.saveFile();
                    return true;
                }
                return false;
            }
        }

        private void saveFile()
        {
            PseudocodeFile pFile = new PseudocodeFile(Scanner.singleEqualIsCompareOperator, codeTextBox.Text.Split('\n'));

            // write to disk
            using (StreamWriter outputFile = new StreamWriter(this.filePath))
            {
                outputFile.Write(JsonConvert.SerializeObject(pFile, Formatting.Indented));
            }

            this.setFileSaved();
            Text = "Pseudocode IDE - " + this.filePath;
        }

        public void openFileDialog()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Pseudocode Datei (*.pseudocode)|*.pseudocode|Alle Dateien (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Pseudocode Datei öffnen";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.filePath = openFileDialog.FileName;

                    // open the file
                    this.openFile();
                }
            }
        }

        private void openFile()
        {
            // read from disk
            using (StreamReader file = new StreamReader(this.filePath))
            {
                this.ignoreTextChange = true;
                PseudocodeFile pFile;
                try
                {
                    pFile = JsonConvert.DeserializeObject<PseudocodeFile>(file.ReadToEnd());
                }
                catch
                {
                    MessageBox.Show("Konnte den JSON Code in '" + this.filePath + "' nicht parsen.", "Fehler");
                    this.filePath = "";
                    this.ignoreTextChange = false;
                    return;
                }

                codeTextBox.Text = string.Join("\n", pFile.fileContent);
                singleEqualIsCompareOperatorMenuItem.Checked = pFile.singleEqualIsCompareOperator;

                this.resetUndoRedo();
            }

            this.setFileSaved();
            Text = "Pseudocode IDE - " + this.filePath;
        }

        // ---------------------------------------------
        // UNDO/REDO
        // ---------------------------------------------

        /// <summary>
        /// Resets the undo and redo system 
        /// </summary>
        private void resetUndoRedo()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
            this.undoStack.Push(codeTextBox.Text);
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Updates the undo stack
        /// </summary>
        /// <param name="forceUpdate">if the update should be done without checking for whole word</param>
        public void updateUndoStack(bool forceUpdate)
        {
            // undo currently disabled?
            if (this.noNewUndoPoint)
            {
                return;
            }

            // when user writes something new, the redo stack will be cleared
            undoToolStripMenuItem.Enabled = true;
            if (this.redoStack.Count != 0)
            {
                this.redoStack.Clear();
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
            if ((codeTextBox.SelectionStart > 1 && !forceUpdate && NO_UPDATE_AFTER.IsMatch(undoText.ElementAt(codeTextBox.SelectionStart - 2).ToString()))
                || this.undoStack.Peek().Equals(undoText))
            {
                return;
            }

            // again, make sure that the redo stack is empty
            this.redoStack.Clear();

            // and update the undo stack, limit max undo size
            this.undoStack.Push(undoText);
            this.undoStack = this.undoStack.trim(MAX_UNDO_SIZE);
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // put current text in redo if not already in there
            if (this.redoStack.Count == 0 || !this.redoStack.Peek().Equals(codeTextBox.Text))
            {
                this.redoStack.Push(codeTextBox.Text);
                redoToolStripMenuItem.Enabled = true;
            }

            // the undo stack always has at least one item in it
            if (this.undoStack.Count <= 1)
            {
                // set the textbox text to the item without removing it from the stack
                this.ignoreTextChange = true;
                int oldSelectionStart = codeTextBox.SelectionStart;
                codeTextBox.Text = this.undoStack.Peek();
                codeTextBox.SelectionStart = Math.Min(oldSelectionStart, codeTextBox.TextLength);

                // no more things to undo
                undoToolStripMenuItem.Enabled = false;
            }
            else
            {
                string currentText = codeTextBox.Text;
                int oldSelectionStart = codeTextBox.SelectionStart;

                // set the textbox text to the first item that doesn't match the current text. also remove them from the undo stack
                do
                {
                    this.ignoreTextChange = true;
                    codeTextBox.Text = this.undoStack.Peek();
                } while (this.undoStack.Count > 1 && currentText.Equals(this.undoStack.Pop()));

                codeTextBox.SelectionStart = Math.Min(oldSelectionStart, codeTextBox.TextLength);
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = true;
            // put current text in undo if not already in there
            if (!this.undoStack.Peek().Equals(codeTextBox.Text))
            {
                this.undoStack.Push(codeTextBox.Text);
            }

            // set the textbox text to the top item of the redo stack. also remove it from the stack
            if (this.redoStack.Count > 0)
            {
                this.ignoreTextChange = true;
                int oldSelectionStart = codeTextBox.SelectionStart;
                codeTextBox.Text = this.redoStack.Pop();
                codeTextBox.SelectionStart = Math.Min(oldSelectionStart, codeTextBox.TextLength);
            }

            // no more things to redo
            if (this.redoStack.Count == 0)
            {
                redoToolStripMenuItem.Enabled = false;
            }
        }

        // ---------------------------------------------
        // FIND/REPLACE
        // ---------------------------------------------

        private void findMenuItem_Click(object sender, EventArgs e)
        {
            this.findReplaceForm.Show(FindReplaceTabs.FIND, codeTextBox.SelectedText);
        }

        private void replaceMenuItem_Click(object sender, EventArgs e)
        {
            this.findReplaceForm.Show(FindReplaceTabs.REPLACE, codeTextBox.SelectedText);
        }

        /// <summary>
        /// Get the start of the current selection in the code text box
        /// </summary>
        public int getSelectionStart()
        {
            return codeTextBox.SelectionStart;
        }

        /// <summary>
        /// Get the end of the current selection in the code text box
        /// </summary>
        public int getSelectionEnd()
        {
            return codeTextBox.SelectionEnd;
        }

        /// <summary>
        /// Get the selection length in the code text box
        /// </summary>
        public int getSelectionLength()
        {
            return codeTextBox.SelectionEnd - codeTextBox.SelectionStart;
        }

        /// <summary>
        /// Get the currently selected text.
        /// </summary>
        public string getSelection()
        {
            return codeTextBox.SelectedText;
        }

        /// <summary>
        /// Select text in the code text box. Will be invoked on the UI thread.
        /// </summary>
        /// <param name="selectionLength">start of the selection</param>
        /// <param name="selectionStart">length of the selection</param>
        public void selectText(int selectionStart, int selectionLength)
        {
            Invoke(new Action(() =>
            {
                codeTextBox.SelectionStart = selectionStart;
                codeTextBox.SelectionEnd = selectionStart + selectionLength;
            }));
        }

        /// <summary>
        /// Replace the selected text. Will be invoked on the UI thread.
        /// </summary>
        /// <param name="toReplace">The new text to replace</param>
        public void setSelectedText(string toReplace)
        {
            Invoke(new Action(() =>
            {
                codeTextBox.ReplaceSelection(toReplace);
            }));
        }

        // ---------------------------------------------
        // RUN PROGRAM
        // ---------------------------------------------

        private void runProgramMenuItem_Click(object sender, EventArgs e)
        {
            this.outputForm.ShowAndRun();
        }

        private void openOutputFormMenuItem_Click(object sender, EventArgs e)
        {
            this.outputForm.Show();
        }

        // ---------------------------------------------
        // SHOW HELP
        // ---------------------------------------------

        private void showHelpMenuItem_Click(object sender, EventArgs e)
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

        private void updatePseudocodeIDEMenuItem_Click(object sender, EventArgs e)
        {
            // dont show remind later
            this.checkForUpdate(false);
        }

        private void checkForUpdate(bool firstRun)
        {
            string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string tempExeDir = Path.Combine(Path.GetTempPath(), "pseudocode-ide\\updater");
            string tempExePath = Path.Combine(tempExeDir, "pseudocodeIdeUpdater.exe");

            // start the local copied exe
            using (Process compiler = new Process())
            {
                compiler.StartInfo.FileName = tempExePath;
                compiler.StartInfo.WorkingDirectory = tempExeDir;
                compiler.StartInfo.Arguments = $"\"{currentDir}\" \"{firstRun}\"";
                compiler.StartInfo.UseShellExecute = true;
                compiler.Start();
            }
        }
    }
}
