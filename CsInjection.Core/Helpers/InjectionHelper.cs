using CsInjection.Core.Native;
using CsInjection.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsInjection.Core.Helpers
{
    public abstract class InjectionHelper
    {
        public static void Initialize()
        {
            Kernel32.AllocConsole();
            ExceptionHandler.Initialize();
        }
    }
}
