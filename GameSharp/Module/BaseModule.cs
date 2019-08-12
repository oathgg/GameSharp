using System.Diagnostics;

namespace GameSharp.Module
{
    public class BaseModule
    {
        public readonly ProcessModule ProcessModule;

        public BaseModule(ProcessModule module)
        {
            ProcessModule = module;
        }
    }
}
