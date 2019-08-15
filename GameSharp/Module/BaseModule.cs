using System;
using System.Diagnostics;

namespace GameSharp.Module
{
    public class BaseModule
    {
        public readonly ProcessModule ProcessModule;

        public string Name { get; }

        public IntPtr BaseAddress { get; }

        public int Size { get; }

        public BaseModule(ProcessModule module)
        {
            ProcessModule = module;
            Name = ProcessModule.ModuleName.ToLower();
            BaseAddress = ProcessModule.BaseAddress;
            Size = ProcessModule.ModuleMemorySize;
        }

        public override string ToString()
        {
            return $"{Name} 0x{BaseAddress.ToString("X")}";
        }
    }
}
