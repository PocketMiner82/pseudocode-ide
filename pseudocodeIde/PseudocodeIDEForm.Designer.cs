﻿namespace pseudocodeIde
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PseudocodeIDEForm));
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
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.goToMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordWrapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runProgramMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openOutputFormMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.singleEqualIsCompareOperatorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.updateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateBetaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeTextBox = new ScintillaNET.Scintilla();
            this.autoCompleteMenu = new AutocompleteMenuNS.AutocompleteMenu();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.viewMenuItem,
            this.runMenuItem,
            this.helpMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
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
            this.fileMenuItem.Size = new System.Drawing.Size(46, 22);
            this.fileMenuItem.Text = "Datei";
            // 
            // newMenuItem
            // 
            this.newMenuItem.Name = "newMenuItem";
            this.newMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newMenuItem.Size = new System.Drawing.Size(166, 22);
            this.newMenuItem.Text = "Neu";
            this.newMenuItem.Click += new System.EventHandler(this.NewMenuItem_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMenuItem.Size = new System.Drawing.Size(166, 22);
            this.openMenuItem.Text = "Öffnen";
            this.openMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Enabled = false;
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveMenuItem.Size = new System.Drawing.Size(166, 22);
            this.saveMenuItem.Text = "Speichern";
            this.saveMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator1,
            this.findMenuItem,
            this.replaceMenuItem,
            this.toolStripSeparator4,
            this.goToMenuItem});
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(75, 22);
            this.editMenuItem.Text = "Bearbeiten";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.undoToolStripMenuItem.Text = "Rückgängig";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.UndoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.redoToolStripMenuItem.Text = "Wiederherstellen";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.RedoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(200, 6);
            // 
            // findMenuItem
            // 
            this.findMenuItem.Name = "findMenuItem";
            this.findMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findMenuItem.Size = new System.Drawing.Size(203, 22);
            this.findMenuItem.Text = "Suchen";
            this.findMenuItem.Click += new System.EventHandler(this.FindMenuItem_Click);
            // 
            // replaceMenuItem
            // 
            this.replaceMenuItem.Name = "replaceMenuItem";
            this.replaceMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceMenuItem.Size = new System.Drawing.Size(203, 22);
            this.replaceMenuItem.Text = "Ersetzen";
            this.replaceMenuItem.Click += new System.EventHandler(this.ReplaceMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(200, 6);
            // 
            // goToMenuItem
            // 
            this.goToMenuItem.Name = "goToMenuItem";
            this.goToMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.goToMenuItem.Size = new System.Drawing.Size(203, 22);
            this.goToMenuItem.Text = "Gehe zu";
            this.goToMenuItem.Click += new System.EventHandler(this.GoToMenuItem_Click);
            // 
            // viewMenuItem
            // 
            this.viewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wordWrapMenuItem});
            this.viewMenuItem.Name = "viewMenuItem";
            this.viewMenuItem.Size = new System.Drawing.Size(59, 22);
            this.viewMenuItem.Text = "Ansicht";
            // 
            // wordWrapMenuItem
            // 
            this.wordWrapMenuItem.CheckOnClick = true;
            this.wordWrapMenuItem.Name = "wordWrapMenuItem";
            this.wordWrapMenuItem.Size = new System.Drawing.Size(215, 22);
            this.wordWrapMenuItem.Text = "Zeilenumbrüche aktivieren";
            this.wordWrapMenuItem.Click += new System.EventHandler(this.WordWrapMenuItem_Click);
            // 
            // runMenuItem
            // 
            this.runMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runProgramMenuItem,
            this.openOutputFormMenuItem,
            this.toolStripSeparator2,
            this.singleEqualIsCompareOperatorMenuItem});
            this.runMenuItem.Name = "runMenuItem";
            this.runMenuItem.Size = new System.Drawing.Size(74, 22);
            this.runMenuItem.Text = "Ausführen";
            // 
            // runProgramMenuItem
            // 
            this.runProgramMenuItem.Name = "runProgramMenuItem";
            this.runProgramMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runProgramMenuItem.Size = new System.Drawing.Size(415, 22);
            this.runProgramMenuItem.Text = "Programm starten";
            this.runProgramMenuItem.Click += new System.EventHandler(this.RunProgramMenuItem_Click);
            // 
            // openOutputFormMenuItem
            // 
            this.openOutputFormMenuItem.Name = "openOutputFormMenuItem";
            this.openOutputFormMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.openOutputFormMenuItem.Size = new System.Drawing.Size(415, 22);
            this.openOutputFormMenuItem.Text = "Ausgabefenster öffnen";
            this.openOutputFormMenuItem.Click += new System.EventHandler(this.OpenOutputFormMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(412, 6);
            // 
            // singleEqualIsCompareOperatorMenuItem
            // 
            this.singleEqualIsCompareOperatorMenuItem.CheckOnClick = true;
            this.singleEqualIsCompareOperatorMenuItem.Name = "singleEqualIsCompareOperatorMenuItem";
            this.singleEqualIsCompareOperatorMenuItem.ShortcutKeyDisplayString = "";
            this.singleEqualIsCompareOperatorMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D0)));
            this.singleEqualIsCompareOperatorMenuItem.Size = new System.Drawing.Size(415, 22);
            this.singleEqualIsCompareOperatorMenuItem.Text = "Einfaches \'=\' wird als Vergleichsoperator verwendet";
            this.singleEqualIsCompareOperatorMenuItem.Click += new System.EventHandler(this.SingleEqualIsCompareOperatorMenuItem_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHelpMenuItem,
            this.toolStripSeparator3,
            this.updateMenuItem,
            this.updateBetaMenuItem});
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(44, 22);
            this.helpMenuItem.Text = "Hilfe";
            // 
            // showHelpMenuItem
            // 
            this.showHelpMenuItem.Name = "showHelpMenuItem";
            this.showHelpMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.showHelpMenuItem.Size = new System.Drawing.Size(274, 22);
            this.showHelpMenuItem.Text = "Hilfe anzeigen";
            this.showHelpMenuItem.Click += new System.EventHandler(this.ShowHelpMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(271, 6);
            // 
            // updateMenuItem
            // 
            this.updateMenuItem.Name = "updateMenuItem";
            this.updateMenuItem.Size = new System.Drawing.Size(274, 22);
            this.updateMenuItem.Text = "Pseudocode IDE aktualisieren";
            this.updateMenuItem.Click += new System.EventHandler(this.UpdateMenuItem_Click);
            // 
            // updateBetaMenuItem
            // 
            this.updateBetaMenuItem.Name = "updateBetaMenuItem";
            this.updateBetaMenuItem.Size = new System.Drawing.Size(274, 22);
            this.updateBetaMenuItem.Text = "Pseudocode IDE auf Beta aktualisieren";
            this.updateBetaMenuItem.Click += new System.EventHandler(this.UpdateBetaMenuItem_Click);
            // 
            // codeTextBox
            // 
            this.codeTextBox.AdditionalSelectionTyping = true;
            this.codeTextBox.AutoCMaxHeight = 9;
            this.codeTextBox.BiDirectionality = ScintillaNET.BiDirectionalDisplayType.Disabled;
            this.codeTextBox.CaretLineBackColor = System.Drawing.Color.WhiteSmoke;
            this.codeTextBox.CaretLineVisible = true;
            this.codeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeTextBox.EolMode = ScintillaNET.Eol.Lf;
            this.codeTextBox.IndentationGuides = ScintillaNET.IndentView.Real;
            this.codeTextBox.IndentWidth = 4;
            this.codeTextBox.LexerName = null;
            this.codeTextBox.Location = new System.Drawing.Point(0, 24);
            this.codeTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.codeTextBox.MultiPaste = ScintillaNET.MultiPaste.Each;
            this.codeTextBox.MultipleSelection = true;
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.ScrollWidth = 39;
            this.codeTextBox.Size = new System.Drawing.Size(784, 437);
            this.codeTextBox.TabIndents = true;
            this.codeTextBox.TabIndex = 1;
            this.codeTextBox.UseRightToLeftReadingLayout = false;
            this.codeTextBox.UseTabs = true;
            this.codeTextBox.WrapMode = ScintillaNET.WrapMode.None;
            this.codeTextBox.Zoom = 2;
            this.codeTextBox.CharAdded += new System.EventHandler<ScintillaNET.CharAddedEventArgs>(this.CodeTextBox_CharAdded);
            this.codeTextBox.UpdateUI += new System.EventHandler<ScintillaNET.UpdateUIEventArgs>(this.CodeTextBox_UpdateUI);
            this.codeTextBox.TextChanged += new System.EventHandler(this.CodeTextBox_TextChanged);
            this.codeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CodeTextBox_KeyDown);
            this.codeTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CodeTextBox_KeyPress);
            // 
            // autoCompleteMenu
            // 
            this.autoCompleteMenu.AllowsTabKey = true;
            this.autoCompleteMenu.AppearInterval = 50;
            this.autoCompleteMenu.Colors = ((AutocompleteMenuNS.Colors)(resources.GetObject("autoCompleteMenu.Colors")));
            this.autoCompleteMenu.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoCompleteMenu.ImageList = null;
            this.autoCompleteMenu.Items = new string[0];
            this.autoCompleteMenu.MaximumSize = new System.Drawing.Size(250, 200);
            this.autoCompleteMenu.MinFragmentLength = 1;
            this.autoCompleteMenu.TargetControlWrapper = null;
            this.autoCompleteMenu.Selected += new System.EventHandler<AutocompleteMenuNS.SelectedEventArgs>(this.AutoCompleteMenu_Selected);
            this.autoCompleteMenu.Hovered += new System.EventHandler<AutocompleteMenuNS.HoveredEventArgs>(this.AutoCompleteMenu_Hovered);
            // 
            // PseudocodeIDEForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.codeTextBox);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(346, 238);
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
        private System.Windows.Forms.ToolStripMenuItem updateMenuItem;
        private ScintillaNET.Scintilla codeTextBox;
        private System.Windows.Forms.ToolStripMenuItem goToMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private AutocompleteMenuNS.AutocompleteMenu autoCompleteMenu;
        private System.Windows.Forms.ToolStripMenuItem updateBetaMenuItem;
    }
}

