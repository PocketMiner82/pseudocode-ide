using pseudocode_ide.interpreter.sequences;
using pseudocode_ide.interpreter;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.LinkLabel;
using System;
using System.Data;

namespace pseudocode_ide
{
    public partial class PseudocodeIDE : Form
    {
        private Interpreter interpreter = new Interpreter();
        private string filePath;

        public PseudocodeIDE()
        {
            InitializeComponent();
        }
        
        private void wordWrapMenuItem_Click(object sender, System.EventArgs e)
        {
            codeTextBox.WordWrap = wordWrapMenuItem.Checked;
        }

        private void EqualsIsOperatorMenuItem_Click(object sender, System.EventArgs e)
        {
            Tokens.setEqualOperatorForCompare(EqualsIsOperatorMenuItem.Checked);
        }

        private void codeTextBox_TextChanged(object sender, System.EventArgs e)
        {
            interpreter.code = codeTextBox.Text;
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
        
        public bool openFile()
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
                    StreamReader file = new StreamReader(this.filePath);
                    codeTextBox.Text = file.ReadToEnd();
                    return true;
                }
            }
            return false;
        }
        private void newMenuItem_Click(object sender, System.EventArgs e)
        {
            codeTextBox.Clear();
        }

        private void openMenuItem_Click(object sender, System.EventArgs e)
        {
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
        }
    }
}
