using pseudocode_ide.interpreter.sequences;
using System.Windows.Forms;

namespace pseudocode_ide
{
    public partial class PseudocodeIDE : Form
    {   
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
    }
}
