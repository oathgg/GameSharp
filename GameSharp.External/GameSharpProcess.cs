using GameSharp.Core;
using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.PInvoke;
using GameSharp.Core.Services;
using GameSharp.External.Helpers;
using GameSharp.External.Memory;
using GameSharp.External.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace GameSharp.External
{
    public class GameSharpProcess : IProcess
    {
        public Dictionary<string, IMemoryModule> Modules { get; set; } = new Dictionary<string, IMemoryModule>();

        public Process NativeProcess { get; }

        public IntPtr Handle => NativeProcess.Handle;

        public ProcessModule MainModule => NativeProcess.MainModule;

        public GameSharpProcess(Process process)
        {
            NativeProcess = process;

            RefreshModules();
        }

        public IMemoryModule LoadLibrary(string pathToDll, bool resolveReferences = true)
        {
            byte[] loadLibraryOpcodes = LoadLibraryPayload(pathToDll);

            IMemoryAddress allocatedMemory = AllocateManagedMemory(loadLibraryOpcodes.Length);

            if (Kernel32.WriteProcessMemory(NativeProcess.Handle, allocatedMemory.BaseAddress, loadLibraryOpcodes, loadLibraryOpcodes.Length, out IntPtr _))
            {
                IMemoryModule kernel32Module = Modules["kernel32.dll"];
                IMemoryAddress loadLibraryAddress;
                if (resolveReferences)
                {
                    loadLibraryAddress = kernel32Module.GetProcAddress("LoadLibraryW");
                }
                else
                {
                    loadLibraryAddress = kernel32Module.GetProcAddress("LoadLibraryExW");
                }

                if (loadLibraryAddress == null)
                {
                    throw new Win32Exception($"Couldn't get proc address, error code: {Marshal.GetLastWin32Error()}.");
                }

                if (CreateRemoteThread(loadLibraryAddress, allocatedMemory) == IntPtr.Zero)
                {
                    throw new Win32Exception($"Couldn't create a remote thread, error code: {Marshal.GetLastWin32Error()}.");
                }
            }

            RefreshModules();

            return Modules[Path.GetFileName(pathToDll).ToLower()];
        }

        // TODO: Refactor to an actual payload, another detection vector is to get the entry point of a thread if its equal to LoadLibrary.
        private byte[] LoadLibraryPayload(string pathToDll)
        {
            if (string.IsNullOrWhiteSpace(pathToDll) || !File.Exists(pathToDll))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            byte[] pathBytes = Encoding.Unicode.GetBytes(pathToDll);

            return pathBytes;
        }

        public void RefreshModules()
        {
            NativeProcess.Refresh();

            Modules.Clear();

            foreach (ProcessModule processModule in NativeProcess.Modules)
            {
                Modules.Add(processModule.ModuleName.ToLower(), new MemoryModule(this, processModule));
            }

            Thread.Sleep(1000);
        }

        public void AllocConsole()
        {
            LoggingService.Info($"Creating a console for output from our injected DLL.");

            IntPtr kernel32Module = Kernel32.GetModuleHandle("kernel32.dll");
            IntPtr allocConsoleAddress = Kernel32.GetProcAddress(kernel32Module, "AllocConsole");
            Kernel32.CreateRemoteThread(NativeProcess.Handle, IntPtr.Zero, 0, allocConsoleAddress, IntPtr.Zero, 0, IntPtr.Zero);
        }

        public void AttachDebugger() => DebugHelper.SafeAttach(this);

        public void SuspendThreads(bool suspend)
        {
            foreach (ProcessThread pT in NativeProcess.Threads)
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

        public IMemoryAddress AllocateManagedMemory(int size)
        {
            return new MemoryAddress(this, Kernel32.VirtualAllocEx(NativeProcess.Handle, IntPtr.Zero, (uint) size, AllocationType.Reserve | AllocationType.Commit, MemoryProtection.ExecuteReadWrite));
        }

        public IntPtr CreateRemoteThread(IMemoryAddress entryPoint, IMemoryAddress arguments)
        {
            return Kernel32.CreateRemoteThread(NativeProcess.Handle, IntPtr.Zero, 0, entryPoint.BaseAddress, arguments.BaseAddress, 0, IntPtr.Zero);
        }
    }
}
