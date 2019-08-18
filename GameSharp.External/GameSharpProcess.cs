using GameSharp.Core;
using GameSharp.Core.Module;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.PInvoke;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GameSharp.External
{
    public class GameSharpProcess : IProcess
    {
        public List<IModule> Modules { get; set; }

        public Process Process { get; }

        public GameSharpProcess(Process process)
        {
            Process = process;
        }

        public IModule LoadLibrary(string pathToDll) => LoadLibrary(pathToDll, true);

        public IModule LoadLibrary(string pathToDll, bool resolveReferences)
        {
            byte[] loadLibraryOpcodes = LoadLibraryPayload(pathToDll);

            IntPtr allocatedMemory = Kernel32.VirtualAllocEx(Process.Handle, IntPtr.Zero, (uint)loadLibraryOpcodes.Length, AllocationType.Reserve | AllocationType.Commit, MemoryProtection.ExecuteReadWrite);

            if (Kernel32.WriteProcessMemory(Process.Handle, allocatedMemory, loadLibraryOpcodes, loadLibraryOpcodes.Length, out IntPtr _))
            {
                IntPtr kernel32Module = Kernel32.GetModuleHandle("kernel32.dll");
                if (kernel32Module == IntPtr.Zero)
                {
                    throw new Win32Exception($"Couldn't get handle for module the module, error code: {Marshal.GetLastWin32Error()}.");
                }

                IntPtr loadLibraryAddress = Kernel32.GetProcAddress(kernel32Module, "LoadLibraryW");
                if (loadLibraryAddress == IntPtr.Zero)
                {
                    throw new Win32Exception($"Couldn't get proc address, error code: {Marshal.GetLastWin32Error()}.");
                }

                IntPtr tHandle = Kernel32.CreateRemoteThread(Process.Handle, IntPtr.Zero, 0, loadLibraryAddress, allocatedMemory, 0, IntPtr.Zero);
                if (tHandle == IntPtr.Zero)
                {
                    throw new Win32Exception($"Couldn't create a remote thread, error code: {Marshal.GetLastWin32Error()}.");
                }
            }

            return Modules.FirstOrDefault(x => x.Name == Path.GetFileName(pathToDll));
        }

        // TODO: Refactor to an actual payload, another detection vector is to get the entry point of a thread if its equal to LoadLibrary.
        private byte[] LoadLibraryPayload(string pathToDll)
        {
            if (string.IsNullOrWhiteSpace(pathToDll) || !File.Exists(pathToDll))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            byte[] pathBytes = Encoding.Unicode.GetBytes(pathToDll);

            return pathBytes;
        }

        public void RefreshModules()
        {
            Process.Refresh();

            Modules.Clear();

            foreach (ProcessModule processModule in Process.Modules)
            {
                Modules.Add(new Module.MemoryModule(processModule));
            }
        }
    }
}
