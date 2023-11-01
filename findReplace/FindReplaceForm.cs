using pseudocode_ide.findReplace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pseudocode_ide
{
    public partial class FindReplaceForm : Form
    {
        private PseudocodeIDEForm mainForm;

        public FindReplaceForm(PseudocodeIDEForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }

        public void Show(FindReplaceTabs tab)
        {
            tabControl.SelectedIndex = (int) tab;

            Show();
            Focus();
        }

        private void FindReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Visible)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void FindReplaceForm_Activated(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

        private void FindReplaceForm_Deactivate(object sender, EventArgs e)
        {
            this.Opacity = 0.5;
        }

        private void FindReplaceForm_Resize(object sender, EventArgs e)
        {
            Debug.WriteLine($"Width: {Width}; Height: {Height}");
        }
    }
}
