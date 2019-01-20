using CsInjection.Core.Helpers;
using CsInjection.Core.Native;
using CsInjection.Injectors.Injection;
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

        protected override void Inject(string pathToDll, string entryPoint)
        {
            if (string.IsNullOrWhiteSpace(pathToDll) || !File.Exists(pathToDll))
                throw new ArgumentException(string.Format($"Cannot access DLL: '{pathToDll}"));

            byte[] pathBytes = Encoding.ASCII.GetBytes(pathToDll);

            // Allocates memory the size of our path to our dll in bytes in the remote process.
            IntPtr allocatedMemory = Kernel32.VirtualAllocEx(_process.Handle, IntPtr.Zero, (uint)pathBytes.Length,
                Enums.AllocationType.Reserve | Enums.AllocationType.Commit, Enums.MemoryProtection.ExecuteReadWrite);

            // Write the path to our dll in the newly allocated memory section of the process.
            if (Kernel32.WriteProcessMemory(_process.Handle, allocatedMemory, pathBytes, pathBytes.Length, out IntPtr a))
            {
                // Gets the base address of the Kernel32.Dll file
                IntPtr kernel32Module = Kernel32.GetModuleHandle(Constants.KERNEL32_DLL);

                // Gets the address of the exported function 'LoadLibraryA' from the kernel32.dll file
                IntPtr loadLibraryAddress = Kernel32.GetProcAddress(kernel32Module, Constants.LOAD_LIBRARY_PROC);

                // Creates a remote thread in the process that will call the function loadlibrary which takes a memory pointer which contains the path to our dll.
                IntPtr remoteThreadHandle = Kernel32.CreateRemoteThread(_process.Handle, IntPtr.Zero, 0, loadLibraryAddress, allocatedMemory, 0, IntPtr.Zero);
            }
        }

        protected override void Execute(string pathToDll, string entryPoint)
        {
            // Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
            IntPtr myModule = Kernel32.LoadLibraryEx(pathToDll, IntPtr.Zero, Enums.LoadLibraryFlags.DontResolveDllReferences);

            // Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
            IntPtr moduleEntryPoint = Kernel32.GetProcAddress(myModule, entryPoint);

            if (moduleEntryPoint == IntPtr.Zero)
            {
                Log.Write("Entry point cannot be found in DLL, is there an export available?");
            }

            // Creates a thread that runs in the virtual address space of another process.
            IntPtr modulePath = Kernel32.CreateRemoteThread(_process.Handle, IntPtr.Zero, 0, moduleEntryPoint, IntPtr.Zero, 0, IntPtr.Zero);
        }
    }
}
