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
            // Attaches our current debugger to the process we are injecting to if we currently have a debugger present.
            //if (Debugger.IsAttached)
            //    _process.Attach();

            // Creates a console for the output we want to write from the injected program.
            AllocConsole();

            Inject(pathToDll, entryPoint);
            EraseHeaders.Erase(_process, pathToDll);
            Execute(pathToDll, entryPoint);
        }

        protected abstract void Inject(string pathToDll, string entryPoint);
        protected abstract void Execute(string pathToDll, string entryPoint);
        protected abstract void AllocConsole();
    }
}
