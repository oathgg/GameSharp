using CsInjection.Core.Utilities;
using RGiesecke.DllExport;

namespace ScyllaTest_x86.Injectable
{
    public class Entrypoint
    {
        [DllExport]
        public static void Main()
        {
            ExceptionHandler.Initialize();
            Logger.Write("Injected");
        }
    }
}
