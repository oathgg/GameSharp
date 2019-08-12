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

            SafeCallMessageBoxW safeMessageBoxFunction = new SafeCallMessageBoxW();
            InternalProcess.Instance.CallFunction<int>(safeMessageBoxFunction, IntPtr.Zero, "This is a sample of how to Call a function", "Title of the Messagebox", (uint)0);

            HookMessageBoxW messageBoxHook = new HookMessageBoxW();
            LoggingService.Info("Hooked messagebox!");
            messageBoxHook.Enable();

            //Native.Structs.ThreadContext32 context = InternalProcess.Instance.GetThreadContext();
        }
    }
}
