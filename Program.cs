using System;
using System.Windows.Forms;

namespace pseudocodeIde
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // allow automatic scaling
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PseudocodeIDEForm());
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
