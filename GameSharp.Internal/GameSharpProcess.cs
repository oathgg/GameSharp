using GameSharp.Core;
using GameSharp.Core.Module;
using GameSharp.Core.Native.PInvoke;
using GameSharp.Internal.Memory;
using GameSharp.Internal.Module;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GameSharp.Internal
{
    public sealed class GameSharpProcess : IProcess
    {
        private static GameSharpProcess _instance;

        public static GameSharpProcess Instance => _instance ?? (_instance = new GameSharpProcess());

        public List<IModule> Modules { get; set; }

        public Process Process { get; } = Process.GetCurrentProcess();

        public IModule LoadLibrary(string pathToDll) => LoadLibrary(pathToDll, true);

        public IModule LoadLibrary(string libraryPath, bool resolveReferences)
        {
            Kernel32.LoadLibrary(libraryPath, resolveReferences);

            return Modules.FirstOrDefault(x => x.Name == Path.GetFileName(libraryPath.ToLower()));
        }

        public T CallFunction<T>(SafeFunction safeFunction, params object[] parameters) => safeFunction.Call<T>(parameters);

        public void RefreshModules()
        {
            Process.Refresh();

            Modules.Clear();

            foreach (ProcessModule processModule in Process.Modules)
            {
                Modules.Add(new MemoryModule(processModule));
            }
        }
    }
}
