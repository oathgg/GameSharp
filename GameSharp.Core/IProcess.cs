using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameSharp.Core
{
    public interface IProcess
    {
        Process Native { get; }
        ProcessModule MainModule { get; }
        IntPtr Handle { get; }
        Dictionary<string, IModulePointer> Modules { get; }
        IModulePointer LoadLibrary(string pathToDll, bool resolveReferences = true);
        IMemoryPointer AllocateManagedMemory(int size);
        void RefreshModules();
        MemoryPeb GetPeb();
    }
}
