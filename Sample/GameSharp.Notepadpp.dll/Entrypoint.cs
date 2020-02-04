using GameSharp.Core.Services;
using GameSharp.Internal;
using GameSharp.Notepadpp.Hooks;
using RGiesecke.DllExport;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace GameSharp.Notepadpp
{
    public class Entrypoint
    {
        // Prevents it from being cleaned by the GC
        static readonly HookMessageBoxW messageBoxHook = new HookMessageBoxW();

        [DllExport]
        public static void Main()
        {
            LoggingService.Info("I have been injected!");

            LoggingService.Info("Calling MessageBoxW!");
            if (Functions.MessageBoxW.Call(IntPtr.Zero, "Through a SafeFunctionCall method", "Caption", 0) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            LoggingService.Info("Enabling hook on MessageBoxW!");
            messageBoxHook.Enable();
        }
    }
}
