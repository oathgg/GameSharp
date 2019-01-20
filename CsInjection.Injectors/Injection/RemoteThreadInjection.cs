using CsInjection.Core.Utilities;
using CsInjection.Injection.Native;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CsInjection.Injection.Injection
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
            string dllName = Path.GetFileName(pathToDll);

            // Get the DLL base address of the injected DLL in the remote process.
            ProcessModule remoteInjectedDllModule = _process.Modules.Cast<ProcessModule>()
                .SingleOrDefault(m => string.Equals(m.ModuleName, dllName, StringComparison.OrdinalIgnoreCase));

            // Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
            IntPtr localInjectedDllModule = Kernel32.LoadLibraryEx(pathToDll, IntPtr.Zero, Enums.LoadLibraryFlags.DontResolveDllReferences);

            // Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
            IntPtr dllEntryPoint = Kernel32.GetProcAddress(localInjectedDllModule, entryPoint) - (int)localInjectedDllModule;
            if (dllEntryPoint == IntPtr.Zero)
            {
                Log.Write("Entry point cannot be found in DLL, is there an export available?");
            }

            // Calculate the Entry point in the remote process.
            IntPtr dllEntryPointRemoteProcess = remoteInjectedDllModule.BaseAddress + (int)dllEntryPoint;

            // Creates a thread that runs in the virtual address space of another process.
            IntPtr modulePath = Kernel32.CreateRemoteThread(_process.Handle, IntPtr.Zero, 0, dllEntryPointRemoteProcess, IntPtr.Zero, 0, IntPtr.Zero);
        }

        protected override void AllocConsole()
        {
            // Gets the base address of the Kernel32.Dll file
            IntPtr kernel32Module = Kernel32.GetModuleHandle(Constants.KERNEL32_DLL);

            // Gets the address of the exported function 'LoadLibraryA' from the kernel32.dll file
            IntPtr allocConsoleAddress = Kernel32.GetProcAddress(kernel32Module, "AllocConsole");

            // Creates a remote thread in the process that will call the function loadlibrary which takes a memory pointer which contains the path to our dll.
            IntPtr remoteThreadHandle = Kernel32.CreateRemoteThread(_process.Handle, IntPtr.Zero, 0, allocConsoleAddress, IntPtr.Zero, 0, IntPtr.Zero);
        }
    }
}
