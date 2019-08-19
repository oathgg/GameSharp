using GameSharp.Core.Module;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameSharp.Core
{
    public interface IProcess
    {
        Process Process { get; }
        List<IMemoryModule> Modules { get; set; }
        IMemoryModule LoadLibrary(string pathToDll);
        IMemoryModule LoadLibrary(string pathToDll, bool resolveReferences);
        void RefreshModules();
    }
}
