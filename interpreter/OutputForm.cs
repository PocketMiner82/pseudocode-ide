using pseudocode_ide.interpreter;
using pseudocode_ide.interpreter.log;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pseudocode_ide
{
    public partial class OutputForm : Form
    {
        private Task runTask = Task.CompletedTask;

        public static CancellationTokenSource runTaskCancelTokenSource { get; set; }

        public static CancellationToken runTaskCancelToken { get; set; }

        public PseudocodeIDEForm mainForm
        {
            get
            {
                return (PseudocodeIDEForm)Owner;
            }
        }

        public string outputText
        {
            get
            {
                return (string)Invoke((Func<string>) delegate
                {
                    return rtbOutput.Text;
                });
            }
            set
            {
                Invoke(new Action(() =>
                {
                    rtbOutput.Text = value;
                }));
            }
        }

        private Interpreter interpreter;


        public OutputForm(PseudocodeIDEForm mainForm)
        {
            Owner = mainForm;
            this.interpreter = new Interpreter(this);
            Logger.outputForm = this;

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
        }

        // ---------------------------------------------
        // METHODS
        // ---------------------------------------------

        public void ShowAndRun()
        {
            this.Show();

            this.startMenuItem_Click(null, null);
        }

        private void startMenuItem_Click(object sender, EventArgs e)
        {
            startMenuItem.Enabled = false;
            stopMenuItem.Enabled = true;

            runTaskCancelTokenSource = new CancellationTokenSource();
            runTaskCancelToken = runTaskCancelTokenSource.Token;
            this.runTask = Task.Run(() =>this.interpreter.run());
        }

        public void stopMenuItem_Click(object sender, EventArgs e)
        {
            if (Interpreter.hadError)
            {
                // TODO
            }

            Invoke(new Action(() =>
            {
                startMenuItem.Enabled = true;
                stopMenuItem.Enabled = false;
            }));

            if (!this.runTask.IsCompleted)
            {
                runTaskCancelTokenSource.Cancel();
            }
        }

        public void scrollRtbOutputToEnd()
        {
            Invoke(new Action(() =>
            {
                rtbOutput.SelectionStart = rtbOutput.Text.Length;
                rtbOutput.ScrollToCaret();
            }));
        }
    }
}
