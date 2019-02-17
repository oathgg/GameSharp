using CsInjection.Core.Utilities;
using RGiesecke.DllExport;

namespace WoW.Injectable
{
    public class Entrypoint
    {
        [DllExport]
        public static void Main()
        {
            ExceptionHandler.Initialize();
            Logger.Info("Injected");
        }
    }
}
