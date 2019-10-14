using GameSharp.Core.Memory;
using GameSharp.Core.Native.PInvoke;
using System;
using System.Diagnostics;

namespace GameSharp.Core.Module
{
    public abstract class ModulePointerBase : IModulePointer
    {
        public ProcessModule NativeProcessModule { get; }
        public string Name { get; }
        public IntPtr BaseAddress { get; }
        public int ModuleMemorySize { get; }
        public IntPtr Handle => Kernel32.GetModuleHandle(Name);

        public ModulePointerBase(ProcessModule module)
        {
            NativeProcessModule = module;
            Name = NativeProcessModule.ModuleName.ToLower();
            BaseAddress = NativeProcessModule.BaseAddress;
            ModuleMemorySize = NativeProcessModule.ModuleMemorySize;
        }

        public override string ToString()
        {
            return $"{Name} 0x{BaseAddress.ToString("X")}";
        }

        public abstract IMemoryPointer GetProcAddress(string name);
        public abstract IMemoryPointer MemoryAddress { get; }
    }
}
