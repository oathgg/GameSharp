using GameSharp.Core.Memory;
using System;
using System.Diagnostics;

namespace GameSharp.Core.Module
{
    internal interface IModulePointer
    {
        ProcessModule ProcessModule { get; }

        string Name { get; }

        IntPtr Address { get; }

        int Size { get; }

        IMemoryPointer MemoryPointer { get; }

        IMemoryPointer GetProcAddress(string name);

        IntPtr Handle { get; }
    }
}
