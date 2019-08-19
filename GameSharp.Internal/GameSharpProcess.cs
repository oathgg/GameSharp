using GameSharp.Core;
using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.PInvoke;
using GameSharp.Internal.Memory;
using GameSharp.Internal.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GameSharp.Internal
{
    public sealed class GameSharpProcess : IProcess
    {
        private static GameSharpProcess _instance;

        public static GameSharpProcess Instance => _instance ?? (_instance = new GameSharpProcess());

        public List<IMemoryModule> Modules { get; private set; } = new List<IMemoryModule>();

        public Process Process { get; } = Process.GetCurrentProcess();

        public IntPtr Handle => Instance.Process.Handle;

        public ProcessModule MainModule => Instance.Process.MainModule;

        public IMemoryModule LoadLibrary(string pathToDll)
        {
            return LoadLibrary(pathToDll, true);
        }

        public IMemoryModule LoadLibrary(string libraryPath, bool resolveReferences)
        {
            if (!File.Exists(libraryPath))
            {
                throw new FileNotFoundException(libraryPath);
            }

            IntPtr libraryAddress = resolveReferences
                ? Kernel32.LoadLibrary(libraryPath)
                : Kernel32.LoadLibraryExW(libraryPath, IntPtr.Zero, LoadLibraryFlags.DontResolveDllReferences);

            if (libraryAddress == IntPtr.Zero)
            {
                throw new Win32Exception($"Couldn't load the library {libraryPath}.");
            }

            return Modules.FirstOrDefault(x => x.Name == Path.GetFileName(libraryPath.ToLower()));
        }

        public T CallFunction<T>(SafeFunction safeFunction, params object[] parameters)
        {
            return safeFunction.Call<T>(parameters);
        }

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
