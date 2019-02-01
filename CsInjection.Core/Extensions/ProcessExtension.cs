using CsInjection.Core.Native;
using EnvDTE;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CsInjection.Core.Extensions
{
    public static class ProcessExtension
    {
        public static void Attach(this System.Diagnostics.Process process)
        {
            // Reference Visual Studio core
            DTE dte;
            try
            {
                dte = (DTE)Marshal.GetActiveObject("VisualStudio.DTE.15.0");
            }
            catch (COMException)
            {
                Debug.WriteLine(String.Format(@"Visual studio not found."));
                return;
            }

            // Try loop - Visual Studio may not respond the first time.
            int tryCount = 5;
            while (tryCount-- > 0)
            {
                try
                {
                    Processes processes = dte.Debugger.LocalProcesses;
                    foreach (EnvDTE80.Process2 proc in processes.Cast<EnvDTE80.Process2>().Where(proc => proc.Name.IndexOf(process.ProcessName) != -1))
                    {
                        // Get the debug engine we want to use.
                        EnvDTE80.Engine debugEngine = proc.Transport.Engines.Item("Managed (v4.6, v4.5, v4.0)");
                        proc.Attach2(debugEngine);
                        break;
                    }
                    break;
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        public static void RandomizePeHeader (this System.Diagnostics.Process process, string dllPath)
        {
            // Get the name of the dll
            string dllName = Path.GetFileName(dllPath);

            // Get an instance of the dll in the process
            ProcessModule module;
            do
            {
                process.Refresh();

                // Get an instance of the dll in the process
                module = process.Modules.Cast<ProcessModule>()
                    .SingleOrDefault(m => string.Equals(m.ModuleName, dllName, StringComparison.OrdinalIgnoreCase));
            } while (module is null);

            // Get the base address of the dll
            IntPtr dllBaseAddress = module.BaseAddress;

            // Get the information about the header region of the dll
            int memoryInformationSize = Marshal.SizeOf(typeof(Structs.MemoryBasicInformation));

            if (!Kernel32.VirtualQueryEx(process.Handle, dllBaseAddress, out var memoryInformation, memoryInformationSize))
                throw new Exception("Failed to query the memory of the process");

            // Create a buffer to write over the header region with
            byte[] buffer = new byte[(int) memoryInformation.RegionSize];

            // Fill the buffer with random bytes
            new Random().NextBytes(buffer);

            // Write over the header region with the buffer
            Kernel32.WriteProcessMemory(process.Handle, dllBaseAddress, buffer, (int) memoryInformation.RegionSize, out IntPtr ignored);
        }

        public static void ErasePeHeader(this System.Diagnostics.Process process, string dllPath)
        {
            // Get the id of the process
            int processId = process.Id;

            // Get the name of the dll
            string dllName = Path.GetFileName(dllPath);

            // Get an instance of the dll in the process
            ProcessModule module;
            do
            {
                process.Refresh();

                // Get an instance of the dll in the process
                module = process.Modules.Cast<ProcessModule>()
                    .SingleOrDefault(m => string.Equals(m.ModuleName, dllName, StringComparison.OrdinalIgnoreCase));
            } while (module is null);

            // Get the base address of the dll
            IntPtr dllBaseAddress = module.BaseAddress;

            // Open a handle to the process
            IntPtr processHandle = process.Handle;

            // Get the information about the header region of the dll
            int memoryInformationSize = Marshal.SizeOf(typeof(Structs.MemoryBasicInformation));
            if (!Kernel32.VirtualQueryEx(processHandle, dllBaseAddress, out var memoryInformation, memoryInformationSize))
                throw new Exception("Failed to query the memory of the process");

            // Create a buffer to write over the header region with
            byte[] buffer = new byte[(int)memoryInformation.RegionSize];

            // Write over the header region with the buffer
            Kernel32.WriteProcessMemory(processHandle, dllBaseAddress, buffer, (int)memoryInformation.RegionSize, out IntPtr ignored);
        }
    }
}