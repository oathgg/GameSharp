using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Module
{
    public class ExternalModule : ModuleBase
    {
        public ExternalModule(ProcessModule processModule) : base(processModule)
        {
        }

        public override IntPtr GetProcAddress(string entryPoint)
        {
            throw new NotImplementedException();
        }
    }
}
