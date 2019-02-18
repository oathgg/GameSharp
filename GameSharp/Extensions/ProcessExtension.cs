using GameSharp.Native;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace GameSharp.Extensions
{
    public static class ProcessExtension
    {
        public static bool IsWow64(this Process process)
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
                Debug.WriteLine("Visual studio v2017 not found.");
                return;
            }

            // Try loop - Visual Studio may not respond the first time.
            int tryCount = 5;
            do
            {
                Thread.Sleep(5000);

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
                catch
                {
                    // Swallow
                }
            } while (tryCount-- > 0);
        }

        public static ProcessModule GetProcessModule(this Process process, string moduleName)
        {
            int retryCount = 5;
            ProcessModule module = null;
            do
            {
                process.Refresh();

                // Get an instance of the dll in the process
                module = process.Modules.Cast<ProcessModule>()
                    .SingleOrDefault(m => string.Equals(m.ModuleName, moduleName, StringComparison.OrdinalIgnoreCase));

                Thread.Sleep(3000);
            } while (retryCount-- > 0);

            return module;
        }

        /// <summary>
        ///     https://github.com/Akaion/Bleak/blob/master/Bleak/Extensions/RandomiseHeaders.cs
        /// </summary>
        /// <param name="process"></param>
        /// <param name="dllPath"></param>
        public static void RandomizePeHeader (this Process process, string dllPath)
        {
            // Get the name of the dll
            string dllName = Path.GetFileName(dllPath);

            // Get an instance of the dll in the process
            ProcessModule module = process.GetProcessModule(dllName);

            // Get the base address of the dll
            IntPtr dllBaseAddress = module.BaseAddress;

            // Get the information about the header region of the dll
            int memoryInformationSize = Marshal.SizeOf(typeof(Structs.MemoryBasicInformation));

            if (!Kernel32.VirtualQueryEx(process.Handle, dllBaseAddress, out Structs.MemoryBasicInformation memoryInformation, memoryInformationSize))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            // Create a buffer to write over the header region with
            byte[] buffer = new byte[(int) memoryInformation.RegionSize];

            // Fill the buffer with random bytes
            new Random().NextBytes(buffer);

            // Write over the header region with the buffer
            try
            {
                Kernel32.WriteProcessMemory(process.SafeHandle, dllBaseAddress, buffer, (int)memoryInformation.RegionSize, out IntPtr ignored);
            }
            catch (Exception)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}