namespace pseudocodeIde
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
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
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
            this.tabPageReplace.Location = new System.Drawing.Point(4, 29);
            this.tabPageReplace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageReplace.Name = "tabPageReplace";
            this.tabPageReplace.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageReplace.Size = new System.Drawing.Size(718, 146);
            this.tabPageReplace.TabIndex = 1;
            this.tabPageReplace.Text = "Replace";
            this.tabPageReplace.UseVisualStyleBackColor = true;
            // 
            // cbMatchCase2
            // 
            this.cbMatchCase2.AutoSize = true;
            this.cbMatchCase2.Location = new System.Drawing.Point(9, 112);
            this.cbMatchCase2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbMatchCase2.Name = "cbMatchCase2";
            this.cbMatchCase2.Size = new System.Drawing.Size(117, 24);
            this.cbMatchCase2.TabIndex = 3;
            this.cbMatchCase2.Text = "Match case";
            this.cbMatchCase2.UseVisualStyleBackColor = true;
            // 
            // btReplaceAll
            // 
            this.btReplaceAll.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btReplaceAll.Location = new System.Drawing.Point(510, 100);
            this.btReplaceAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btReplaceAll.Name = "btReplaceAll";
            this.btReplaceAll.Size = new System.Drawing.Size(195, 35);
            this.btReplaceAll.TabIndex = 6;
            this.btReplaceAll.Text = "Replace All";
            this.btReplaceAll.UseVisualStyleBackColor = true;
            // 
            // btReplace
            // 
            this.btReplace.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btReplace.Location = new System.Drawing.Point(510, 55);
            this.btReplace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btReplace.Name = "btReplace";
            this.btReplace.Size = new System.Drawing.Size(195, 35);
            this.btReplace.TabIndex = 5;
            this.btReplace.Text = "Replace";
            this.btReplace.UseVisualStyleBackColor = true;
            // 
            // btFindNext2
            // 
            this.btFindNext2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btFindNext2.Location = new System.Drawing.Point(510, 10);
            this.btFindNext2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btFindNext2.Name = "btFindNext2";
            this.btFindNext2.Size = new System.Drawing.Size(195, 35);
            this.btFindNext2.TabIndex = 4;
            this.btFindNext2.Text = "Find next";
            this.btFindNext2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 62);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Replace with:";
            // 
            // tbReplaceWith
            // 
            this.tbReplaceWith.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReplaceWith.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbReplaceWith.Location = new System.Drawing.Point(120, 60);
            this.tbReplaceWith.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbReplaceWith.Name = "tbReplaceWith";
            this.tbReplaceWith.Size = new System.Drawing.Size(377, 26);
            this.tbReplaceWith.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Find what:";
            // 
            // tbFindWhat2
            // 
            this.tbFindWhat2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFindWhat2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFindWhat2.Location = new System.Drawing.Point(120, 15);
            this.tbFindWhat2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbFindWhat2.Name = "tbFindWhat2";
            this.tbFindWhat2.Size = new System.Drawing.Size(377, 26);
            this.tbFindWhat2.TabIndex = 1;
            // 
            // tabPageFind
            // 
            this.tabPageFind.Controls.Add(this.btCount);
            this.tabPageFind.Controls.Add(this.cbMatchCase1);
            this.tabPageFind.Controls.Add(this.btFindNext1);
            this.tabPageFind.Controls.Add(this.label3);
            this.tabPageFind.Controls.Add(this.tbFindWhat1);
            this.tabPageFind.Location = new System.Drawing.Point(4, 29);
            this.tabPageFind.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageFind.Name = "tabPageFind";
            this.tabPageFind.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageFind.Size = new System.Drawing.Size(718, 146);
            this.tabPageFind.TabIndex = 0;
            this.tabPageFind.Text = "Find";
            this.tabPageFind.UseVisualStyleBackColor = true;
            // 
            // btCount
            // 
            this.btCount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btCount.Location = new System.Drawing.Point(510, 55);
            this.btCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btCount.Name = "btCount";
            this.btCount.Size = new System.Drawing.Size(195, 35);
            this.btCount.TabIndex = 4;
            this.btCount.Text = "Count";
            this.btCount.UseVisualStyleBackColor = true;
            // 
            // cbMatchCase1
            // 
            this.cbMatchCase1.AutoSize = true;
            this.cbMatchCase1.Location = new System.Drawing.Point(9, 112);
            this.cbMatchCase1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbMatchCase1.Name = "cbMatchCase1";
            this.cbMatchCase1.Size = new System.Drawing.Size(117, 24);
            this.cbMatchCase1.TabIndex = 2;
            this.cbMatchCase1.Text = "Match case";
            this.cbMatchCase1.UseVisualStyleBackColor = true;
            // 
            // btFindNext1
            // 
            this.btFindNext1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btFindNext1.Location = new System.Drawing.Point(510, 10);
            this.btFindNext1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btFindNext1.Name = "btFindNext1";
            this.btFindNext1.Size = new System.Drawing.Size(195, 35);
            this.btFindNext1.TabIndex = 3;
            this.btFindNext1.Text = "Find next";
            this.btFindNext1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 17);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Find what:";
            // 
            // tbFindWhat1
            // 
            this.tbFindWhat1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFindWhat1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFindWhat1.Location = new System.Drawing.Point(120, 15);
            this.tbFindWhat1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbFindWhat1.Name = "tbFindWhat1";
            this.tbFindWhat1.Size = new System.Drawing.Size(377, 26);
            this.tbFindWhat1.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageFind);
            this.tabControl.Controls.Add(this.tabPageReplace);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(726, 179);
            this.tabControl.TabIndex = 50;
            this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Selecting);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 179);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip.Size = new System.Drawing.Size(726, 32);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(17, 25);
            this.statusLabel.Text = " ";
            // 
            // FindReplaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 211);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(149999990, 267);
            this.MinimumSize = new System.Drawing.Size(739, 267);
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