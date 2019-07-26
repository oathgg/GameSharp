using GameSharp.Extensions;
using GameSharp.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Notepadpp.dll
{
    public class HookMessageBoxW : Hooks.Hook
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        private delegate int HookMessageBoxWDelegate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]string text, [MarshalAs(UnmanagedType.LPWStr)]string caption, uint type);

        private int DetourMethod(IntPtr hWnd, string text, string caption, uint type)
        {
            Logger.Info("MessageBoxW called");

            Logger.Info(caption);
            Logger.Info(text);

            int result = this.CallOriginal<int>(hWnd, text, caption, type);

            return result;
        }

        public override Delegate GetDetourDelegate()
        {
            return new HookMessageBoxWDelegate(DetourMethod);
        }

        public override Delegate GetHookDelegate()
        {
            ProcessModule module = Process.GetCurrentProcess().GetProcessModule("USER32.DLL");

            return Marshal.GetDelegateForFunctionPointer<HookMessageBoxWDelegate>(module.BaseAddress + 0x78290);
        }
    }
}
