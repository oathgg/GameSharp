using GameSharp.Memory;
using System.Diagnostics;

namespace GameSharp.Processes
{
    public interface IProcess
    {
        IModule LoadLibrary(string libraryPath, bool resolveReferences = true);
        IModule GetModule(string moduleName);
    }
}
