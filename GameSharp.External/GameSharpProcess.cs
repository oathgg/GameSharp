using GameSharp.Core;
using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.PInvoke;
using GameSharp.Core.Native.Structs;
using GameSharp.Core.Services;
using GameSharp.External.Extensions;
using GameSharp.External.Helpers;
using GameSharp.External.Memory;
using GameSharp.External.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace GameSharp.External
{
    public class GameSharpProcess : IProcess
    {
        public Dictionary<string, ModulePointer> Modules => RefreshModules();
        public Process Native { get; }
        public IntPtr Handle { get; }
        public ProcessModule MainModule { get; }
        public bool Is64Bit { get; }
        public MemoryPeb MemoryPeb { get; }
        private IntPtr InternalHandle { get; }
        public GameSharpProcess(Process process)
        {
            Native = process ?? throw new NullReferenceException("process");
            InternalHandle = Kernel32.OpenProcess(ProcessAccess.All, false, Native.Id);
            Handle = Native.Handle;
            MainModule = Native.MainModule;
            Is64Bit = IntPtr.Size == 8;
            MemoryPeb = new MemoryPeb(this);
        }

        public IMemoryPointer GetPebAddress()
        {
            ProcessBasicInformation pbi = new ProcessBasicInformation();

            Ntdll.NtQueryInformationProcess(InternalHandle, ProcessInformationClass.ProcessBasicInformation, ref pbi, pbi.Size, out int _);

            return new MemoryPointer(this, pbi.PebBaseAddress);
        }

        public ModulePointer LoadLibrary(string pathToDll, bool resolveReferences = true)
        {
            byte[] loadLibraryOpcodes = LoadLibraryHelper.LoadLibraryPayload(pathToDll);

            IMemoryPointer allocatedMemory = AllocateManagedMemory(loadLibraryOpcodes.Length);

            if (Kernel32.WriteProcessMemory(Native.Handle, allocatedMemory.Address, loadLibraryOpcodes, loadLibraryOpcodes.Length, out IntPtr _))
            {
                ModulePointer kernel32Module = Modules["kernel32.dll"];
                IMemoryPointer loadLibraryAddress;
                if (resolveReferences)
                {
                    loadLibraryAddress = kernel32Module.GetProcAddress("LoadLibraryW");
                }
                else
                {
                    loadLibraryAddress = kernel32Module.GetProcAddress("LoadLibraryExW");
                }

                if (loadLibraryAddress == null)
                {
                    throw new Win32Exception($"Couldn't get proc address, error code: {Marshal.GetLastWin32Error()}.");
                }

                if (Kernel32.CreateRemoteThread(Native.Handle, IntPtr.Zero, 0, loadLibraryAddress.Address, allocatedMemory.Address, 0, IntPtr.Zero) == IntPtr.Zero)
                {
                    throw new Win32Exception($"Couldn't create a remote thread, error code: {Marshal.GetLastWin32Error()}.");
                }
            }

            ModulePointer injectedModule;

            while (!Modules.TryGetValue(Path.GetFileName(pathToDll).ToLower(), out injectedModule))
            {
                Thread.Sleep(1);
            }

            return injectedModule;
        }

        public Dictionary<string, ModulePointer> RefreshModules()
        {
            Native.Refresh();

            Dictionary<string, ModulePointer> modules = new Dictionary<string, ModulePointer>();

            foreach (ProcessModule processModule in Native.Modules)
            {
                modules.Add(processModule.ModuleName.ToLower(), new ExternalModulePointer(this, processModule));
            }
            
            return modules;
        }

        public void AttachDebugger()
        {
            DebugHelper.SafeAttach(this);
        }

        public IMemoryPointer AllocateManagedMemory(int size)
        {
            return new MemoryPointer(this, Kernel32.VirtualAllocEx(Native.Handle, IntPtr.Zero, (uint)size, AllocationType.Reserve | AllocationType.Commit, MemoryProtection.ExecuteReadWrite));
        }
    }
}
