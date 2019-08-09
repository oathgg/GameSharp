using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Memory
{
    public interface IModule
    {
        IntPtr BaseAddress();
        IntPtr GetProcAddress(string name);
    }
}
