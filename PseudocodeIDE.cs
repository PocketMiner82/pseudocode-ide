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

        private void codeTextBox_TextChanged(object sender, System.EventArgs e)
        {
            interpreter.code = codeTextBox.Text;
        }
    }
}
