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
        public Dictionary<string, IModulePointer> Modules { get; set; } = new Dictionary<string, IModulePointer>();

        public Process Native { get; }

        public IntPtr Handle => Native.Handle;

        public ProcessModule MainModule => Native.MainModule;

        public GameSharpProcess(Process process)
        {
            Native = process ?? throw new NullReferenceException("process");

            RefreshModules();
        }

        public MemoryPeb GetPeb()
        {
            ProcessBasicInformation pbi = new ProcessBasicInformation();

            IMemoryPointer ntResult = AllocateManagedMemory(pbi.Size);

            // Currently only supports 64 bit, 32 bit requires the ProcessInformationClass.ProcessWow64Information enum and a different size.
            Ntdll.NtQueryInformationProcess(Native.Handle, ProcessInformationClass.ProcessBasicInformation, ntResult.Address, pbi.Size, out int _);

            return new MemoryPeb(ntResult);
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

            RefreshModules();

            return Modules[Path.GetFileName(pathToDll).ToLower()];
        }

        public void RefreshModules()
        {
            Thread.Sleep(1000);

            Native.Refresh();

            Modules.Clear();

            foreach (ProcessModule processModule in Native.Modules)
            {
                Modules.Add(processModule.ModuleName.ToLower(), new ModulePointer(this, processModule));
            }
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
