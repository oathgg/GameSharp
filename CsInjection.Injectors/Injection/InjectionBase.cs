using CsInjection.Core.Helpers;
using CsInjection.Injectors.Extensions;
using System.Diagnostics;

namespace CsInjection.Injectors.Injection
{
    public abstract class InjectionBase : IInjection
    {
        protected Process _process;

        public InjectionBase(Process process)
        {
            _process = process;
        }

        public void InjectAndExecute(string pathToDll, string entryPoint)
        {
            Inject(pathToDll, entryPoint);
            
            // Attaches our current debugger to the process we are injecting to if we currently have a debugger present.
            if (Debugger.IsAttached)
                _process.Attach();

            InjectionHelper.Initialize();
        }

        protected abstract void Inject(string pathToDll, string entryPoint);
        protected abstract void Execute(string pathToDll, string entryPoint);
    }
}
