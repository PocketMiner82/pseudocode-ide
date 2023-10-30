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
    }
}
