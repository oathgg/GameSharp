using System.Diagnostics;

namespace GameSharp.Processes
{
    public interface IProcess
    {
        ProcessModule LoadLibrary(string libraryPath, bool resolveReferences = true);
        ProcessModule GetProcessModule(string moduleName);
    }
}
