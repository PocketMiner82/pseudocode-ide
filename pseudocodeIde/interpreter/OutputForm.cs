// Pseudocode IDE - Execute Pseudocode for the German (BW) 2024 Abitur
// Copyright (C) 2024  PocketMiner82
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using pseudocode_ide;
using pseudocodeIde.interpreter;
using pseudocodeIde.interpreter.logging;
using System;
using System.Windows.Forms;

namespace pseudocodeIde
{
    public partial class OutputForm : Form
    {
        public PseudocodeIDEForm MainForm
        {
            get
            {
                return (PseudocodeIDEForm)Owner;
            }
        }

        public string OutputText
        {
            get
            {
                return (string)Invoke((Func<string>)delegate
                {
                    return rtbOutput.Text;
                });
            }
            set
            {
                Invoke(new Action(() =>
                {
                    rtbOutput.Text = value;
                    rtbOutput.SelectionStart = rtbOutput.Text.Length;
                    rtbOutput.ScrollToCaret();
                }));
            }
        }

        private readonly Interpreter _interpreter;


        public OutputForm(PseudocodeIDEForm mainForm)
        {
            Owner = mainForm;
            _interpreter = new Interpreter(this);
            Logger.OutputForm = this;

            InitializeComponent();
        }

        // ---------------------------------------------
        // EVENTS
        // ---------------------------------------------

        public new void Show()
        {
            Focus();
            base.Show();

            rtbOutput.Select();
        }

        private void OutputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Visible)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                _interpreter.Stop();
            }
        }

        // ---------------------------------------------
        // METHODS
        // ---------------------------------------------

        public void ShowAndRun()
        {
            Show();

            StartMenuItem_Click(null, null);
        }

        private void StartMenuItem_Click(object sender, EventArgs e)
        {
            copyCSharpCodeMenuItem.Visible = false;
            StopMenuItem_Click(null, null);

            startMenuItem.Enabled = false;
            stopMenuItem.Enabled = true;

            OutputText = "";
            _interpreter.Run();
        }

        public void StopMenuItem_Click(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                startMenuItem.Enabled = true;
                stopMenuItem.Enabled = false;
            }));

            _interpreter.Stop();

            Logger.Print("");
            Logger.Info(LogMessage.STOPPED_PROGRAM);
        }

        private void CopyCSharpCodeMenuItem_Click(object sender, EventArgs e)
        {
            new SetClipboardHelper(DataFormats.UnicodeText, _interpreter.CodeManager.CodeText).Go();
            MessageBox.Show("C# Code in Zwischenablage gespeichert.", "Zwischenablage");
        }

        public void ShowCopyButton()
        {
            Invoke(new Action(() => copyCSharpCodeMenuItem.Visible = true));
        }
    }
}
