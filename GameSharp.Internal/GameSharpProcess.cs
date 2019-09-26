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
using System.Runtime.InteropServices;

namespace GameSharp.Internal
{
    public sealed class GameSharpProcess : IProcess
    {
        private GameSharpProcess()
        {
            RefreshModules();
        }

        private static GameSharpProcess _instance;

        public static GameSharpProcess Instance => _instance ?? (_instance = new GameSharpProcess());

        public Dictionary<string, IMemoryModule> Modules { get; private set; } = new Dictionary<string, IMemoryModule>();

        public Process NativeProcess { get; } = Process.GetCurrentProcess();

        public IntPtr Handle => Instance.NativeProcess.Handle;

        public ProcessModule MainModule => Instance.NativeProcess.MainModule;

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

            return Modules[Path.GetFileName(libraryPath.ToLower())];
        }

        public IMemoryAddress AllocateManagedMemory(int size)
        {
            return new MemoryAddress(Marshal.AllocHGlobal(size));
        }

        public void RefreshModules()
        {
            NativeProcess.Refresh();

            Modules.Clear();

            foreach (ProcessModule processModule in NativeProcess.Modules)
            {
                Modules.Add(processModule.ModuleName.ToLower(), new MemoryModule(processModule));
            }
        }
    }
}
