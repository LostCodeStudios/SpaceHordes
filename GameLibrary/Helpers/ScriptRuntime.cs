using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.CodeDom;
using System.Dynamic;
using System.ComponentModel;

namespace GameLibrary.Helpers
{
    public static class ScriptRuntime
    {
        #region Compile

        //        /// <summary>
        //        /// Compiles and runs C# code
        //        /// </summary>
        //        /// <param name="code"></param>
        //        /// <param name="codeNamespace"></param>
        //        /// <param name="parameterList"></param>
        //        /// <param name="dependencies"></param>
        //        /// <param name="variables"></param>
        //        public static void Execute(string code,
        //            object scope)
        //        {

        //            string dependencies = @"using System;
        //                            using System.Collections.Generic;
        //                            using System.Text;
        //                            using GameLibrary.Dependencies.Entities;
        //                            using System.Collections.Concurrent;
        //                            using System.Threading;
        //                            using GameLibrary.Helpers;
        //                            using GameLibrary;";

        //            StringBuilder prg = new StringBuilder();
        //            IndentedTextWriter programmer = new IndentedTextWriter(new StringWriter(prg), "    ");
        //            programmer.Indent = 0;
        //            programmer.WriteLine(dependencies.StripLeadingWhitespace());
        //            programmer.WriteLine("using " + scope.GetType().Namespace +";");
        //                programmer.Indent++;
        //                programmer.WriteLine("public class " + scope.GetType().FullName);

        //                    programmer.WriteLine("{");
        //                    programmer.Indent++;
        //                    programmer.WriteLine("public void Run()\n{");
        //                    programmer.WriteLine(code);
        //                    programmer.WriteLine("}");
        //                    programmer.Indent--;
        //                programmer.WriteLine("}");
        //                programmer.Indent--;

        //                for (int i = 0; i < prg.Length; i++)
        //                {
        //                    if (prg[i] == '\n')
        //                        Console.Write('\n' + (prg.ToString().Take(i).Count(c => c == '\n') + 1).ToString() + ": ");
        //                    else if (i == 0)
        //                        Console.Write(((prg.ToString().Take(i).Count(c => c == '\n') + 1).ToString() + ": ") + prg[i]);
        //                    else
        //                        Console.Write(prg[i]);
        //                }

        //            //Build
        //            Dictionary<string, string> opts = new Dictionary<string, string>();
        //            opts["CompilerVersion"] = "v4.0"; // .NET target
        //            CSharpCodeProvider provider = new CSharpCodeProvider(opts);
        //            CompilerParameters CompilerParams = new CompilerParameters
        //            {
        //                GenerateInMemory = true,
        //                GenerateExecutable = false
        //            };

        //            var curAss = Assembly.GetExecutingAssembly();

        //            CompilerParams.ReferencedAssemblies.Add("System.dll");
        //            CompilerParams.ReferencedAssemblies.Add("mscorlib.dll");
        //            CompilerParams.ReferencedAssemblies.Add("System.dll");
        //            CompilerParams.ReferencedAssemblies.Add("System.Data.dll");
        //            CompilerParams.ReferencedAssemblies.Add(curAss.Location);
        //            CompilerParams.ReferencedAssemblies.Add(scope.GetType().Assembly.Location);
        //            CompilerResults compile = provider.CompileAssemblyFromSource(CompilerParams, prg.ToString());


        //            if (compile.Errors.HasErrors)
        //            {
        //Console.ForegroundColor = ConsoleColor.Red;
        //foreach (CompilerError err in compile.Errors)
        //    Console.WriteLine((err.Line) + "|" + err.Column + ": " + err.ErrorText);
        //Console.ResetColor();
        //            }
        //            else
        //            {
        //                object o = compile.CompiledAssembly.CreateInstance(scope.GetType().FullName, true);
        //                MethodInfo mi = o.GetType().GetMethod("Run");
        //                dynamic inst = scope;
        //                inst.run = mi.
        //                mi.Invoke(scope, null);
        //            }
        //        }

        #endregion

        private const string classTemplate = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.CodeDom;
using System.Dynamic;
using System.ComponentModel;
using Microsoft.Xna.Framework;



            public static class ExTemplate
            {{
                public static Action<object> GetRun(object scope){{
                    return {0};
                }}
            }}
        ";

        public static void RunScope(string code, object scope)
        {
            var finalCode = string.Format(classTemplate, ("new Action<object>((s) => {" + scope.GetType().FullName + " S = s as " + scope.GetType().FullName + "; " + code + "})"));
            var assembly = CompileAssembly(finalCode, scope);
            if (assembly == Assembly.GetExecutingAssembly())
                return;
            var expression = GetExpressionsFromAssembly(scope, assembly);
            expression(scope);

        }

        static Action<object> GetExpressionsFromAssembly(object scope, Assembly assembly)
        {
            var type = assembly.GetType("ExTemplate");
            var method = type.GetMethod("GetRun");
            return (Action<object>)method.Invoke(null, new object[] { scope });
        }

        static Assembly CompileAssembly(string source, object scope)
        {
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
            CompilerParams.ReferencedAssemblies.Add("System.Core.dll");
            CompilerParams.ReferencedAssemblies.Add("mscorlib.dll");
            CompilerParams.ReferencedAssemblies.Add("System.dll");
            CompilerParams.ReferencedAssemblies.Add(Environment.GetEnvironmentVariable("XNAGSv4") + @"\References\Windows\x86\Microsoft.Xna.Framework.dll");

            CompilerParams.ReferencedAssemblies.Add("System.Data.dll");
            CompilerParams.ReferencedAssemblies.Add(curAss.Location);
            CompilerParams.ReferencedAssemblies.Add(scope.GetType().Assembly.Location);
            var results = provider.CompileAssemblyFromSource(CompilerParams, source);
            if (results.Errors.HasErrors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (CompilerError err in results.Errors)
                    Console.WriteLine((err.Line) + "|" + err.Column + ": " + err.ErrorText);
                Console.ResetColor();
                return curAss;
            }
            else
            {
                var assembly = results.CompiledAssembly;
                return assembly;
            }
        }

    }
}
