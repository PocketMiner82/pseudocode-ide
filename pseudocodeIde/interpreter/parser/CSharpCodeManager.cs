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

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using pseudocodeIde.interpreter.logging;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading;

namespace pseudocodeIde.interpreter.parser
{
    public class CSharpCodeManager
    {
        /// <summary>
        /// Fields that will be put in the CodeOutput class
        /// </summary>
        public string Fields { get; set; } = "";

        /// <summary>
        /// The constructor that will be put in the CodeOutput class
        /// </summary>
        public string Constructor { get; set; } = "";

        /// <summary>
        /// The methods that will be put in the CodeOutput class
        /// </summary>
        public string Methods { get; set; } = "";

        /// <summary>
        /// The generated CodeOutput class
        /// </summary>
        public string CodeText { get; private set; } = "";

        private Assembly _compiledAssembly;


        /// <summary>
        /// This method fills out the code template class and tries to compile it.
        /// If it doesnt compile, the error messages will be printed to the output console.
        /// </summary>
        public void Compile()
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

            // use different lang version
            parameters.CompilerOptions += "/unsafe /optimize /langversion:9.0";


            // read the code text from the embedded ressource (TemplateCodeOutput.cs)
            // and fill in the fileds, constructor and methods
            CodeText = Assembly.GetExecutingAssembly().ReadResource("TemplateCodeOutput.cs")
                .Replace("%FIELDS%", Fields)
                .Replace("%CONSTRUCTOR%", Constructor)
                .Replace("%METHODS%", Methods);

            // pretty print code
            SyntaxNode node = CSharpSyntaxTree.ParseText(CodeText).GetRoot();
            CodeText = node.NormalizeWhitespace().ToFullString();

            Logger.Info(LogMessage.GENERATED_C_SHARP_CODE);

            // add line numbers to the code output
            string printCode = "1\t";
            int line = 1;
            foreach (char c in CodeText)
            {
                if (c == '\n')
                {
                    printCode += "\n" + ++line + "\t";
                    continue;
                }

                printCode += c;
            }

            Logger.Print($"\n{printCode}\n");

            // try to compile the code
            Logger.Info(LogMessage.COMPILING_C_SHARP_CODE);
            CompilerResults result = provider.CompileAssemblyFromSource(parameters, CodeText);

            // print the errors if there are any
            if (result.Errors.Count > 0)
            {
                Logger.Error(LogMessage.COMPILE_ERRORS);

                foreach (CompilerError error in result.Errors)
                {
                    if (!error.IsWarning)
                    {
                        Logger.Error($"\t{error}");
                    }
                }

                return;
            }

            _compiledAssembly = result.CompiledAssembly;
        }

        /// <summary>
        /// Execute the compiled assembly.
        /// If there are runtime erros, they will be printed to the output console.
        /// </summary>
        public void Execute()
        {
            try
            {
                if (_compiledAssembly != null)
                {
                    Type type = _compiledAssembly.GetType("codeOutput.CodeOutput");

                    // pass the logger print method to the CodeOutput class
                    // so that the "schreibe" pseudocode can actually print something
                    Activator.CreateInstance(type, new Action<string, bool>(Logger.Print));
                }
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                Interpreter.HadRuntimeError = true;
            }

            Interpreter.OnStop();
        }
    }
}
