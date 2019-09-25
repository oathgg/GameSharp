using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameSharp.Core
{
    public interface IProcess
    {
        Process NativeProcess { get; }
        ProcessModule MainModule { get; }
        IntPtr Handle { get; }
        Dictionary<string, IMemoryModule> Modules { get; }
        IMemoryModule LoadLibrary(string pathToDll, bool resolveReferences = true);
        void RefreshModules();
    }
}
