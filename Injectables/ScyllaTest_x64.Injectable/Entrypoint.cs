using CsInjection.Core.Utilities;
using RGiesecke.DllExport;

namespace DllTest
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
