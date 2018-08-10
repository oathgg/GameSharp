using CsInjection.WinAPI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CsInjection
{
    public class RemoteThreadInjection
    {
        private Process _process;

        public RemoteThreadInjection(Process process)
        {
            _process = process;
        }

        public void Inject(string pathToDll, string entryPoint)
        {
            if (string.IsNullOrWhiteSpace(pathToDll) || !File.Exists(pathToDll))
                throw new ArgumentException(string.Format("Cannot access DLL: \"{0}\"", pathToDll));

            IntPtr processHandle = Kernel32.OpenProcess(Enums.ProcessAccessFlags.All, bInheritHandle: false, processId: _process.Id);

            byte[] pathBytes = Encoding.ASCII.GetBytes(pathToDll);
            var addressOfDllPath = Kernel32.VirtualAllocEx(processHandle, IntPtr.Zero, (uint)pathBytes.Length,
                Enums.AllocationType.Reserve | Enums.AllocationType.Commit,
                Enums.MemoryProtection.ExecuteReadWrite);

            if (Kernel32.WriteProcessMemory(processHandle, addressOfDllPath, pathBytes, pathBytes.Length, out IntPtr a))
            {
                // Gets the base address of the Kernel32.Dll file
                IntPtr kernel32Module = Kernel32.GetModuleHandle(Constants.KERNEL32_DLL);
                // Gets the address of the exported function 'LoadLibraryA' from the kernel32.dll file
                IntPtr loadLibraryAddress = Kernel32.GetProcAddress(kernel32Module, Constants.LOAD_LIBRARY_PROC);
                // Creates a remote thread in the process that will call the function loadlibrary which takes a memory pointer which contains the path to our dll.
                IntPtr remoteThreadHandle = Kernel32.CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAddress, addressOfDllPath, 0, IntPtr.Zero);

                // Dynamically load the DLL into our own process.
                IntPtr myModule = Kernel32.LoadLibraryExA(pathToDll, IntPtr.Zero, Enums.LoadLibraryFlags.DontResolveDllReferences);
                // Get the address of our entry point.
                IntPtr loadLibraryAnsiPtr = Kernel32.GetProcAddress(myModule, entryPoint);
                // Invoke the entry point in the remote process
                IntPtr modulePath = Kernel32.CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAnsiPtr, IntPtr.Zero, 0, IntPtr.Zero);
            }
        }
    }
}
