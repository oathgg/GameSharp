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
            string coreDllPath = Path.GetDirectoryName(pathToDll);
            string processPath = Path.GetDirectoryName(_process.MainModule.FileName);
            string coreFileName = "CsInjection.Core.dll";
            File.Copy(Path.Combine(coreDllPath, coreFileName), Path.Combine(processPath, coreFileName), overwrite: true);

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

        protected abstract void Inject(string pathToDll, string entryPoint);
        protected abstract void Execute(string pathToDll, string entryPoint);
        protected abstract void AllocConsole();
    }
}
