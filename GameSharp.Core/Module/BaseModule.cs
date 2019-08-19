using GameSharp.Core.Memory;
using GameSharp.Core.Native.PInvoke;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameSharp.Core.Module
{
    public abstract class ModuleBase : IMemoryModule
    {
        public ProcessModule ProcessModule { get; }
        public string Name { get; }
        public IntPtr BaseAddress { get; }
        public int ModuleMemorySize { get; }
        public IntPtr Handle => Kernel32.GetModuleHandle(Name);

        public abstract IMemoryAddress MemoryAddress { get; }

        public ModuleBase(ProcessModule module)
        {
            ProcessModule = module;
            Name = ProcessModule.ModuleName.ToLower();
            BaseAddress = ProcessModule.BaseAddress;
            ModuleMemorySize = ProcessModule.ModuleMemorySize;
        }

        public override string ToString()
        {
            return $"{Name} 0x{BaseAddress.ToString("X")}";
        }

        public abstract IMemoryAddress GetProcAddress(string name);
    }
}
