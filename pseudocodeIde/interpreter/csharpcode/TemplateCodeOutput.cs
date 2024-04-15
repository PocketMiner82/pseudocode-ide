﻿// 
// THIS FILE IS NOT COMPILED AS C# CODE.
// IT IS AN EMBEDDED RESSOURCE.
//
// This is the template code that will be dynamically edited to contain the C# code that was
// generated from the user-written Pseudocode.
// 

#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace codeOutput
{
    public class Liste<T> : List<T>
    {
        public void _anhaengen(T value)
        {
            Add(value);
        }

        public T _gib(int index)
        {
            return this[index];
        }

        public void _ersetzen(int index, T value)
        {
            this[index] = value;
        }
    }

    public class BaseCodeOutput
    {
        private readonly Action<string, bool> _printMethod;

        public BaseCodeOutput(Action<string, bool> printMethod)
        {
            _printMethod = printMethod;
        }

        protected virtual void _schreibe(object msg, bool newLine = true)
        {
            _printMethod(msg == null ? "NICHTS" : msg.ToString(), newLine);
        }

        protected virtual void _warte(int millis)
        {
            Thread.Sleep(millis);
        }

        protected virtual T? _benutzereingabe<T>(string text, string title)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };
            Label textLabel = new Label() { Location = new System.Drawing.Point(12, 24), Size = new System.Drawing.Size(454, 42), Text = text };
            TextBox textBox = new TextBox() { Location = new System.Drawing.Point(12, 69), Size = new System.Drawing.Size(454, 42) };
            Button confirmation = new Button() { Text = "OK", Location = new System.Drawing.Point(373, 101), Size = new System.Drawing.Size(93, 31), DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => prompt.Close();
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            try
            {
                DialogResult result = prompt.ShowDialog();
                Type nullableType = Nullable.GetUnderlyingType(typeof(T));
                Type type = typeof(T);
                Type usedType = nullableType ?? type;

                if (result == DialogResult.OK)
                {
                    return (usedType == typeof(double) || usedType == typeof(int))
                        && double.TryParse(textBox.Text, out double parsed)
                    ? (T)Convert.ChangeType(parsed, usedType)
                    : (T)Convert.ChangeType(textBox.Text, usedType);
                }
            }
            catch { }

            return default;
        }
    }

    // -----------------------------------------
    // ---- AUTO-GENERATED CODE STARTS HERE ----
    // -----------------------------------------

    public class CodeOutput : BaseCodeOutput
    {
%FIELDS%

        public CodeOutput(Action<string, bool> printMethod) : base(printMethod)
        {
%CONSTRUCTOR%
        }

%METHODS%
    }
}