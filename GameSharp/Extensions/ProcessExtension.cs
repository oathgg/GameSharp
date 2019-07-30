using GameSharp.Native;
using GameSharp.Utilities;
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
        public static void Attach(this Process process)
        {
            EnvDTE.DTE dte;
            try
            {
                dte = (EnvDTE.DTE)Marshal.GetActiveObject("VisualStudio.DTE.16.0");
            }
            catch (COMException)
            {
                Debug.WriteLine("Visual studio v2019 not found.");
                return;
            }

            // Try loop - Visual Studio may not respond the first time.
            int tryCount = 5;
            do
            {
                process.WaitForInputIdle();

                try
                {
                    EnvDTE.Processes processes = dte.Debugger.LocalProcesses;
                    foreach (EnvDTE80.Process2 proc in processes.Cast<EnvDTE80.Process2>().Where(proc => proc.Name.IndexOf(process.ProcessName) != -1))
                    {
                        EnvDTE80.Engine debugEngine = proc.Transport.Engines.Item("Managed (v4.6, v4.5, v4.0)");
                        proc.Attach2(debugEngine);
                        break;
                    }
                    break;
                }
                catch {}

                Thread.Sleep(1000);
            } while (tryCount-- > 0);
        }

        public static ProcessModule GetProcessModule(this Process process, string moduleName)
        {
            int retryCount = 5;
            ProcessModule module = null;
            do
            {
                // We do a refresh in case something has changed in the process, for example a DLL has been injected.
                process.Refresh();

                module = process.Modules.Cast<ProcessModule>().SingleOrDefault(m => string.Equals(m.ModuleName, moduleName, StringComparison.OrdinalIgnoreCase));

                if (module != null)
                    break;

                Thread.Sleep(1000);
            } while (retryCount-- > 0);

            return module;
        }

        /// <summary>
        ///     Loads the specified module into the address space of the calling process.
        /// </summary>
        /// <param name="libraryPath">
        ///     The name of the module. This can be either a library module (a .dll file) or an executable
        ///     module (an .exe file).
        /// </param>
        /// <returns>A <see cref="ProcessModule" /> corresponding to the loaded library.</returns>
        public static ProcessModule LoadLibrary(this Process process, string libraryPath, bool resolveReferences = true)
        {
            // Check whether the file exists
            if (!File.Exists(libraryPath))
                throw new FileNotFoundException($"Couldn't load the library {libraryPath} because the file doesn't exist.");

            bool failed = resolveReferences
                ? Kernel32.LoadLibrary(libraryPath) == IntPtr.Zero
                : Kernel32.LoadLibraryExW(libraryPath, IntPtr.Zero, Enums.LoadLibraryFlags.DontResolveDllReferences) == IntPtr.Zero;

            if (failed)
                throw new Win32Exception($"Couldn't load the library {libraryPath}.");

            process.Refresh();

            // Enumerate the loaded modules and return the one newly added
            return process.Modules.Cast<ProcessModule>().First(m => m.FileName == libraryPath);
        }

        /// <summary>
        ///     https://github.com/Akaion/Bleak/blob/master/Bleak/Extensions/RandomiseHeaders.cs 
        ///     I've made it compatible with my codebase, however this is where the idea came from.
        ///     
        ///     We cannot call the default extension methods, as these methods are only available once we are injected.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="dllPath"></param>
        public static void RandomizePeHeader(this Process process, string moduleName)
        {
            Logger.Info($"Randomizing PE header for {moduleName}.");

            IntPtr dllBaseAddress = process.GetProcessModule(moduleName).BaseAddress;

            Kernel32.VirtualQueryEx(process.Handle, dllBaseAddress, out Structs.MemoryBasicInformation memoryInformation, Marshal.SizeOf<Structs.MemoryBasicInformation>());
            byte[] buffer = new byte[(int)memoryInformation.RegionSize];
            new Random().NextBytes(buffer);
            Kernel32.WriteProcessMemory(process.Handle, dllBaseAddress, buffer, (int)memoryInformation.RegionSize, out IntPtr _);
        }
    }
}