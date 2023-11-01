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

        private string findWhat = "";

        private string replaceWith = "";

        private bool matchCase = false;

        public FindReplaceForm(PseudocodeIDEForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();

            btFindNext1.Click += (ignored1, ignored2) => this.findNext();
            btFindNext2.Click += (ignored1, ignored2) => this.findNext();

            btCount.Click += (ignored1, ignored2) => this.count();

            btReplace.Click += (ignored1, ignored2) => this.replace();
            btReplaceAll.Click += (ignored1, ignored2) => this.replaceAll();

            tbFindWhat1.TextChanged += (ignored1, ignored2) => this.findWhat = tbFindWhat1.Text;
            tbFindWhat2.TextChanged += (ignored1, ignored2) => this.findWhat = tbFindWhat2.Text;

            tbReplaceWith.TextChanged += (ignored1, ignored2) => this.replaceWith = tbReplaceWith.Text;

            cbMatchCase1.CheckedChanged += (ignored1, ignored2) => this.matchCase = cbMatchCase1.Checked;
            cbMatchCase2.CheckedChanged += (ignored1, ignored2) => this.matchCase = cbMatchCase2.Checked;
        }

        public void Show(FindReplaceTabs tab, string findWhat = "")
        {
            tabControl.SelectedIndex = (int) tab;

            Show();
            Focus();

            this.findWhat = findWhat != null && !findWhat.Equals("") ? findWhat : this.findWhat;

            this.pageSwitch((int) tab);
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

        private void FindReplaceForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                this.findNext();
            }
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            this.pageSwitch(e.TabPageIndex);
        }

        private void pageSwitch(int pageIndex)
        {
            Debug.WriteLine(pageIndex);
            switch (pageIndex)
            {
                case (int)FindReplaceTabs.REPLACE:
                    tbFindWhat2.Text = this.findWhat;
                    cbMatchCase2.Checked = this.matchCase;
                    tbFindWhat2.Select();
                    tbFindWhat2.SelectAll();
                    break;

                case (int)FindReplaceTabs.FIND:
                default:
                    tbFindWhat1.Text = this.findWhat;
                    cbMatchCase1.Checked = this.matchCase;
                    tbFindWhat1.Select();
                    tbFindWhat1.SelectAll();
                    break;
            }
        }

        public void findNext()
        {
            string code = this.matchCase ? this.mainForm.code : this.mainForm.code.ToLower();
            string findWhat = this.matchCase ? this.findWhat : this.findWhat.ToLower();

            int firstOccurence = code.IndexOf(findWhat, this.mainForm.getSelectionEnd());

            if (firstOccurence < 0)
            {
                if (this.mainForm.getSelectionEnd() != 0)
                {
                    statusLabel.Text = $"Find: The end of the document was reached. Continued on the top.";
                    statusLabel.ForeColor = Color.Green;

                    // wrap around
                    this.mainForm.selectText(0, 0);
                    firstOccurence = code.IndexOf(findWhat, this.mainForm.getSelectionEnd());
                }

                if (firstOccurence < 0)
                {
                    statusLabel.Text = $"Find: Unable to find \"{this.findWhat}\" in the code.";
                    statusLabel.ForeColor = Color.Red;
                    return;
                }
            }
            else
            {
                statusLabel.Text = "";
                statusLabel.ForeColor = SystemColors.ControlText;
            }

            this.mainForm.selectText(firstOccurence, findWhat.Length);
        }

        public void count()
        {
            string code = this.matchCase ? this.mainForm.code : this.mainForm.code.ToLower();
            string findWhat = this.matchCase ? this.findWhat : this.findWhat.ToLower();

            statusLabel.Text = $"Count: Found {code.AllIndexesOf(findWhat).Count} occurrences in the code.";
            statusLabel.ForeColor = Color.Blue;
        }

        public void replace()
        {

        }

        public void replaceAll()
        {

        }
    }
}
