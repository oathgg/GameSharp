using System;
using System.Diagnostics;

namespace GameSharp.Memory.External
{
    public class ExternalModule : IModule
    {
        private ProcessModule ProcessModule { get; }

        public ExternalModule(ProcessModule processModule)
        {
            ProcessModule = processModule;
        }

        public IntPtr GetProcAddress(string name)
        {
            throw new NotImplementedException();
        }

        public IntPtr BaseAddress() => ProcessModule.BaseAddress;
    }
}
