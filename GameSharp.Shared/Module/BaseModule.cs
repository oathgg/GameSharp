using GameSharp.Core.Memory;
using System;
using System.Diagnostics;

namespace GameSharp.Core.Module
{
    public abstract class ModuleBase : IModule
    {
        public ProcessModule ProcessModule { get; }

        public string Name { get; }

        public IntPtr BaseAddress { get; }

        public int Size { get; }

        public abstract IMemoryAddress MemoryAddress { get; }

        public ModuleBase(ProcessModule module)
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
