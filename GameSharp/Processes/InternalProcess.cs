using GameSharp.Interoperability;
using GameSharp.Module;
using GameSharp.Native;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GameSharp.Processes
{
    public sealed class InternalProcess
    {
        public readonly Process Process = Process.GetCurrentProcess();

        private static InternalProcess _instance;

        public static InternalProcess Instance => _instance ?? (_instance = new InternalProcess());

        public List<InternalModule> Modules => GetModules();

        public InternalModule LoadLibrary(string libraryPath, bool resolveReferences = true)
        {
            Kernel32.LoadLibrary(libraryPath, resolveReferences);

            return Modules.FirstOrDefault(x => x.Name == Path.GetFileName(libraryPath.ToLower()));
        }

        public List<InternalModule> GetModules()
        {
            Process.Refresh();

            List<InternalModule> iModules = new List<InternalModule>();

            foreach (ProcessModule pm in Process.Modules)
            {
                iModules.Add(new InternalModule(pm));
            }

            return iModules;
        }

        public T CallFunction<T>(SafeFunction safeFunction, params object[] parameters) => safeFunction.Call<T>(parameters);
    }
}
