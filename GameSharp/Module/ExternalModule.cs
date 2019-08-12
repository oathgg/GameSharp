using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Module
{
    public class ExternalModule : IModule
    {
        public IntPtr GetProcAddress(string entryPoint)
        {
            throw new NotImplementedException();
        }
    }
}
