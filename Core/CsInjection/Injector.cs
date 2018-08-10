using CsInjection.Core.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsInjection
{
    public class Injector
    {
        private Process _process;

        public Injector(Process process)
        {
            _process = process;
        }

        public void Inject(string pathToDll, string entryPoint)
        {
            if (string.IsNullOrWhiteSpace(pathToDll) || !File.Exists(pathToDll))
                throw new ArgumentException(string.Format("Cannot access DLL: \"{0}\"", pathToDll));

            IntPtr processHandle = WinAPI.OpenProcess(WinAPI.ProcessAccessFlags.All, bInheritHandle: false, processId: _process.Id);

            byte[] pathBytes = Encoding.ASCII.GetBytes(pathToDll);
            var addressOfDllPath = WinAPI.VirtualAllocEx(processHandle, IntPtr.Zero, (uint)pathBytes.Length,
                WinAPI.AllocationType.Reserve | WinAPI.AllocationType.Commit,
                WinAPI.MemoryProtection.ExecuteReadWrite);

            if (WinAPI.WriteProcessMemory(processHandle, addressOfDllPath, pathBytes, pathBytes.Length, out IntPtr a))
            {
                // Gets the base address of the Kernel32.Dll file
                IntPtr kernel32Module = WinAPI.GetModuleHandle(WinAPI.KERNEL32_DLL);
                // Gets the address of the exported function 'LoadLibraryA' from the kernel32.dll file
                IntPtr loadLibraryAddress = WinAPI.GetProcAddress(kernel32Module, WinAPI.LOAD_LIBRARY_PROC);
                // Creates a remote thread in the process that will call the function loadlibrary which takes a memory pointer which contains the path to our dll.
                IntPtr remoteThreadHandle = WinAPI.CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAddress, addressOfDllPath, 0, IntPtr.Zero);

                // Dynamically load the DLL into our own process.
                IntPtr myModule = WinAPI.LoadLibraryExA(pathToDll, IntPtr.Zero, WinAPI.LoadLibraryFlags.DontResolveDllReferences);
                // Get the address of our entry point.
                IntPtr loadLibraryAnsiPtr = WinAPI.GetProcAddress(myModule, entryPoint);
                // Invoke the entry point in the remote process
                IntPtr modulePath = WinAPI.CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAnsiPtr, IntPtr.Zero, 0, IntPtr.Zero);
            }
        }
    }
}
