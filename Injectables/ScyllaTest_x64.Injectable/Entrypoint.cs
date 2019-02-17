using CsInjection.Core.Utilities;
using RGiesecke.DllExport;

namespace ScyllaTest_x64.Injectable
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
