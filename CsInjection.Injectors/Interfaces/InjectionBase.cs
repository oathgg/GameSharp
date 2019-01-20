using CsInjection.Core.Helpers;
using CsInjection.Injectors.Extensions;
using System.Diagnostics;

namespace CsInjection.Interfaces
{
    public abstract class InjectionBase : IInjection
    {
        protected Process _process;

        public InjectionBase(Process process)
        {
            _process = process;
        }

        public void Inject(string pathToDll, string entryPoint)
        {
            InjectImplementation(pathToDll, entryPoint);

            // Attaches our current debugger to the process we are injecting to if we currently have a debugger present.
            if (Debugger.IsAttached)
                _process.Attach();

            InjectionHelper.Initialize();
        }

        public abstract void InjectImplementation(string pathToDll, string entryPoint);
    }
}
