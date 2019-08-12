using GameSharp.Module;

namespace GameSharp.Processes
{
    public interface IProcess
    {
        ModuleBase LoadLibrary(string libraryPath, bool resolveReferences = true);
        ModuleBase GetModule(string moduleName);
    }
}
