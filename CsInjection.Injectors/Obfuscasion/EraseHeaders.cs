using CsInjection.Core.Extensions;
using CsInjection.Injection.Native;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace CsInjection.Injection.Obfuscasion
{
    internal class EraseHeaders
    {
        public static void Erase(Process process, string dllPath)
        {
            // Get the id of the process
            int processId = process.Id;

            // Get the name of the dll
            string dllName = Path.GetFileName(dllPath);

            // Get an instance of the dll in the process
            ProcessModule module = process.Modules.Cast<ProcessModule>()
                .SingleOrDefault(m => string.Equals(m.ModuleName, dllName, StringComparison.OrdinalIgnoreCase));

            if (module is null)
            {
                throw new ArgumentException($"There is no module named {dllName} loaded in the process");
            }

            // Get the base address of the dll
            IntPtr dllBaseAddress = module.BaseAddress;

            // Open a handle to the process
            IntPtr processHandle = process.Handle;

            // Get the information about the header region of the dll
            int memoryInformationSize = Marshal.SizeOf(typeof(Structs.MemoryBasicInformation));
            if (!Kernel32.VirtualQueryEx(processHandle, dllBaseAddress, out var memoryInformation, memoryInformationSize))
            {
                throw new Exception("Failed to query the memory of the process");
            }

            // Create a buffer to write over the header region with
            byte[] buffer = new byte[(int)memoryInformation.RegionSize];

            // Write over the header region with the buffer
            Kernel32.WriteProcessMemory(processHandle, dllBaseAddress, buffer, (int) memoryInformation.RegionSize, out IntPtr ignored);
        }
    }
}
