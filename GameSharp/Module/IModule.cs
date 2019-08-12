using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Module
{
    public interface IModule
    {
        IntPtr GetProcAddress(string entryPoint);
    }
}
