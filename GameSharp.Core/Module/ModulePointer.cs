using GameSharp.Core.Memory;
using GameSharp.Core.Native.PInvoke;
using System;
using System.Diagnostics;

namespace GameSharp.Core.Module
{
    public abstract class ModulePointer : IModulePointer
    {
        public ProcessModule ProcessModule { get; }
        public string Name { get; }
        public IntPtr Address { get; }
        public int Size { get; }
        public IntPtr Handle => Kernel32.GetModuleHandle(Name);

        public ModulePointer(ProcessModule module)
        {
            ProcessModule = module;
            Name = ProcessModule.ModuleName.ToLower();
            Address = ProcessModule.BaseAddress;
            Size = ProcessModule.ModuleMemorySize;
        }

        public override string ToString()
        {
            return $"{Name} 0x{Address.ToString("X")}";
        }

        public abstract IMemoryPointer GetProcAddress(string name);
        public abstract IMemoryPointer MemoryPointer { get; }
    }
}
