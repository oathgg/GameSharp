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
        public Dictionary<string, IModulePointer> Modules => RefreshModules();

        public Process Native { get; }

        public IntPtr Handle { get; }

        public ProcessModule MainModule { get; }

        public GameSharpProcess(Process process)
        {
            Native = process ?? throw new NullReferenceException("process");
            Handle = Native.Handle;
            MainModule = Native.MainModule;
        }

        public MemoryPeb GetPeb()
        {
            ProcessBasicInformation pbi = new ProcessBasicInformation();

            IMemoryPointer ntResult = AllocateManagedMemory(pbi.Size);

            // Currently only supports 64 bit, 32 bit requires the ProcessInformationClass.ProcessWow64Information enum and a different size.
            Ntdll.NtQueryInformationProcess(Native.Handle, ProcessInformationClass.ProcessBasicInformation, ntResult.Address, pbi.Size, out int _);

            IMemoryPointer pebPointer = new MemoryPointer(this, ntResult.Read<ProcessBasicInformation>().PebBaseAddress);

            return new MemoryPeb(pebPointer);
        }

        public IModulePointer LoadLibrary(string pathToDll, bool resolveReferences = true)
        {
            byte[] loadLibraryOpcodes = LoadLibraryHelper.LoadLibraryPayload(pathToDll);

            IMemoryPointer allocatedMemory = AllocateManagedMemory(loadLibraryOpcodes.Length);

            if (Kernel32.WriteProcessMemory(Native.Handle, allocatedMemory.Address, loadLibraryOpcodes, loadLibraryOpcodes.Length, out IntPtr _))
            {
                IModulePointer kernel32Module = Modules["kernel32.dll"];
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

            IModulePointer injectedModule;

            while (!Modules.TryGetValue(Path.GetFileName(pathToDll).ToLower(), out injectedModule))
            {
                Thread.Sleep(1000);
            }

            return injectedModule;
        }

        public Dictionary<string, IModulePointer> RefreshModules()
        {
            Native.Refresh();

            Dictionary<string, IModulePointer> modules = new Dictionary<string, IModulePointer>();

            foreach (ProcessModule processModule in Native.Modules)
            {
                modules.Add(processModule.ModuleName.ToLower(), new ModulePointer(this, processModule));
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
