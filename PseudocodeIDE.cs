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
                MessageBox.Show("Yes");
                
            } else { wordWrapMenuItem.Checked = false; MessageBox.Show("No"); }
              
        }   
    }
}
