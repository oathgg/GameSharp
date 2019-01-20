using CsInjection.Injection.Extensions;
using System;
using System.Diagnostics;
using CsInjection.Injection.Obfuscasion;
using System.Threading;
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
            EraseHeaders.Erase(_process, pathToDll);
            Execute(pathToDll, entryPoint);

            // Attaches our current debugger to the process we are injecting to if we currently have a debugger present.
            // TODO: WoW cancels the debugger out, Anti-Debugger?
            //if (Debugger.IsAttached)
            //    _process.Attach();
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

        protected abstract void Inject(string pathToDll, string entryPoint);
        protected abstract void Execute(string pathToDll, string entryPoint);
        protected abstract void AllocConsole();
    }
}
