using GameSharp.Core.Memory;
using System;
using System.Diagnostics;

namespace GameSharp.Core.Module
{
    public interface IMemoryModule
    {
        ProcessModule ProcessModule { get; }

        string Name { get; }

        IntPtr BaseAddress { get; }

        int Size { get; }

        IMemoryAddress MemoryAddress { get; }
    }
}
