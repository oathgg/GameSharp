using CsInjection.Core.Native;
using CsInjection.Core.Utilities;

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