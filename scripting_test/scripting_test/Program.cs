using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace scripting_test
{
    class Program
    {
        static string source =
            "using System;\n" +
            "class CSharpScript {\n" +
            "static void Main() {\n" +
            " System.Console.WriteLine(\"Hello world!\");\n" +
            " System.Console.ReadKey();\n" +
            "} }\n";

        static void Main(string[] args)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.GenerateExecutable = true;
            cp.GenerateInMemory = true;
            CompilerResults result = provider.CompileAssemblyFromSource(cp, source);

            if (result.Errors.HasErrors)
            {
                string errstr = "Error:\n";
                foreach (CompilerError err in result.Errors)
                    errstr += err.ErrorText + " at line " + err.Line + " column " + err.Column;

                System.Console.WriteLine(errstr);
            }

            Assembly a = result.CompiledAssembly;
            try
            {
                object o = a.CreateInstance("CSharpScript");
                MethodInfo mi = a.EntryPoint;
                mi.Invoke(o, null);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            System.Console.ReadKey();
        }
    }
}
