using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using System.Text.RegularExpressions;

namespace GameLibrary.Helpers
{
    public static class ScriptRuntime
    {
        private static void RunInScope(string code)
        {
            
        }

        /// <summary>
        /// Compiles and runs C# code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="codeNamespace"></param>
        /// <param name="parameterList"></param>
        /// <param name="dependencies"></param>
        /// <param name="variables"></param>
        public static void CompileAndRun(string code,
            string codeNamespace = "GameLibrary.Scripting",
            string parameterList = "",
            string dependencies = @"using System;
                            using System.Collections.Generic;
                            using System.Text;
                            using GameLibrary.Dependencies.Entities;
                            using System.Collections.Concurrent;
                            using System.Threading;
                            using GameLibrary.Helpers;
                            using GameLibrary;",
            params object[] variables)
        {
            StringBuilder prg = new StringBuilder();
            IndentedTextWriter programmer = new IndentedTextWriter(new StringWriter(prg), "    ");
            programmer.Indent = 0;
            programmer.WriteLine(dependencies.StripLeadingWhitespace());
            programmer.WriteLine("namespace " + codeNamespace + "\n{");
                programmer.Indent++;
                programmer.WriteLine("public class ScriptRuntime\n{");
                    programmer.Indent++;
                    programmer.WriteLine("public void Run(" + parameterList + ")\n{");
                    programmer.WriteLine(code);
                    programmer.WriteLine("}");
                    programmer.Indent--;
                programmer.WriteLine("}");
                programmer.Indent--;
            programmer.WriteLine("}");

            for (int i = 0; i < prg.Length; i++)
            {
                if (prg[i] == '\n')
                    Console.Write((prg.ToString().Take(i).Count(c => c == '\n') + 1).ToString() + ": ");
                else if (i == 0)
                    Console.Write(((prg.ToString().Take(i).Count(c => c == '\n') + 1).ToString() + ": ") + prg[i]);
                else
                    Console.Write(prg[i]);
            }

            //Build
            Dictionary<string, string> opts = new Dictionary<string, string>();
            opts["CompilerVersion"] = "v4.0"; // .NET target
            CSharpCodeProvider provider = new CSharpCodeProvider(opts);

            CompilerParameters CompilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false
            };

            var curAss = Assembly.GetExecutingAssembly();

            CompilerParams.ReferencedAssemblies.Add("System.dll");
            CompilerParams.ReferencedAssemblies.Add("mscorlib.dll");
            CompilerParams.ReferencedAssemblies.Add("System.dll");
            CompilerParams.ReferencedAssemblies.Add("System.Data.dll");
            CompilerParams.ReferencedAssemblies.Add(curAss.Location);


            CompilerResults compile = provider.CompileAssemblyFromSource(CompilerParams, programmer.ToString());


            if (compile.Errors.HasErrors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (CompilerError err in compile.Errors)
                    Console.WriteLine((err.Line) + "|" + err.Column + ": " + err.ErrorText);
                Console.ResetColor();
            }
            else
            {
                ExpoloreAssembly(compile.CompiledAssembly);
                object o = compile.CompiledAssembly.CreateInstance(codeNamespace + ".ScriptRuntime");
                MethodInfo mi = o.GetType().GetMethod("Execute");
                mi.Invoke(o, variables);
            }
        }

        public static void CompileAndRun(string code, string paramList, params object[] parameters)
        {
            CompileAndRun(code,parameterList: paramList, variables: parameters);
        }

        public static void ExpoloreAssembly(Assembly assembly)
        {
            Console.WriteLine("Modules in the assembly:");
            foreach (Module m in assembly.GetModules())
            {
                Console.WriteLine("{0}", m);

                foreach (Type t in m.GetTypes())
                {
                    Console.WriteLine("t{0}", t.Name);

                    foreach (MethodInfo mi in t.GetMethods())
                    {
                        Console.WriteLine("tt{0}", mi.Name);
                    }
                }
            }
        }
    }
}
