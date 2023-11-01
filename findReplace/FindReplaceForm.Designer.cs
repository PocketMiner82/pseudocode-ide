namespace pseudocode_ide
{
    partial class FindReplaceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabPageReplace = new System.Windows.Forms.TabPage();
            this.cbMatchCase2 = new System.Windows.Forms.CheckBox();
            this.btReplaceAll = new System.Windows.Forms.Button();
            this.btReplace = new System.Windows.Forms.Button();
            this.btFindNext2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbReplaceWith = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbFindWhat2 = new System.Windows.Forms.TextBox();
            this.tabPageFind = new System.Windows.Forms.TabPage();
            this.btCount = new System.Windows.Forms.Button();
            this.cbMatchCase1 = new System.Windows.Forms.CheckBox();
            this.btFindNext1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFindWhat1 = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPageReplace.SuspendLayout();
            this.tabPageFind.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // tabPageReplace
            // 
            this.tabPageReplace.Controls.Add(this.cbMatchCase2);
            this.tabPageReplace.Controls.Add(this.btReplaceAll);
            this.tabPageReplace.Controls.Add(this.btReplace);
            this.tabPageReplace.Controls.Add(this.btFindNext2);
            this.tabPageReplace.Controls.Add(this.label1);
            this.tabPageReplace.Controls.Add(this.tbReplaceWith);
            this.tabPageReplace.Controls.Add(this.label2);
            this.tabPageReplace.Controls.Add(this.tbFindWhat2);
            this.tabPageReplace.Location = new System.Drawing.Point(4, 22);
            this.tabPageReplace.Name = "tabPageReplace";
            this.tabPageReplace.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReplace.Size = new System.Drawing.Size(476, 93);
            this.tabPageReplace.TabIndex = 1;
            this.tabPageReplace.Text = "Replace";
            this.tabPageReplace.UseVisualStyleBackColor = true;
            // 
            // cbMatchCase2
            // 
            this.cbMatchCase2.AutoSize = true;
            this.cbMatchCase2.Location = new System.Drawing.Point(7, 70);
            this.cbMatchCase2.Name = "cbMatchCase2";
            this.cbMatchCase2.Size = new System.Drawing.Size(82, 17);
            this.cbMatchCase2.TabIndex = 3;
            this.cbMatchCase2.Text = "Match case";
            this.cbMatchCase2.UseVisualStyleBackColor = true;
            // 
            // btReplaceAll
            // 
            this.btReplaceAll.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btReplaceAll.Location = new System.Drawing.Point(340, 64);
            this.btReplaceAll.Name = "btReplaceAll";
            this.btReplaceAll.Size = new System.Drawing.Size(130, 23);
            this.btReplaceAll.TabIndex = 6;
            this.btReplaceAll.Text = "Replace All";
            this.btReplaceAll.UseVisualStyleBackColor = true;
            // 
            // btReplace
            // 
            this.btReplace.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btReplace.Location = new System.Drawing.Point(340, 35);
            this.btReplace.Name = "btReplace";
            this.btReplace.Size = new System.Drawing.Size(130, 23);
            this.btReplace.TabIndex = 5;
            this.btReplace.Text = "Replace";
            this.btReplace.UseVisualStyleBackColor = true;
            // 
            // btFindNext2
            // 
            this.btFindNext2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btFindNext2.Location = new System.Drawing.Point(340, 6);
            this.btFindNext2.Name = "btFindNext2";
            this.btFindNext2.Size = new System.Drawing.Size(130, 23);
            this.btFindNext2.TabIndex = 4;
            this.btFindNext2.Text = "Find next";
            this.btFindNext2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Replace with:";
            // 
            // tbReplaceWith
            // 
            this.tbReplaceWith.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReplaceWith.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbReplaceWith.Location = new System.Drawing.Point(82, 36);
            this.tbReplaceWith.Name = "tbReplaceWith";
            this.tbReplaceWith.Size = new System.Drawing.Size(250, 20);
            this.tbReplaceWith.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Find what:";
            // 
            // tbFindWhat2
            // 
            this.tbFindWhat2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFindWhat2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFindWhat2.Location = new System.Drawing.Point(82, 7);
            this.tbFindWhat2.Name = "tbFindWhat2";
            this.tbFindWhat2.Size = new System.Drawing.Size(250, 20);
            this.tbFindWhat2.TabIndex = 1;
            // 
            // tabPageFind
            // 
            this.tabPageFind.Controls.Add(this.btCount);
            this.tabPageFind.Controls.Add(this.cbMatchCase1);
            this.tabPageFind.Controls.Add(this.btFindNext1);
            this.tabPageFind.Controls.Add(this.label3);
            this.tabPageFind.Controls.Add(this.tbFindWhat1);
            this.tabPageFind.Location = new System.Drawing.Point(4, 22);
            this.tabPageFind.Name = "tabPageFind";
            this.tabPageFind.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFind.Size = new System.Drawing.Size(476, 93);
            this.tabPageFind.TabIndex = 0;
            this.tabPageFind.Text = "Find";
            this.tabPageFind.UseVisualStyleBackColor = true;
            // 
            // btCount
            // 
            this.btCount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btCount.Location = new System.Drawing.Point(340, 35);
            this.btCount.Name = "btCount";
            this.btCount.Size = new System.Drawing.Size(130, 23);
            this.btCount.TabIndex = 4;
            this.btCount.Text = "Count";
            this.btCount.UseVisualStyleBackColor = true;
            // 
            // cbMatchCase1
            // 
            this.cbMatchCase1.AutoSize = true;
            this.cbMatchCase1.Location = new System.Drawing.Point(7, 70);
            this.cbMatchCase1.Name = "cbMatchCase1";
            this.cbMatchCase1.Size = new System.Drawing.Size(82, 17);
            this.cbMatchCase1.TabIndex = 2;
            this.cbMatchCase1.Text = "Match case";
            this.cbMatchCase1.UseVisualStyleBackColor = true;
            // 
            // btFindNext1
            // 
            this.btFindNext1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btFindNext1.Location = new System.Drawing.Point(340, 6);
            this.btFindNext1.Name = "btFindNext1";
            this.btFindNext1.Size = new System.Drawing.Size(130, 23);
            this.btFindNext1.TabIndex = 3;
            this.btFindNext1.Text = "Find next";
            this.btFindNext1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Find what:";
            // 
            // tbFindWhat1
            // 
            this.tbFindWhat1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFindWhat1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFindWhat1.Location = new System.Drawing.Point(82, 7);
            this.tbFindWhat1.Name = "tbFindWhat1";
            this.tbFindWhat1.Size = new System.Drawing.Size(250, 20);
            this.tbFindWhat1.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageFind);
            this.tabControl.Controls.Add(this.tabPageReplace);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(484, 119);
            this.tabControl.TabIndex = 50;
            this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Selecting);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 119);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(484, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(10, 17);
            this.statusLabel.Text = " ";
            // 
            // FindReplaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 141);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(100000000, 180);
            this.MinimumSize = new System.Drawing.Size(500, 180);
            this.Name = "FindReplaceForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Find";
            this.Activated += new System.EventHandler(this.FindReplaceForm_Activated);
            this.Deactivate += new System.EventHandler(this.FindReplaceForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindReplaceForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindReplaceForm_KeyDown);
            this.tabPageReplace.ResumeLayout(false);
            this.tabPageReplace.PerformLayout();
            this.tabPageFind.ResumeLayout(false);
            this.tabPageFind.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TabPage tabPageReplace;
        private System.Windows.Forms.TabPage tabPageFind;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.CheckBox cbMatchCase2;
        private System.Windows.Forms.Button btReplaceAll;
        private System.Windows.Forms.Button btReplace;
        private System.Windows.Forms.Button btFindNext2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbReplaceWith;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFindWhat2;
        private System.Windows.Forms.Button btCount;
        private System.Windows.Forms.CheckBox cbMatchCase1;
        private System.Windows.Forms.Button btFindNext1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFindWhat1;
    }
}