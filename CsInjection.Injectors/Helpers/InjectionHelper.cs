using CsInjection.Core.Utilities;
using CsInjection.Injection.Native;

namespace CsInjection.Injection.Helpers
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