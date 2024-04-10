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

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace pseudocode_ide
{
    // from https://stackoverflow.com/questions/899350/how-do-i-copy-the-contents-of-a-string-to-the-clipboard-in-c

    public class SetClipboardHelper : StaHelper
    {
        readonly string FORMAT;
        readonly object DATA;

        public SetClipboardHelper(string format, object data)
        {
            FORMAT = format;
            DATA = data;
        }

        protected override void Work()
        {
            DataObject obj = new System.Windows.Forms.DataObject(
                FORMAT,
                DATA
            );

            Clipboard.SetDataObject(obj, true);
        }
    }

    public abstract class StaHelper
    {
        readonly ManualResetEvent COMPLETE = new ManualResetEvent(false);

        public void Go()
        {
            Thread thread = new Thread(new ThreadStart(DoWork))
            {
                IsBackground = true,
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        // Thread entry method
        private void DoWork()
        {
            try
            {
                COMPLETE.Reset();
                Work();
            }
            catch (Exception ex)
            {
                if (DontRetryWorkOnFailed)
                {
                    throw;
                }
                else
                {
                    try
                    {
                        Thread.Sleep(1000);
                        Work();
                    }
                    catch
                    {
                        // ex from first exception
                        Debug.WriteLine(ex);
                    }
                }
            }
            finally
            {
                COMPLETE.Set();
            }
        }

        public bool DontRetryWorkOnFailed { get; set; }

        // Implemented in base class to do actual work.
        protected abstract void Work();
    }
}
