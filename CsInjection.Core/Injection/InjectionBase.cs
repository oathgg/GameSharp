using CsInjection.Core.Extensions;
using CsInjection.Core.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CsInjection.Core.Injection
{
    public abstract class InjectionBase : IInjection
    {
        protected Process _process;
        List<string> _copiedDlls = new List<string>();

        public InjectionBase(Process process)
        {
            _process = process ?? throw new NullReferenceException();
        }

        public void InjectAndExecute(string pathToDll, string entryPoint)
        {
            // Update all DLL files in WoW exe directory which we need to inject.
            UpdateDlls(pathToDll);

            // Creates a console for the output we want to write from the injected program.
            AllocConsole();

            Inject(pathToDll, entryPoint);

            // To hide our presence we randomize the PE headers of the DLL we have injected.
            _process.RandomizePeHeader(pathToDll);

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

        private void UpdateDlls(string pathToDll)
        {
            // Directory of our currently injecting DLL
            string coreDllPath = Path.GetDirectoryName(pathToDll);

            // Full path to the process
            string processPath = Path.GetDirectoryName(_process.MainModule.FileName);

            // Copy all DLLs which our injecting DLL might use which are in the same folder.
            foreach (string filePath in Directory.GetFiles(Path.GetDirectoryName(pathToDll), "CsInjection.Core.dll"))
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
        protected abstract void Execute(string pathToDll, string entryPoint);
    }
}
