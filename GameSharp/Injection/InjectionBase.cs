using GameSharp.Extensions;
using GameSharp.Native;
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
        /// <summary>
        ///     Process of to inject into.
        /// </summary>
        protected Process _process;

        /// <summary>
        ///     List of all the DLLs which have been transfered.
        /// </summary>
        List<string> _copiedDlls = new List<string>();

        public InjectionBase(Process process)
        {
            _process = process ?? throw new NullReferenceException();
        }

        /// <summary>
        ///     Injects and executes the DLL through the specified injection method.
        /// </summary>
        /// <param name="pathToDll"></param>
        /// <param name="entryPoint"></param>
        public void InjectAndExecute(string pathToDll, string entryPoint, bool attach)
        {
            // Update all DLL files in WoW exe directory which we need to inject.
            UpdateFiles(pathToDll);

            // Pause all threads before injecting in case of anti-cheat.
            SuspendThreads(true);

            // Injects our DLL into the specified process.
            Inject(pathToDll);

            // To hide our presence we randomize the PE headers of the DLL we have injected.
            _process.RandomizePeHeader(pathToDll);

            // Creates a console for the output we want to write from the injected program.
            AllocConsole();

            // Attach to the remote process if wanted.
            if (attach)
                AttachToProcess();

            // Executes the entry point of the DLL.
            Execute(pathToDll, entryPoint);

            SuspendThreads(false);
        }

        /// <summary>
        ///     Suspends or resumes all active threads
        /// </summary>
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
            IntPtr kernel32Module = Kernel32.GetModuleHandle(Constants.KERNEL32_DLL);

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
        ///     Executes the specified entry point.
        /// </summary>
        /// <param name="pathToDll"></param>
        /// <param name="entryPoint"></param>
        protected abstract void Execute(string pathToDll, string entryPoint);
    }
}
