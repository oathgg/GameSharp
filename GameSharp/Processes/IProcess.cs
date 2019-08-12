using GameSharp.Memory.Module;

namespace GameSharp.Processes
{
    public interface IProcess
    {
        Module LoadLibrary(string libraryPath, bool resolveReferences = true);
        Module GetModule(string moduleName);
    }
}
