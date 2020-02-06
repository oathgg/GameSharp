using GameSharp.Core.Services;
using RGiesecke.DllExport;

namespace GameSharp.Notepadpp.AntiDebug
{
    public class Entrypoint
    {
        [DllExport]
        public static void Main()
        {
            LoggingService.Info("I have been injected!");

            AntiDebugChecks.CheckForDebugger();
        }
    }
}
