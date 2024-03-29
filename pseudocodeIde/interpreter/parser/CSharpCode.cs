﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using pseudocodeIde.interpreter.logging;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading;

namespace pseudocodeIde.interpreter.parser
{
    public class CSharpCode
    {
        private const string TEMPLATE_CLASS =
@"using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace codeOutput {
    public class Liste<T> : List<T> {
        public void _anhaengen(T value) {
            Add(value);
        }

        public T _gib(int index) {
            return this[index];
        }

        public void _ersetzen(int index, T value) {
            this[index] = value;
        }
    }

    public class BaseCodeOutput {
        private Action<string, bool> printMethod;

        public BaseCodeOutput(Action<string, bool> printMethod) {
            this.printMethod = printMethod;
        }

        protected virtual void _schreibe(object msg, bool newLine = true) {
            this.printMethod(msg == null ? ""NICHTS"" : msg.ToString(), newLine);
        }

        protected virtual void _warte(int millis) {
            Thread.Sleep(millis);
        }

        protected virtual T? _benutzereingabe<T>(string text, string title) {
            Form prompt = new Form() {
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
            Button confirmation = new Button() { Text = ""OK"", Location = new System.Drawing.Point(373, 101), Size = new System.Drawing.Size(93, 31), DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            try {
                if (prompt.ShowDialog() == DialogResult.OK && double.TryParse(textBox.Text, out double result)) {
                    return (T?)Convert.ChangeType(result, Nullable.GetUnderlyingType(typeof(T)));
                } else {
                    return (T?)Convert.ChangeType(textBox.Text, Nullable.GetUnderlyingType(typeof(T)));
                }
            } catch {
                return default(T?);
            }
        }
    }

// ---------------------------------------------------------
// ---- AB HIER BEGINNT DER AUTOMATISCH GENERIERTE CODE ----
// ---------------------------------------------------------

    public class CodeOutput : BaseCodeOutput {
%FIELDS%

        public CodeOutput(Action<string, bool> printMethod) : base(printMethod) {
%CONSTRUCTOR%
        }

%METHODS%
    }
}";

        public string fields { get; set; } = "";

        public string constructor { get; set; } = "";

        public string methods { get; set; } = "";

        public string codeText { get; private set; } = "";

        private Assembly compiledAssembly;


        public void compile()
        {
            CodeDomProvider provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();

            CompilerParameters parameters = new CompilerParameters();

            // add default assemblies
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
            parameters.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
            parameters.ReferencedAssemblies.Add("System.Deployment.dll");
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            parameters.ReferencedAssemblies.Add("System.Net.Http.dll");
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");

            parameters.CompilerOptions += "/unsafe /optimize /langversion:9.0";
            

            this.codeText = TEMPLATE_CLASS
                .Replace("%FIELDS%", this.fields)
                .Replace("%CONSTRUCTOR%", this.constructor)
                .Replace("%METHODS%", this.methods);

            // pretty print code
            SyntaxNode node = CSharpSyntaxTree.ParseText(this.codeText).GetRoot();
            this.codeText = node.NormalizeWhitespace().ToFullString();

            Logger.info(LogMessage.GENERATED_C_SHARP_CODE);

            string printCode = "1\t";
            int line = 1;
            foreach(char c in this.codeText)
            {
                if (c == '\n')
                {
                    printCode += "\n" + ++line + "\t";
                    continue;
                }

                printCode += c;
            }

            Logger.print($"\n{printCode}\n");

            Logger.info(LogMessage.COMPILING_C_SHARP_CODE);

            CompilerResults result = provider.CompileAssemblyFromSource(parameters, this.codeText);

            if (result.Errors.Count > 0)
            {
                Logger.error(LogMessage.COMPILE_ERRORS);

                foreach(CompilerError error in result.Errors)
                {
                    Logger.error($"\t{error}");
                }

                return;
            }

            this.compiledAssembly = result.CompiledAssembly;
        }

        public void execute()
        {
            try
            {
                if (this.compiledAssembly != null)
                {
                    Type type = this.compiledAssembly.GetType("codeOutput.CodeOutput");

                    Activator.CreateInstance(type, new Action<string, bool>(Logger.print));
                }
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception e)
            {
                Logger.error(e.ToString());
                Interpreter.hadRuntimeError = true;
            }

            Interpreter.onStop();
        }
    }
}
