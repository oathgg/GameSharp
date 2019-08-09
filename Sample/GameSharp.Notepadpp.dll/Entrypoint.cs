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

            HookMessageBoxW messageBoxHook = new HookMessageBoxW();
            messageBoxHook.Enable();

            SafeCallMessageBoxW safeMessageBoxFunction = new SafeCallMessageBoxW();
            InternalProcess.GetInstance.CallFunction<int>(safeMessageBoxFunction, IntPtr.Zero, "This is a sample of how to Call a function", "Title of the Messagebox", (uint)0);
        }
    }
}
