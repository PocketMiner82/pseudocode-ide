using System;
using System.IO;
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

            string myAppPath = System.Reflection.Assembly.GetEntryAssembly().Location;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PseudocodeIDEForm());
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
