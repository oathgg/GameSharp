using CsInjection.Core.Helpers;
using CsInjection.Core.Native;
using CsInjection.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CsInjection.Types
{
    public class RemoteThreadInjection : InjectionBase
    {
        public RemoteThreadInjection(Process process) : base(process)
        {
        }

        protected override void InjectImplementation(string pathToDll, string entryPoint)
        {
            // Sanity check.
            if (string.IsNullOrWhiteSpace(pathToDll) || !File.Exists(pathToDll))
                throw new ArgumentException(string.Format("Cannot access DLL: \"{0}\"", pathToDll));

            // Opens a handle to the process with FULL CONTROL.
            IntPtr processHandle = Kernel32.OpenProcess(Enums.ProcessAccessFlags.All, bInheritHandle: false, processId: _process.Id);

            // Gets the bytes of our path to our dll which will be used to write into the process so the loadlibrary function knows where to look.
            byte[] pathBytes = Encoding.ASCII.GetBytes(pathToDll);

            // Allocates memory the size of our path to our dll in bytes in the remote process.
            var addressOfDllPath = Kernel32.VirtualAllocEx(processHandle, IntPtr.Zero, (uint)pathBytes.Length,
                Enums.AllocationType.Reserve | Enums.AllocationType.Commit,
                Enums.MemoryProtection.ExecuteReadWrite);

            // Write the path to our dll in the newly allocated memory section of the process.
            if (Kernel32.WriteProcessMemory(processHandle, addressOfDllPath, pathBytes, pathBytes.Length, out IntPtr a))
            {
                // Gets the base address of the Kernel32.Dll file
                IntPtr kernel32Module = Kernel32.GetModuleHandle(Constants.KERNEL32_DLL);
                // Gets the address of the exported function 'LoadLibraryA' from the kernel32.dll file
                IntPtr loadLibraryAddress = Kernel32.GetProcAddress(kernel32Module, Constants.LOAD_LIBRARY_PROC);
                // Creates a remote thread in the process that will call the function loadlibrary which takes a memory pointer which contains the path to our dll.
                IntPtr remoteThreadHandle = Kernel32.CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAddress, addressOfDllPath, 0, IntPtr.Zero);

                /* EXECUTE OUR DLL ENTRYPOINT */

                // Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
                IntPtr myModule = Kernel32.LoadLibraryEx(pathToDll, IntPtr.Zero, Enums.LoadLibraryFlags.DontResolveDllReferences);
                // Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
                IntPtr loadLibraryAnsiPtr = Kernel32.GetProcAddress(myModule, entryPoint);
                if (loadLibraryAnsiPtr == IntPtr.Zero)
                {
                    Log.Write("Entry point cannot be found in DLL, is there an export available?");
                }
                // Creates a thread that runs in the virtual address space of another process.
                IntPtr modulePath = Kernel32.CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAnsiPtr, IntPtr.Zero, 0, IntPtr.Zero);
            }
        }
    }
}
