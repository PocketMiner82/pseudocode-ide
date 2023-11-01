using pseudocode_ide.interpreter.sequences;
using pseudocode_ide.interpreter;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.LinkLabel;
using System;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace pseudocode_ide
{
    public partial class PseudocodeIDE : Form
    {
        private Stack<string> undoStack = new Stack<string>();
        private Stack<string> redoStack = new Stack<string>();

        private Interpreter interpreter = new Interpreter();
        private string filePath { get; set; }


        private bool isSaved = true;
        private int lastCursorPosition = 0;
        private bool ignoreTextChange = false;

        public PseudocodeIDE()
        {
            InitializeComponent();
            this.resetUndoRedo();
        }
        
        private void wordWrapMenuItem_Click(object sender, System.EventArgs e)
        {
            codeTextBox.WordWrap = wordWrapMenuItem.Checked;
        }

        private void EqualsIsOperatorMenuItem_Click(object sender, System.EventArgs e)
        {
            Tokens.setEqualsIsCompareOperator(EqualsIsOperatorMenuItem.Checked);
        }

        private void codeTextBox_TextChanged(object sender, System.EventArgs e)
        {
            interpreter.code = codeTextBox.Text;
            this.isSaved = false;


            if (!Text.EndsWith("*"))
            {
                Text += "*";
            }

            if (this.ignoreTextChange)
            {
                this.ignoreTextChange = false;
            }
            else
            {
                this.updateUndoStack(false);
            }
        }
        public bool saveNewFile()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Pseudocode File (*.pseudocode)|*.pseudocode|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Title = "New Pseudocode File";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // if the file already exists, we need to overwrite it
                    if (File.Exists(saveFileDialog.FileName))
                    {
                        File.Delete(saveFileDialog.FileName);
                    }
                    
                    this.filePath = saveFileDialog.FileName;

                    // get the path of specified file
                    this.saveFile();
                    return true;
                }
                return false;
            }
        }
        
        public void openFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Pseudocode File (*.pseudocode)|*.pseudocode|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Open Pseudocode File"; ;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.filePath = openFileDialog.FileName;

                    using (StreamReader file = new StreamReader(this.filePath))
                    {
                        int oldSelectionStart = codeTextBox.SelectionStart;
                        this.ignoreTextChange = true;
                        codeTextBox.Text = file.ReadToEnd();
                        this.resetUndoRedo();
                        codeTextBox.SelectionStart = Math.Min(oldSelectionStart, codeTextBox.TextLength);
                    }
                    this.isSaved = true;
                    Text = "Pseudocode IDE - " + this.filePath;
                }
            }
        }
        private void newMenuItem_Click(object sender, System.EventArgs e)
        {
            if (!this.isSaved)
            {
                if (MessageBox.Show("Do you really want to create a new file?\nAll unsaved changes will be lost.", "Unsaved changes",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            
            this.ignoreTextChange = true;
            codeTextBox.Clear();
            this.resetUndoRedo();
            Text = "Pseudocode IDE - New File";
            this.isSaved = true;
            this.filePath = "";
        }

        private void resetUndoRedo()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
            this.undoStack.Push(codeTextBox.Text);
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;
        }

        private void openMenuItem_Click(object sender, System.EventArgs e)
        {
            if (!this.isSaved)
            {
                if (MessageBox.Show("Do you really want to open a new file?\nAll unsaved changes will be lost.", "Unsaved changes",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            this.openFile();
        }

        private void saveMenuItem_Click(object sender, System.EventArgs e)
        {
            if(this.filePath == null || this.filePath.Trim() == "")
            {
               saveNewFile();
            }
            else
            {
                saveFile();
            }
           
        }

        private void saveFile()
        {
            using (StreamWriter outputFile = new StreamWriter(this.filePath))
            {
                outputFile.Write(codeTextBox.Text);
            }
            this.isSaved = true;

            Text = "Pseudocode IDE - " + this.filePath;
        }

        private void PseudocodeIDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.isSaved)
            {
                if (MessageBox.Show("Do you want to save them?", "Unsaved changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.saveMenuItem_Click(null, null);
                }
            }
        }

        private void codeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // ignore CTRL[+SHIFT]+(Z/Y/L/R/E)
            if ((e.KeyCode == Keys.Z || e.KeyCode == Keys.Y || e.KeyCode == Keys.L || e.KeyCode == Keys.R || e.KeyCode == Keys.E)
                && (Control.ModifierKeys == Keys.Control || Control.ModifierKeys == (Keys.Control | Keys.Shift)))
            {
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                this.ignoreTextChange = true;
                this.updateUndoStack(true);
            }
        }

        private void updateUndoStack(bool forceUpdate)
        {
            undoToolStripMenuItem.Enabled = true;
            if (this.redoStack.Count != 0)
            {
                this.redoStack.Clear();
                redoToolStripMenuItem.Enabled = false;
            }

            Regex noUpdateAfter = new Regex(@"^[a-zA-Z0-9_]*$");
            string undoText = codeTextBox.Text;

            try
            {
                if (!forceUpdate)
                {
                    undoText = undoText.Remove(codeTextBox.SelectionStart - 1, 1);
                }
            }
            catch (ArgumentOutOfRangeException) {
                return;
            }

            if ((codeTextBox.SelectionStart > 1 && !forceUpdate && noUpdateAfter.IsMatch(undoText.ElementAt(codeTextBox.SelectionStart - 2).ToString()))
                || this.undoStack.Peek().Equals(undoText))
            {
                return;
            }

            this.redoStack.Clear();
            this.undoStack.Push(undoText);
            this.undoStack = this.undoStack.Trim(250);
            undoToolStripMenuItem.Enabled = true;
        }

        private void codeTextBox_SelectionChanged(object sender, EventArgs e)
        {
            if (this.redoStack.Count != 0)
            {
                this.lastCursorPosition = codeTextBox.SelectionStart;
                return;
            }

            if (codeTextBox.SelectionLength != 0 || Math.Abs(this.lastCursorPosition - codeTextBox.SelectionStart) > 1)
            {
                this.updateUndoStack(true);
            }
            this.lastCursorPosition = codeTextBox.SelectionStart;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.redoStack.Count == 0 || !this.redoStack.Peek().Equals(codeTextBox.Text))
            {
                this.redoStack.Push(codeTextBox.Text);
                redoToolStripMenuItem.Enabled = true;
            }

            if (this.undoStack.Count <= 1)
            {
                this.ignoreTextChange = true;
                int oldSelectionStart = codeTextBox.SelectionStart;
                codeTextBox.Text = this.undoStack.Peek();
                codeTextBox.SelectionStart = Math.Min(oldSelectionStart, codeTextBox.TextLength);

                undoToolStripMenuItem.Enabled = false;
            }
            else
            {
                string currentText = codeTextBox.Text;
                do
                {
                    this.ignoreTextChange = true;
                    int oldSelectionStart = codeTextBox.SelectionStart;
                    codeTextBox.Text = this.undoStack.Peek();
                    codeTextBox.SelectionStart = Math.Min(oldSelectionStart, codeTextBox.TextLength);
                } while (currentText.Equals(this.undoStack.Pop()));
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = true;
            if (!this.undoStack.Peek().Equals(codeTextBox.Text))
            {
                this.undoStack.Push(codeTextBox.Text);
            }

            if (this.redoStack.Count > 0)
            {
                this.ignoreTextChange = true;
                int oldSelectionStart = codeTextBox.SelectionStart;
                codeTextBox.Text = this.redoStack.Pop();
                codeTextBox.SelectionStart = Math.Min(oldSelectionStart, codeTextBox.TextLength);
            }

            if (this.redoStack.Count == 0)
            {
                redoToolStripMenuItem.Enabled = false;
            }
        }
    }
}
