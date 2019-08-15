using GameSharp.Processes;
using GameSharp.Services;
using RGiesecke.DllExport;
using System;

namespace GameSharp.Notepadpp.dll
{
    public class Entrypoint
    {
        [DllExport]
        public static void Main()
        {
            LoggingService.Info("I have been injected!");

            LoggingService.Info("Initializing changes we wish to make...");
            SafeCallMessageBoxW safeMessageBoxFunction = new SafeCallMessageBoxW();
            HookMessageBoxW messageBoxHook = new HookMessageBoxW();

            InternalProcess.Instance.CallFunction<int>(safeMessageBoxFunction, IntPtr.Zero, "This is a sample of how to Call a function", "Title of the Messagebox", (uint)0);
            LoggingService.Info("Hooked messagebox!");
            messageBoxHook.Enable();

            //Native.Structs.ThreadContext32 context = InternalProcess.Instance.GetThreadContext();
        }
    }
}
