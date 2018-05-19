using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsInjection.Core.Hooks
{
    public interface IHook
    {
        void InstallHook();
        string GetName();
        Delegate GetDelegate();
        IntPtr GetModuleAddress();
        int GetAddress();
    }
}
