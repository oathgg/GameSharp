using GameSharp.Extensions;
using GameSharp.Native;
using GameSharp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace GameSharp.Injection
{
    public abstract class InjectionBase : IInjection
    {
        protected Process _process;

        public InjectionBase(Process process)
        {
            _process = process ?? throw new NullReferenceException();
        }

        /// <summary>
        ///     Injects and executes the DLL through the IInjection injection method.
        /// </summary>
        /// <param name="pathToDll"></param>
        /// <param name="entryPoint"></param>
        public void InjectAndExecute(string pathToDll, string entryPoint, bool attach)
        {
            // Update all DLL files in the exe directory which we need to inject or resolve references to.
            UpdateFiles(pathToDll);

            // Pause all threads before injecting in case of anti-cheat.
            SuspendThreads(true);

            Logger.Info($"Injecting DLL '{pathToDll}'.");
            Inject(pathToDll);

            // Possible anti-cheat detterence
            string moduleName = Path.GetFileName(pathToDll);
            _process.RandomizePeHeader(moduleName);

            // Attach to the remote process if wanted.
            if (attach)
                AttachToProcess();

            Logger.Info($"Creating a console for output from our injected DLL.");
            AllocConsole();

            Execute(pathToDll, entryPoint);

            SuspendThreads(false);
        }

        private void SuspendThreads(bool suspend)
        {
            foreach (ProcessThread pT in _process.Threads)
            {
                // Get the handle of the current thread.
                IntPtr tHandle = Kernel32.OpenThread(Enums.ThreadAccessFlags.SUSPEND_RESUME, false, (uint)pT.Id);

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

        /// <summary>
        ///     Attaches the debugger to the process, we need to hide our presence here.
        /// </summary>
        private void AttachToProcess()
        {
            // Attaches our current debugger to the process we are injecting to if we currently have a debugger present.
            Logger.Info($"Attaching our debugger to the remote process.");
            if (Debugger.IsAttached)
                _process.Attach();
        }

        /// <summary>
        ///     Updates all the DLLs which we require for the injection to succeed.
        /// </summary>
        /// <param name="pathToDll"></param>
        private void UpdateFiles(string pathToDll)
        {
            // Full path to the process
            string processPath = Path.GetDirectoryName(_process.MainModule.FileName);

            // Copy all DLLs which our injecting DLL might use which are in the same folder.
            CopyFile(pathToDll, "GameSharp.dll", processPath);
        }

        private void CopyFile(string pathToDll, string dllName, string destination)
        {
            string source = Path.Combine(Path.GetDirectoryName(pathToDll), dllName);
            string destinationFullName = Path.Combine(destination, dllName);

            File.Copy(source, destinationFullName, true);
        }

        /// <summary>
        ///     Allocates a console window by using the native APIs.
        /// </summary>
        private void AllocConsole()
        {
            // Gets the base address of the Kernel32.Dll file
            IntPtr kernel32Module = Kernel32.GetModuleHandle("kernel32.dll");

            // Gets the address of the exported function 'LoadLibraryA' from the kernel32.dll file
            IntPtr allocConsoleAddress = Kernel32.GetProcAddress(kernel32Module, "AllocConsole");

            // Creates a remote thread in the process that will call the function AllocConsole,
            Kernel32.CreateRemoteThread(_process.Handle, IntPtr.Zero, 0, allocConsoleAddress, IntPtr.Zero, 0, IntPtr.Zero);
        }

        /// <summary>
        ///     DLL needs to be the same platform as the target process (e.g. x64 or x86).
        /// </summary>
        /// <param name="pathToDll"></param>
        /// <param name="entryPoint"></param>
        protected abstract void Inject(string pathToDll);

        /// <summary>
        ///     Execution after the DLL has been injected.
        /// </summary>
        /// <param name="pathToDll"></param>
        /// <param name="entryPoint"></param>
        protected abstract void Execute(string pathToDll, string entryPoint);
    }
}
