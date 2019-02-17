using GameSharp.Extensions;
using GameSharp.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
        public void InjectAndExecute(string pathToDll, string entryPoint)
        {
            // Update all DLL files in WoW exe directory which we need to inject.
            UpdateDlls(pathToDll);
            
            //
            // TODO: Pause all threads before injecting in case of anti-cheat.
            //

            // Injects our DLL into the specified process.
            Inject(pathToDll, entryPoint);

            // To hide our presence we randomize the PE headers of the DLL we have injected.
            //FIX: _process.RandomizePeHeader(pathToDll);

            // Creates a console for the output we want to write from the injected program.
            AllocConsole();

            // Executes the entry point of the DLL.
            Execute(pathToDll, entryPoint);
        }

        /// <summary>
        ///     Attaches the debugger to the process, we need to hide our presence here.
        /// </summary>
        public void AttachToProcess()
        {
            // Attaches our current debugger to the process we are injecting to if we currently have a debugger present.
            if (Debugger.IsAttached)
                _process.Attach();
        }

        /// <summary>
        ///     Updates all the DLLs which we require for the injection to succeed.
        /// </summary>
        /// <param name="pathToDll"></param>
        private void UpdateDlls(string pathToDll)
        {
            // Directory of our currently injecting DLL
            string coreDllPath = Path.GetDirectoryName(pathToDll);

            // Full path to the process
            string processPath = Path.GetDirectoryName(_process.MainModule.FileName);

            // Copy all DLLs which our injecting DLL might use which are in the same folder.
            foreach (string filePath in Directory.GetFiles(Path.GetDirectoryName(pathToDll), "GameSharp.dll"))
            {
                string destination = Path.Combine(processPath, Path.GetFileName(filePath));
                File.Copy(filePath, destination, overwrite: true);
                _copiedDlls.Add(destination);
            }

            // Once the process exits we want to cleanup any lingering DLLs
            _process.Exited += CleanUpDlls;
        }

        /// <summary>
        ///     Gets triggered once the process exits and removes all copied Dlls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CleanUpDlls(object sender, EventArgs e)
        {
            foreach (string copiedDll in _copiedDlls)
            {
                File.Delete(copiedDll);
            }
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
            IntPtr remoteThreadHandle = Kernel32.CreateRemoteThread(_process.Handle, IntPtr.Zero, 0, allocConsoleAddress, IntPtr.Zero, 0, IntPtr.Zero);
        }

        /// <summary>
        ///     DLL needs to be the same platform as the target process (e.g. x64 or x86).
        /// </summary>
        /// <param name="pathToDll"></param>
        /// <param name="entryPoint"></param>
        protected abstract void Inject(string pathToDll, string entryPoint);

        /// <summary>
        ///     Executes the specified entry point.
        /// </summary>
        /// <param name="pathToDll"></param>
        /// <param name="entryPoint"></param>
        protected abstract void Execute(string pathToDll, string entryPoint);
    }
}
