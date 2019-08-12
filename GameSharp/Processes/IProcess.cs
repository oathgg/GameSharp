using GameSharp.Module;

namespace GameSharp.Processes
{
    public interface IProcess
    {
        InternalModule LoadLibrary(string libraryPath, bool resolveReferences = true);
        InternalModule GetModule(string moduleName);
    }
}
