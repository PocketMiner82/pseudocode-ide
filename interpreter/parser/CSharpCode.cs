using pseudocodeIde.interpreter.codeOutput;
using pseudocodeIde.interpreter.logging;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pseudocodeIde.interpreter.parser
{
    public class CSharpCode
    {
        private const string TEMPLATE_CLASS = @"
using System;
using System.Collections.Generic;

namespace pseudocodeIde.interpreter.codeOutput
{
    public class CodeOutput : BaseCodeOutput
    {
        %FIELDS%

        public CodeOutput(Action<string> printMethod) : base(printMethod)
        {
            schreibe(""Hello world!"");
            %CONSTRUCTOR%
        }

        %METHODS%
    }

    public class BaseCodeOutput
    {
        private Action<string> printMethod;

        public BaseCodeOutput(Action<string> printMethod)
        {
            this.printMethod = printMethod;
        }

        protected virtual void schreibe(string msg)
        {
            this.printMethod(msg);
        }
    }
}
";

        public string fields { get; set; } = "";

        public string constructor { get; set; } = "";

        public string methods { get; set; } = "";

        private Assembly compiledAssembly;


        public void compile()
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

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
            

            string code = TEMPLATE_CLASS
                .Replace("%FIELDS%", this.fields)
                .Replace("%CONSTRUCTOR%", this.constructor)
                .Replace("%METHODS%", this.methods);


            CompilerResults result = provider.CompileAssemblyFromSource(parameters, code);

            if (result.Errors.Count > 0)
            {
                Logger.error($"Error while compiling the following generated C# code:\n\n{code}\n");
                Logger.error($"Following errors occurred:");

                foreach(CompilerError error in result.Errors)
                {
                    Logger.error($"- {error}");
                }

                return;
            }

            this.compiledAssembly = result.CompiledAssembly;
        }

        public void execute()
        {
            if (this.compiledAssembly != null)
            {
                Type type = this.compiledAssembly.GetType("pseudocodeIde.interpreter.codeOutput.CodeOutput");
                Activator.CreateInstance(type, new Action<string>(Logger.print));
            }
        }
    }
}
