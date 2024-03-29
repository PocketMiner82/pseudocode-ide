﻿using pseudocodeIde.findReplace;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace pseudocodeIde
{
    public partial class FindReplaceForm : Form
    {
        public PseudocodeIDEForm mainForm
        {
            get
            {
                return (PseudocodeIDEForm) Owner;
            }
        }

        /// <summary>
        /// the search term to find
        /// </summary>
        private string findWhat = "";

        /// <summary>
        /// the term to replace the search term with
        /// </summary>
        private string replaceWith = "";

        /// <summary>
        /// whether the searching should be case sensitive
        /// </summary>
        private bool matchCase = false;

        public FindReplaceForm(PseudocodeIDEForm mainForm)
        {
            Owner = mainForm;
            InitializeComponent();

            // register some event listeners

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

        // ---------------------------------------------
        // EVENTS
        // ---------------------------------------------

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
            this.pageSwitch((FindReplaceTabs)e.TabPageIndex);
        }

        // ---------------------------------------------
        // METHODS
        // ---------------------------------------------

        /// <summary>
        /// Show the form.
        /// </summary>
        /// <param name="tab">the tab to open</param>
        /// <param name="findWhat">the initial value of the findWhat textbox</param>
        public void Show(FindReplaceTabs tab, string findWhat = "")
        {
            tabControl.SelectedIndex = (int)tab;
            statusLabel.Text = " ";
            statusLabel.ForeColor = SystemColors.ControlText;

            Show();
            Focus();

            this.findWhat = findWhat != null && !findWhat.Equals("") ? findWhat : this.findWhat;

            this.pageSwitch(tab);
        }

        /// <summary>
        /// Go to a different tab
        /// </summary>
        /// <param name="tab">the tab to switch to</param>
        private void pageSwitch(FindReplaceTabs tab)
        {
            Debug.WriteLine(tab);
            switch (tab)
            {
                case FindReplaceTabs.REPLACE:
                    tbFindWhat2.Text = this.findWhat;
                    cbMatchCase2.Checked = this.matchCase;
                    tbFindWhat2.Select();
                    tbFindWhat2.SelectAll();
                    break;

                case FindReplaceTabs.FIND:
                default:
                    tbFindWhat1.Text = this.findWhat;
                    cbMatchCase1.Checked = this.matchCase;
                    tbFindWhat1.Select();
                    tbFindWhat1.SelectAll();
                    break;
            }
        }

        /// <summary>
        /// Find the next occurrence and update the status bar.
        /// </summary>
        public void findNextAndUpdateStatus()
        {
            switch (this.findNext())
            {
                case FindReplaceResult.WRAPPED:
                    statusLabel.Text = $"Suchen: Ende des Dokuments erreicht. Fortsetzung am Anfang.";
                    statusLabel.ForeColor = Color.Green;
                    break;

                case FindReplaceResult.NOTHING_FOUND:
                    statusLabel.Text = $"Suchen: Konnte \"{this.findWhat}\" nicht finden.";
                    statusLabel.ForeColor = Color.Red;
                    break;

                default:
                    statusLabel.Text = " ";
                    statusLabel.ForeColor = SystemColors.ControlText;
                    break;
            }
        }

        /// <summary>
        /// Find the next occurrence.
        /// </summary>
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

        /// <summary>
        /// Count the occurrences of a search term and update the status bar.
        /// </summary>
        public void count()
        {
            string code = this.matchCase ? this.mainForm.code : this.mainForm.code.ToLower();
            string findWhat = this.matchCase ? this.findWhat : this.findWhat.ToLower();

            statusLabel.Text = $"Zählen: {code.allIndexesOf(findWhat).Count} Vorkommen gefunden.";
            statusLabel.ForeColor = Color.Blue;
        }

        /// <summary>
        /// Replace the current occurrence, find the next and update the status bar.
        /// If there isn't a current occurence, just find the next.
        /// </summary>
        public void replaceNextAndUpdateStatus()
        {
            switch (this.replaceNext())
            {
                case FindReplaceResult.WRAPPED:
                    statusLabel.Text = $"Suchen: Ende des Dokuments erreicht. Fortsetzung am Anfang.";
                    statusLabel.ForeColor = Color.Green;
                    break;

                case FindReplaceResult.REPLACED_WRAPPED:
                    statusLabel.Text = $"Ersetzen: 1 Vorkommen ersetzt. Fortsetzung am Anfang.";
                    statusLabel.ForeColor = Color.Blue;
                    break;

                case FindReplaceResult.NOTHING_FOUND:
                    statusLabel.Text = $"Suchen: Konnte \"{this.findWhat}\" nicht finden.";
                    statusLabel.ForeColor = Color.Red;
                    break;

                case FindReplaceResult.NOTHING_MORE_FOUND:
                    statusLabel.Text = $"Ersetzen: Letztes Vorkommen ersetzt.";
                    statusLabel.ForeColor = Color.Blue;
                    break;

                case FindReplaceResult.REPLACED:
                    statusLabel.Text = $"Ersetzen: 1 Vorkommen ersetzt.";
                    statusLabel.ForeColor = Color.Blue;
                    break;

                default:
                    statusLabel.Text = " ";
                    statusLabel.ForeColor = SystemColors.ControlText;
                    break;
            }
        }

        /// <summary>
        /// Replace the current occurrence and find the next.
        /// If there isn't a current occurence, just find the next.
        /// </summary>
        public FindReplaceResult replaceNext()
        {
            // if selection doesn't match search term, find next
            if ((!this.matchCase && !this.mainForm.getSelection().ToLower().Equals(this.findWhat.ToLower()))
                || (this.matchCase && !this.mainForm.getSelection().Equals(this.findWhat)))
            {
                this.mainForm.selectText(this.mainForm.getSelectionStart(), 0);
                return this.findNext();
            }

            // the selected text will be the search result, so replace it
            this.mainForm.setSelectedText(this.replaceWith);
            this.mainForm.updateUndoStack(true);

            // find the next result
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

        /// <summary>
        /// Replace all occurrencea and update the status bar.
        /// </summary>
        public void replaceAll()
        {
            string code = this.matchCase ? this.mainForm.code : this.mainForm.code.ToLower();
            string findWhat = this.matchCase ? this.findWhat : this.findWhat.ToLower();
            int count = code.allIndexesOf(findWhat).Count;

            // no result found
            if (count == 0)
            {
                statusLabel.Text = $"Suchen: Konnte \"{this.findWhat}\" nicht finden.";
                statusLabel.ForeColor = Color.Red;
                return;
            }

            // go to beginning
            this.mainForm.selectText(0, 0);
            
            // find the first occurence
            this.findNext();

            // loop through the occurences and replace them all, the count - 1 is needed to allow undo to work correctly
            this.mainForm.noNewUndoPoint = true;
            for (int i = 0; i < (count - 1); i++)
            {
                this.replaceNext();
            }
            this.mainForm.noNewUndoPoint = false;
            this.replaceNext();

            statusLabel.Text = $"Ersetzen: {count} Vorkommen ersetzt.";
            statusLabel.ForeColor = Color.Blue;
        }
    }
}
