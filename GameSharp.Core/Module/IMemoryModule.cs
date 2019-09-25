using GameSharp.Core.Memory;
using System;
using System.Diagnostics;

namespace GameSharp.Core.Module
{
    public interface IMemoryModule
    {
        ProcessModule NativeProcessModule { get; }

        string Name { get; }

        IntPtr BaseAddress { get; }

        int ModuleMemorySize { get; }

        IMemoryAddress MemoryAddress { get; }

        IMemoryAddress GetProcAddress(string name);

        IntPtr Handle { get; }
    }
}
