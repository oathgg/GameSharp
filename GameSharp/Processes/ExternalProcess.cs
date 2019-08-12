using GameSharp.Extensions;
using GameSharp.Memory;
using GameSharp.Memory.External;
using GameSharp.Native;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GameSharp.Processes
{
    public class ExternalProcess : IProcess
    {
        public Process Process { get; }

        public ExternalProcess(Process process)
        {
            Process = process;
        }

        public ProcessModule LoadLibrary(string pathToDll, bool resolveReferences = true)
        {
            if (string.IsNullOrWhiteSpace(pathToDll) || !File.Exists(pathToDll))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            byte[] pathBytes = Encoding.Unicode.GetBytes(pathToDll);

            IntPtr allocatedMemory = Kernel32.VirtualAllocEx(Process.Handle, IntPtr.Zero, (uint)pathBytes.Length, Enums.AllocationType.Reserve | Enums.AllocationType.Commit, Enums.MemoryProtection.ExecuteReadWrite);

            if (Kernel32.WriteProcessMemory(Process.Handle, allocatedMemory, pathBytes, pathBytes.Length, out IntPtr a))
            {
                IntPtr kernel32Module = Kernel32.GetModuleHandle("kernel32.dll");
                if (kernel32Module == IntPtr.Zero)
                    throw new Win32Exception($"Couldn't get handle for module the module, error code: {Marshal.GetLastWin32Error()}.");

                IntPtr loadLibraryAddress = Kernel32.GetProcAddress(kernel32Module, "LoadLibraryW");
                if (loadLibraryAddress == IntPtr.Zero)
                    throw new Win32Exception($"Couldn't get proc address, error code: {Marshal.GetLastWin32Error()}.");

                IntPtr tHandle = Kernel32.CreateRemoteThread(Process.Handle, IntPtr.Zero, 0, loadLibraryAddress, allocatedMemory, 0, IntPtr.Zero);
                if (tHandle == IntPtr.Zero)
                    throw new Win32Exception($"Couldn't create a remote thread, error code: {Marshal.GetLastWin32Error()}.");
            }

            return GetModule(Path.GetFileName(pathToDll));
        }

        public ProcessModule GetModule(string moduleName) => Process.GetProcessModule(moduleName);
    }
}
