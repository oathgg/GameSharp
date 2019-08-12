using GameSharp.Extensions;
using GameSharp.Interoperability;
using GameSharp.Module;
using GameSharp.Processes;
using GameSharp.Services;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Notepadpp.dll
{
    public class HookMessageBoxW : Hook
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        private delegate int HookMessageBoxWDelegate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]string text, [MarshalAs(UnmanagedType.LPWStr)]string caption, uint type);

        private int DetourMethod(IntPtr hWnd, string text, string caption, uint type)
        {
            LoggingService.Info("MessageBoxW called");

            LoggingService.Info(caption = "Changed messagebox title");
            LoggingService.Info(text);

            int result = this.CallOriginal<int>(hWnd, text, caption, type);

            return result;
        }

        public override Delegate GetDetourDelegate()
        {
            return new HookMessageBoxWDelegate(DetourMethod);
        }

        public override Delegate GetHookDelegate()
        {
            InternalModule module = InternalProcess.Instance.GetModule("USER32.DLL");

            return (module.ProcessModule.BaseAddress + 0x78290).ToDelegate<HookMessageBoxWDelegate>();
        }
    }
}
