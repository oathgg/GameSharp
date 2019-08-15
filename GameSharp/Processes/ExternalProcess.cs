using GameSharp.Module;
using GameSharp.Native;
using GameSharp.Native.Enums;
using GameSharp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace GameSharp.Processes
{
    public class ExternalProcess
    {
        public Process Process { get; }
        public List<ExternalModule> Modules => GetModules();

        public ExternalProcess(Process process)
        {
            Process = process;
        }

        public ExternalModule LoadLibrary(string pathToDll, bool resolveReferences = true)
        {
            byte[] loadLibraryOpcodes = LoadLibraryPayload(pathToDll);

            IntPtr allocatedMemory = Kernel32.VirtualAllocEx(Process.Handle, IntPtr.Zero, (uint)loadLibraryOpcodes.Length, AllocationType.Reserve | AllocationType.Commit, MemoryProtection.ExecuteReadWrite);

            if (Kernel32.WriteProcessMemory(Process.Handle, allocatedMemory, loadLibraryOpcodes, loadLibraryOpcodes.Length, out IntPtr _))
            {
                IntPtr kernel32Module = Kernel32.GetModuleHandle("kernel32.dll");
                if (kernel32Module == IntPtr.Zero)
                {
                    throw new Win32Exception($"Couldn't get handle for module the module, error code: {Marshal.GetLastWin32Error()}.");
                }

                IntPtr loadLibraryAddress = Kernel32.GetProcAddress(kernel32Module, "LoadLibraryW");
                if (loadLibraryAddress == IntPtr.Zero)
                {
                    throw new Win32Exception($"Couldn't get proc address, error code: {Marshal.GetLastWin32Error()}.");
                }

                IntPtr tHandle = Kernel32.CreateRemoteThread(Process.Handle, IntPtr.Zero, 0, loadLibraryAddress, allocatedMemory, 0, IntPtr.Zero);
                if (tHandle == IntPtr.Zero)
                {
                    throw new Win32Exception($"Couldn't create a remote thread, error code: {Marshal.GetLastWin32Error()}.");
                }
            }

            return Modules.FirstOrDefault(x => x.Name == Path.GetFileName(pathToDll));
        }

        // TODO: Refactor to an actual payload, another detection vector is to get the entry point of a thread if its equal to LoadLibrary.
        public byte[] LoadLibraryPayload(string pathToDll)
        {
            if (string.IsNullOrWhiteSpace(pathToDll) || !File.Exists(pathToDll))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            byte[] pathBytes = Encoding.Unicode.GetBytes(pathToDll);

            return pathBytes;
        }

        public List<ExternalModule> GetModules()
        {
            Process.Refresh();

            List<ExternalModule> iModules = new List<ExternalModule>();

            foreach (ProcessModule pm in Process.Modules)
            {
                iModules.Add(new ExternalModule(pm));
            }

            return iModules;
        }

        public void AllocConsole()
        {
            LoggingService.Info($"Creating a console for output from our injected DLL.");

            IntPtr kernel32Module = Kernel32.GetModuleHandle("kernel32.dll");
            IntPtr allocConsoleAddress = Kernel32.GetProcAddress(kernel32Module, "AllocConsole");
            Kernel32.CreateRemoteThread(Process.Handle, IntPtr.Zero, 0, allocConsoleAddress, IntPtr.Zero, 0, IntPtr.Zero);
        }

        public void AttachDebugger()
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

            int tryCount = 5;
            do
            {
                Process.WaitForInputIdle();

                try
                {
                    EnvDTE.Processes processes = dte.Debugger.LocalProcesses;
                    foreach (EnvDTE80.Process2 proc in processes.Cast<EnvDTE80.Process2>().Where(proc => proc.Name.IndexOf(Process.ProcessName) != -1))
                    {
                        EnvDTE80.Engine debugEngine = proc.Transport.Engines.Item("Managed (v4.6, v4.5, v4.0)");
                        proc.Attach2(debugEngine);
                        break;
                    }
                    break;
                }
                catch { }

                Thread.Sleep(1000);
            } while (tryCount-- > 0);
        }

        public void SuspendThreads(bool suspend)
        {
            foreach (ProcessThread pT in Process.Threads)
            {
                IntPtr tHandle = Kernel32.OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (tHandle != IntPtr.Zero)
                {
                    if (suspend)
                    {
                        Kernel32.SuspendThread(tHandle);
                    }
                    else
                    {
                        Kernel32.ResumeThread(tHandle);
                    }

                    // Close the handle; https://docs.microsoft.com/nl-nl/windows/desktop/api/processthreadsapi/nf-processthreadsapi-openthread
                    Kernel32.CloseHandle(tHandle);
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }
    }
}
