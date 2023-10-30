using pseudocode_ide.interpreter.sequences;
using pseudocode_ide.interpreter;
using System.Windows.Forms;

namespace pseudocode_ide
{
    public partial class PseudocodeIDE : Form
    {
        private Interpreter interpreter = new Interpreter();

        public PseudocodeIDE()
        {
            InitializeComponent();
        }
        
        private void wordWrapMenuItem_Click(object sender, System.EventArgs e)
        {   
            
            if (wordWrapMenuItem.Checked)
            {
               wordWrapMenuItem.Checked = true;
               codeTextBox.WordWrap = true;
                
            } else { wordWrapMenuItem.Checked = false; codeTextBox.WordWrap = false; }
              
        }   

        private void codeTextBox_TextChanged(object sender, System.EventArgs e)
        {
            interpreter.code = codeTextBox.Text;
        }
    }
}
