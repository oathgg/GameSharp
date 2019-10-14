using GameSharp.Core.Memory;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.PInvoke;
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
        private static readonly GameSharpProcess Process = GameSharpProcess.Instance;

        [DllExport]
        public static void Main()
        {
            LoggingService.Info("I have been injected!");

            LoggingService.Info("Calling MessageBoxW!");
            if (Functions.MessageBoxW.Call(IntPtr.Zero, "Through a SafeFunctionCall method", "Caption", (uint)0) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            LoggingService.Info("Enabling hook on MessageBoxW!");
            HookMessageBoxW messageBoxHook = new HookMessageBoxW();
            messageBoxHook.Enable();

            AntiDebugChecks.CheckForDebugger();
        }
    }
}
