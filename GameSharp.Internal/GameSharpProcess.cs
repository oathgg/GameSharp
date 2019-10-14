using GameSharp.Core;
using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.PInvoke;
using GameSharp.Core.Native.Structs;
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
        private static GameSharpProcess _instance;

        public static GameSharpProcess Instance => _instance ?? (_instance = new GameSharpProcess());

        public Dictionary<string, IModulePointer> Modules { get; private set; } = new Dictionary<string, IModulePointer>();

        public Process Native { get; } = Process.GetCurrentProcess();

        public IntPtr Handle => Instance.Native.Handle;

        public ProcessModule MainModule => Instance.Native.MainModule;

        private GameSharpProcess()
        {
            RefreshModules();
        }

        public MemoryPeb GetPeb()
        {
            ProcessBasicInformation pbi = new ProcessBasicInformation();

            IMemoryPointer ntResult = AllocateManagedMemory(pbi.Size);

            Ntdll.NtQueryInformationProcess(Instance.Handle, ProcessInformationClass.ProcessBasicInformation, ntResult.Address, Marshal.SizeOf(pbi), out int _);

            return new MemoryPeb(ntResult);
        }

        public IModulePointer LoadLibrary(string libraryPath, bool resolveReferences)
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

        public IMemoryPointer AllocateManagedMemory(int size)
        {
            return new MemoryPointer(Marshal.AllocHGlobal(size));
        }

        public void RefreshModules()
        {
            Native.Refresh();

            Modules.Clear();

            foreach (ProcessModule processModule in Native.Modules)
            {
                Modules.Add(processModule.ModuleName.ToLower(), new ModulePointer(processModule));
            }
        }
    }
}
