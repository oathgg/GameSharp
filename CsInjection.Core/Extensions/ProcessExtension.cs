using CsInjection.Core.Native;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace CsInjection.Core.Extensions
{
    public static class ProcessExtension
    {
        public static bool IsWin64Emulator(this Process process)
        {
            if ((Environment.OSVersion.Version.Major > 5)
                || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
            {
                return Kernel32.IsWow64Process(process.Handle, out bool retVal) && retVal;
            }
            return false; // not on 64-bit Windows Emulator
        }

        public static void Attach(this Process process)
        {
            // Reference Visual Studio core
            EnvDTE.DTE dte;
            try
            {
                dte = (EnvDTE.DTE)Marshal.GetActiveObject("VisualStudio.DTE.15.0");
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
                    EnvDTE.Processes processes = dte.Debugger.LocalProcesses;
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

        public static ProcessModule GetProcessModule(this Process process, string moduleName)
        {
            int retryCount = 0;
            ProcessModule module = null;
            do
            {
                process.Refresh();

                // Get an instance of the dll in the process
                module = process.Modules.Cast<ProcessModule>()
                    .SingleOrDefault(m => string.Equals(m.ModuleName, moduleName, StringComparison.OrdinalIgnoreCase));

                Thread.Sleep(1000);
            } while (++retryCount < 5);

            return module;
        }

        public static void RandomizePeHeader (this Process process, string dllPath)
        {
            // Get the name of the dll
            string dllName = Path.GetFileName(dllPath);

            // Get an instance of the dll in the process
            ProcessModule module = process.GetProcessModule(dllPath);

            // Get the base address of the dll
            IntPtr dllBaseAddress = module.BaseAddress;

            // Get the information about the header region of the dll
            int memoryInformationSize = Marshal.SizeOf(typeof(Structs.MemoryBasicInformation));

            if (!Kernel32.VirtualQueryEx(process.Handle, dllBaseAddress, out var memoryInformation, memoryInformationSize))
                throw new Exception("Failed to query the memory.");

            // Create a buffer to write over the header region with
            byte[] buffer = new byte[(int) memoryInformation.RegionSize];

            // Fill the buffer with random bytes
            new Random().NextBytes(buffer);

            // Write over the header region with the buffer
            if (!Kernel32.WriteProcessMemory(process.Handle, dllBaseAddress, buffer, (int)memoryInformation.RegionSize, out IntPtr ignored))
                throw new Exception("Cannot write to memory.");
        }
    }
}