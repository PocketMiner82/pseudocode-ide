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

            btFindNext1.Click += (ignored1, ignored2) => this.findNextAndUpdateStatus();
            btFindNext2.Click += (ignored1, ignored2) => this.findNextAndUpdateStatus();

            btCount.Click += (ignored1, ignored2) => this.count();

            btReplace.Click += (ignored1, ignored2) => this.replaceNextAndUpdateStatus();
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
            statusLabel.Text = "";
            statusLabel.ForeColor = SystemColors.ControlText;

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
                this.findNextAndUpdateStatus();
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

        public void findNextAndUpdateStatus()
        {
            switch (this.findNext())
            {
                case FindReplaceResult.WRAPPED:
                    statusLabel.Text = $"Find: The end of the document was reached. Continued at the top.";
                    statusLabel.ForeColor = Color.Green;
                    break;

                case FindReplaceResult.NOTHING_FOUND:
                    statusLabel.Text = $"Find: Unable to find \"{this.findWhat}\" in the code.";
                    statusLabel.ForeColor = Color.Red;
                    break;

                default:
                    statusLabel.Text = "";
                    statusLabel.ForeColor = SystemColors.ControlText;
                    break;
            }
        }

        public FindReplaceResult findNext()
        {
            string code = this.matchCase ? this.mainForm.code : this.mainForm.code.ToLower();
            string findWhat = this.matchCase ? this.findWhat : this.findWhat.ToLower();
            FindReplaceResult result = FindReplaceResult.NONE;

            int firstOccurence = code.IndexOf(findWhat, this.mainForm.getSelectionEnd());

            if (firstOccurence < 0)
            {
                if (this.mainForm.getSelectionEnd() != 0)
                {
                    result = FindReplaceResult.WRAPPED;

                    // wrap around
                    this.mainForm.selectText(0, 0);
                    firstOccurence = code.IndexOf(findWhat, this.mainForm.getSelectionEnd());
                }

                if (firstOccurence < 0)
                {
                    result = FindReplaceResult.NOTHING_FOUND;
                    return result;
                }
            }

            this.mainForm.selectText(firstOccurence, findWhat.Length);
            return result;
        }

        public void count()
        {
            string code = this.matchCase ? this.mainForm.code : this.mainForm.code.ToLower();
            string findWhat = this.matchCase ? this.findWhat : this.findWhat.ToLower();

            statusLabel.Text = $"Count: Found {code.allIndexesOf(findWhat).Count} occurrences in the code.";
            statusLabel.ForeColor = Color.Blue;
        }

        public void replaceNextAndUpdateStatus()
        {
            switch (this.replaceNext())
            {
                case FindReplaceResult.WRAPPED:
                    statusLabel.Text = $"Find: The end of the document was reached. Continued on the top.";
                    statusLabel.ForeColor = Color.Green;
                    break;

                case FindReplaceResult.REPLACED_WRAPPED:
                    statusLabel.Text = $"Replace: Replaced 1 occurrence. Continued at the top.";
                    statusLabel.ForeColor = Color.Blue;
                    break;

                case FindReplaceResult.NOTHING_FOUND:
                    statusLabel.Text = $"Find: Unable to find \"{this.findWhat}\" in the code.";
                    statusLabel.ForeColor = Color.Red;
                    break;

                case FindReplaceResult.NOTHING_MORE_FOUND:
                    statusLabel.Text = $"Replace: Replaced the last occurence.";
                    statusLabel.ForeColor = Color.Blue;
                    break;

                case FindReplaceResult.REPLACED:
                    statusLabel.Text = $"Replace: Replaced 1 occurrence.";
                    statusLabel.ForeColor = Color.Blue;
                    break;

                default:
                    statusLabel.Text = "";
                    statusLabel.ForeColor = SystemColors.ControlText;
                    break;
            }
        }

        public FindReplaceResult replaceNext()
        {
            if (this.mainForm.getSelectionLength() == 0)
            {
                return this.findNext();
            }

            this.mainForm.setSelectedText(this.replaceWith);
            this.mainForm.updateUndoStack(true);

            FindReplaceResult result = this.findNext();

            switch (result)
            {
                case FindReplaceResult.NOTHING_FOUND:
                    result = FindReplaceResult.NOTHING_MORE_FOUND;
                    break;
                case FindReplaceResult.NONE:
                    result = FindReplaceResult.REPLACED;
                    break;
                case FindReplaceResult.WRAPPED:
                    result = FindReplaceResult.REPLACED_WRAPPED;
                    break;
                default:
                    break;
            }

            return result;
        }

        public void replaceAll()
        {
            string code = this.matchCase ? this.mainForm.code : this.mainForm.code.ToLower();
            string findWhat = this.matchCase ? this.findWhat : this.findWhat.ToLower();
            int count = code.allIndexesOf(findWhat).Count;

            if (count == 0)
            {
                statusLabel.Text = $"Find: Unable to find \"{this.findWhat}\" in the code.";
                statusLabel.ForeColor = Color.Red;
                return;
            }

            this.mainForm.selectText(0, 0);
            
            this.findNext();

            // the count - 1 is needed to allow undo to work correctly
            this.mainForm.noNewUndoPoint = true;
            for (int i = 0; i < (count - 1); i++)
            {
                this.replaceNext();
            }
            this.mainForm.noNewUndoPoint = false;
            this.replaceNext();

            statusLabel.Text = $"Replace: Replaced {count} occurrence(s).";
            statusLabel.ForeColor = Color.Blue;
        }
    }
}
