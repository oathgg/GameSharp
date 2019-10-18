using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameSharp.Core
{
    public interface IProcess
    {
        bool Is64Bit { get; }
        Process Native { get; }
        ProcessModule MainModule { get; }
        IntPtr Handle { get; }
        Dictionary<string, IModulePointer> Modules { get; }
        IModulePointer LoadLibrary(string pathToDll, bool resolveReferences = true);
        IMemoryPointer AllocateManagedMemory(int size);
        Dictionary<string, IModulePointer> RefreshModules();
        IMemoryPointer GetPebAddress();
        MemoryPeb MemoryPeb { get; }
    }
}
