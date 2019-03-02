using Sandbox;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using VRage.Plugins;
using Sandbox.ModAPI;
using VRage.Scripting;

namespace SEDebugger
{
    public sealed class SEDebuggerPlugin : IPlugin
    {
        public SEDebuggerPlugin()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyFolder = Path.GetDirectoryName(assemblyLocation);
            MySandboxGame.Log.WriteLine("Assembly basepath: " + assemblyFolder);
        }

        public void Dispose()
        {
            MySandboxGame.Log.WriteLine("SEDebugger - Goodbye...");
        }

        public void Init(object gameInstance)
        {
            MySandboxGame.Log.WriteLine("Hello from SEDebugger Plugin!");

            MySandboxGame.Log.WriteLine("Debug compile configuration enabled");
            MyScriptCompiler.Static.EnableDebugInformation = true;

            MySandboxGame.Log.WriteLine("Adding compilation flag (to enable sedebugger methods)");
            MyScriptCompiler.Static.AddConditionalCompilationSymbols(new string[] {"/define:MOD_DEBUGGER_ACTIVE",});
            var lines = MyScriptCompiler.Static.ConditionalCompilationSymbols.Select(k => k.ToString());
            MySandboxGame.Log.WriteLine("Conditionals: " + string.Join(Environment.NewLine, lines));

            MySandboxGame.Log.WriteLine("Current diagnostic path (for sources/pdbs/etc.):");
            MySandboxGame.Log.WriteLine(MyScriptCompiler.Static.DiagnosticOutputPath);

            MySandboxGame.Log.WriteLine("Final script whitelist (with Debugger namespace added):");
            MySandboxGame.Log.WriteLine(MyScriptCompiler.Static.Whitelist.ToString());
            using (IMyWhitelistBatch myWhitelistBatch = MyScriptCompiler.Static.Whitelist.OpenBatch())
            {
                myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.Both, typeof(Debugger));
            }
            var wh_dict = MyScriptCompiler.Static.Whitelist.GetWhitelist();
            var lines2 = wh_dict.Select(kvp => kvp.Key.ToString());
            MySandboxGame.Log.WriteLine(string.Join(Environment.NewLine, lines2));
            MySandboxGame.Log.WriteLine("SEDebugger init DONE!");
        }

        public void Update()
        {
        }
    }
}
