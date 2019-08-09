using GameSharp.Native;
using System;
using System.Diagnostics;

namespace GameSharp.Memory.Internal
{
    public class InternalModule : IModule
    {
        private ProcessModule ProcessModule { get; }

        public InternalModule(ProcessModule processModule)
        {
            ProcessModule = processModule;
        }

        public IntPtr GetProcAddress(string name) => Kernel32.GetProcAddress(ProcessModule.BaseAddress, name);

        public IntPtr BaseAddress() => ProcessModule.BaseAddress;
    }
}
