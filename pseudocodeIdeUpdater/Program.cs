using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pseudocodeIdeUpdater
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // allow automatic scaling
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                string path = args[0];
                bool firstRun = bool.Parse(args[1]);

                if (!firstRun)
                {
                    AutoUpdater.CheckForUpdateEvent += AutoUpdater_CheckForUpdateEvent;
                }

                AutoUpdater.InstallationPath = path;
                AutoUpdater.RunUpdateAsAdmin = false;
                AutoUpdater.ClearAppDirectory = true;
                AutoUpdater.AppTitle = "Pseudocode IDE";
                AutoUpdater.ShowSkipButton = false;
                AutoUpdater.Mandatory = !firstRun;
                AutoUpdater.Synchronous = true;
                AutoUpdater.ApplicationExitEvent += AutoUpdater_ApplicationExitEvent;
                AutoUpdater.ExecutablePath = "pseudocode-ide.exe";

                AutoUpdater.Start("https://raw.githubusercontent.com/PocketMiner82/pseudocode-ide/main/AutoUpdater.xml");

            }
            catch
            {
                MessageBox.Show("Bitte führe pseudecode-ide.exe aus, nicht dieses Tool.", "Fehler");
            }
            
        }

        private static void AutoUpdater_ApplicationExitEvent()
        {
            foreach (var process in Process.GetProcessesByName("pseudocode-ide"))
            {
                process.Kill();
            }
        }

        private static void AutoUpdater_CheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.IsUpdateAvailable)
            {
                AutoUpdater.ShowUpdateForm(args);
            }
            else
            {
                MessageBox.Show("Es sind keine Aktualisierungen verfügbar.", "Aktualisierungen");
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
