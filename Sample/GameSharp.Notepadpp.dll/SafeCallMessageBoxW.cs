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
    public class SafeCallMessageBoxW : SafeFunctionCaller
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int MessageBoxWDelegate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]string text, [MarshalAs(UnmanagedType.LPWStr)]string caption, uint type);

        public override Delegate ToCallDelegate()
        {
            ProcessModule module = Process.GetCurrentProcess().GetProcessModule("USER32.DLL");

            return Marshal.GetDelegateForFunctionPointer<MessageBoxWDelegate>(module.BaseAddress + 0x78290);
        }
    }
}
