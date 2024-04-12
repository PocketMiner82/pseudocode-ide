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

using AutoUpdaterDotNET;
using System;
using System.Diagnostics;
using System.Reflection;
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
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                string path = args[0];
                bool firstRun = bool.Parse(args[1]);
                bool betaButton = bool.Parse(args[2]);

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

                Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

                bool beta = betaButton;
                if (assemblyVersion.Revision > 0 && !betaButton && !firstRun)
                {
                    // hack to allow to go back to stable release, as the last version tag (pre release count) will be missing
                    // without this hack, the AutoUpdater would think that the new release is a lower version than this
                    AutoUpdater.InstalledVersion = new Version($"0.0.0");
                }
                else if (assemblyVersion.Revision > 0)
                {
                    // beta release
                    AutoUpdater.InstalledVersion = new Version($"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}.{assemblyVersion.Revision}");
                    beta = true;
                }
                else
                {
                    // stable release
                    AutoUpdater.InstalledVersion = new Version($"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}");
                }

                AutoUpdater.Start($"https://raw.githubusercontent.com/PocketMiner82/pseudocode-ide/{(beta ? "dev" : "main")}/AutoUpdater.xml");

            }
            catch
            {
                MessageBox.Show("Bitte führe pseudecode-ide.exe aus, nicht dieses Tool.", "Fehler");
            }
        }

        private static void AutoUpdater_ApplicationExitEvent()
        {
            foreach (Process process in Process.GetProcessesByName("pseudocode-ide"))
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
