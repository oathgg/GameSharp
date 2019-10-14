using GameSharp.Core.Memory;
using System;
using System.Diagnostics;

namespace GameSharp.Core.Module
{
    public interface IModulePointer
    {
        ProcessModule NativeProcessModule { get; }

        string Name { get; }

        IntPtr BaseAddress { get; }

        int ModuleMemorySize { get; }

        IMemoryPointer MemoryAddress { get; }

        IMemoryPointer GetProcAddress(string name);

        IntPtr Handle { get; }
    }
}
