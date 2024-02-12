namespace pseudocodeIde
{
    partial class OutputForm
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.startMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyCSharpCodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startMenuItem,
            this.stopMenuItem,
            this.copyCSharpCodeMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip.ShowItemToolTips = true;
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // startMenuItem
            // 
            this.startMenuItem.AutoToolTip = true;
            this.startMenuItem.ForeColor = System.Drawing.Color.Green;
            this.startMenuItem.Name = "startMenuItem";
            this.startMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.startMenuItem.Size = new System.Drawing.Size(43, 22);
            this.startMenuItem.Text = "Start";
            this.startMenuItem.ToolTipText = "Start (F5)";
            this.startMenuItem.Click += new System.EventHandler(this.startMenuItem_Click);
            // 
            // stopMenuItem
            // 
            this.stopMenuItem.AutoToolTip = true;
            this.stopMenuItem.Enabled = false;
            this.stopMenuItem.ForeColor = System.Drawing.Color.Red;
            this.stopMenuItem.Name = "stopMenuItem";
            this.stopMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.stopMenuItem.Size = new System.Drawing.Size(43, 22);
            this.stopMenuItem.Text = "Stop";
            this.stopMenuItem.ToolTipText = "Stop (Shift+F5)";
            this.stopMenuItem.Click += new System.EventHandler(this.stopMenuItem_Click);
            // 
            // copyCSharpCodeMenuItem
            // 
            this.copyCSharpCodeMenuItem.Name = "copyCSharpCodeMenuItem";
            this.copyCSharpCodeMenuItem.Size = new System.Drawing.Size(114, 22);
            this.copyCSharpCodeMenuItem.Text = "C# Code kopieren";
            this.copyCSharpCodeMenuItem.Visible = false;
            this.copyCSharpCodeMenuItem.Click += new System.EventHandler(this.copyCSharpCodeMenuItem_Click);
            // 
            // rtbOutput
            // 
            this.rtbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbOutput.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbOutput.Location = new System.Drawing.Point(0, 24);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ReadOnly = true;
            this.rtbOutput.Size = new System.Drawing.Size(784, 437);
            this.rtbOutput.TabIndex = 1;
            this.rtbOutput.Text = "";
            this.rtbOutput.WordWrap = false;
            // 
            // OutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.rtbOutput);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(348, 244);
            this.Name = "OutputForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ausgabe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OutputForm_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem startMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopMenuItem;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.ToolStripMenuItem copyCSharpCodeMenuItem;
    }
}