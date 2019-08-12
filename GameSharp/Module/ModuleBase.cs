using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Module
{
    public abstract class ModuleBase : IModule
    {
        public readonly ProcessModule ProcessModule;

        public ModuleBase(ProcessModule processModule)
        {
            ProcessModule = processModule;
        }

        public abstract IntPtr GetProcAddress(string entryPoint);
    }
}
