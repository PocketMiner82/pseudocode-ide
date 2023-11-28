namespace pseudocodeIde
{
    partial class PseudocodeIDEForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.findMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordWrapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runProgramMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openOutputFormMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.singleEqualIsCompareOperatorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeTextBox = new System.Windows.Forms.RichTextBox();
            this.updatePseudocodeIDEMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.viewMenuItem,
            this.runMenuItem,
            this.helpMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(6, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1176, 33);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenuItem,
            this.openMenuItem,
            this.saveMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(69, 29);
            this.fileMenuItem.Text = "Datei";
            // 
            // newMenuItem
            // 
            this.newMenuItem.Name = "newMenuItem";
            this.newMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newMenuItem.Size = new System.Drawing.Size(270, 34);
            this.newMenuItem.Text = "Neu";
            this.newMenuItem.Click += new System.EventHandler(this.newMenuItem_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMenuItem.Size = new System.Drawing.Size(270, 34);
            this.openMenuItem.Text = "Öffnen";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Enabled = false;
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveMenuItem.Size = new System.Drawing.Size(270, 34);
            this.saveMenuItem.Text = "Speichern";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator1,
            this.findMenuItem,
            this.replaceMenuItem});
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(111, 29);
            this.editMenuItem.Text = "Bearbeiten";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            this.undoToolStripMenuItem.Text = "Rückgängig";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(311, 34);
            this.redoToolStripMenuItem.Text = "Wiederherstellen";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(308, 6);
            // 
            // findMenuItem
            // 
            this.findMenuItem.Name = "findMenuItem";
            this.findMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findMenuItem.Size = new System.Drawing.Size(311, 34);
            this.findMenuItem.Text = "Suchen";
            this.findMenuItem.Click += new System.EventHandler(this.findMenuItem_Click);
            // 
            // replaceMenuItem
            // 
            this.replaceMenuItem.Name = "replaceMenuItem";
            this.replaceMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceMenuItem.Size = new System.Drawing.Size(311, 34);
            this.replaceMenuItem.Text = "Ersetzen";
            this.replaceMenuItem.Click += new System.EventHandler(this.replaceMenuItem_Click);
            // 
            // viewMenuItem
            // 
            this.viewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wordWrapMenuItem});
            this.viewMenuItem.Name = "viewMenuItem";
            this.viewMenuItem.Size = new System.Drawing.Size(86, 29);
            this.viewMenuItem.Text = "Ansicht";
            // 
            // wordWrapMenuItem
            // 
            this.wordWrapMenuItem.CheckOnClick = true;
            this.wordWrapMenuItem.Name = "wordWrapMenuItem";
            this.wordWrapMenuItem.Size = new System.Drawing.Size(320, 34);
            this.wordWrapMenuItem.Text = "Zeilenumbrüche aktivieren";
            this.wordWrapMenuItem.Click += new System.EventHandler(this.wordWrapMenuItem_Click);
            // 
            // runMenuItem
            // 
            this.runMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runProgramMenuItem,
            this.openOutputFormMenuItem,
            this.toolStripSeparator2,
            this.singleEqualIsCompareOperatorMenuItem});
            this.runMenuItem.Name = "runMenuItem";
            this.runMenuItem.Size = new System.Drawing.Size(109, 29);
            this.runMenuItem.Text = "Ausführen";
            // 
            // runProgramMenuItem
            // 
            this.runProgramMenuItem.Name = "runProgramMenuItem";
            this.runProgramMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runProgramMenuItem.Size = new System.Drawing.Size(706, 34);
            this.runProgramMenuItem.Text = "Programm starten";
            this.runProgramMenuItem.Click += new System.EventHandler(this.runProgramMenuItem_Click);
            // 
            // openOutputFormMenuItem
            // 
            this.openOutputFormMenuItem.Name = "openOutputFormMenuItem";
            this.openOutputFormMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.openOutputFormMenuItem.Size = new System.Drawing.Size(706, 34);
            this.openOutputFormMenuItem.Text = "Ausgabefenster öffnen";
            this.openOutputFormMenuItem.Click += new System.EventHandler(this.openOutputFormMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(703, 6);
            // 
            // singleEqualIsCompareOperatorMenuItem
            // 
            this.singleEqualIsCompareOperatorMenuItem.CheckOnClick = true;
            this.singleEqualIsCompareOperatorMenuItem.Name = "singleEqualIsCompareOperatorMenuItem";
            this.singleEqualIsCompareOperatorMenuItem.ShortcutKeyDisplayString = "";
            this.singleEqualIsCompareOperatorMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D0)));
            this.singleEqualIsCompareOperatorMenuItem.Size = new System.Drawing.Size(706, 34);
            this.singleEqualIsCompareOperatorMenuItem.Text = "Einfaches \'=\' wird als Vergleichsoperator verwendet";
            this.singleEqualIsCompareOperatorMenuItem.Click += new System.EventHandler(this.singleEqualIsCompareOperatorMenuItem_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHelpMenuItem,
            this.toolStripSeparator3,
            this.updatePseudocodeIDEMenuItem});
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(64, 29);
            this.helpMenuItem.Text = "Hilfe";
            // 
            // showHelpMenuItem
            // 
            this.showHelpMenuItem.Name = "showHelpMenuItem";
            this.showHelpMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.showHelpMenuItem.Size = new System.Drawing.Size(346, 34);
            this.showHelpMenuItem.Text = "Hilfe anzeigen";
            this.showHelpMenuItem.Click += new System.EventHandler(this.showHelpMenuItem_Click);
            // 
            // codeTextBox
            // 
            this.codeTextBox.AcceptsTab = true;
            this.codeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.codeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeTextBox.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.codeTextBox.HideSelection = false;
            this.codeTextBox.Location = new System.Drawing.Point(0, 33);
            this.codeTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(1176, 676);
            this.codeTextBox.TabIndex = 1;
            this.codeTextBox.Text = "";
            this.codeTextBox.WordWrap = false;
            this.codeTextBox.SelectionChanged += new System.EventHandler(this.codeTextBox_SelectionChanged);
            this.codeTextBox.TextChanged += new System.EventHandler(this.codeTextBox_TextChanged);
            this.codeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.codeTextBox_KeyDown);
            this.codeTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.codeTextBox_KeyPress);
            // 
            // updatePseudocodeIDEMenuItem
            // 
            this.updatePseudocodeIDEMenuItem.Name = "updatePseudocodeIDEMenuItem";
            this.updatePseudocodeIDEMenuItem.Size = new System.Drawing.Size(346, 34);
            this.updatePseudocodeIDEMenuItem.Text = "Pseudocode IDE aktualisieren";
            this.updatePseudocodeIDEMenuItem.Click += new System.EventHandler(this.updatePseudocodeIDEMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(343, 6);
            // 
            // PseudocodeIDEForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 709);
            this.Controls.Add(this.codeTextBox);
            this.Controls.Add(this.menuStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(511, 345);
            this.Name = "PseudocodeIDEForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pseudocode IDE - Neue Datei";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PseudocodeIDE_FormClosing);
            this.Load += new System.EventHandler(this.PseudocodeIDEForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHelpMenuItem;
        private System.Windows.Forms.RichTextBox codeTextBox;
        private System.Windows.Forms.ToolStripMenuItem runMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runProgramMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordWrapMenuItem;
        private System.Windows.Forms.ToolStripMenuItem singleEqualIsCompareOperatorMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openOutputFormMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem updatePseudocodeIDEMenuItem;
    }
}

