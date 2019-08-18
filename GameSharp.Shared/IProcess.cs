using GameSharp.Core.Module;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameSharp.Core
{
    public interface IProcess
    {
        Process Process { get; }

        List<IModule> Modules { get; set; }

        IModule LoadLibrary(string pathToDll);

        IModule LoadLibrary(string pathToDll, bool resolveReferences);

        void RefreshModules();
    }
}
