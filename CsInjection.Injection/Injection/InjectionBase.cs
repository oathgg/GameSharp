using CsInjection.Injection.Extensions;
using CsInjection.Injection.Native;
using System;
using System.Diagnostics;
using System.IO;

namespace CsInjection.Injection.Injection
{
    public abstract class InjectionBase : IInjection
    {
        protected Process _process;

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

            // To Hide our presence we randomize the PE headers of the DLL we have injected.
            _process.RandomizePeHeader(pathToDll);

            Execute(pathToDll, entryPoint);
        }

        public void AttachToProcess()
        {
            // Attaches our current debugger to the process we are injecting to if we currently have a debugger present.
            if (Debugger.IsAttached)
                _process.Attach();
        }

        private void UpdateDlls(string pathToDll)
        {
            string coreDllPath = Path.GetDirectoryName(pathToDll);
            string processPath = Path.GetDirectoryName(_process.MainModule.FileName);
            foreach (string filePath in Directory.GetFiles(Path.GetDirectoryName(pathToDll), "*.dll"))
            {
                string destination = Path.Combine(processPath, Path.GetFileName(filePath));
                Console.WriteLine($"Copying {filePath} to {destination}");
                File.Copy(filePath, destination, overwrite: true);
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
        ///     DLL needs to be the same platform as the game (e.g. x64 or x86).
        /// </summary>
        /// <param name="pathToDll"></param>
        /// <param name="entryPoint"></param>
        protected abstract void Inject(string pathToDll, string entryPoint);
        protected abstract void Execute(string pathToDll, string entryPoint);
    }
}
