using GameSharp.Extensions;
using GameSharp.Memory;
using GameSharp.Memory.Internal;
using GameSharp.Processes;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameSharp.Notepadpp.dll
{
    public class SafeCallMessageBoxW : SafeFunction
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int MessageBoxWDelegate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]string text, [MarshalAs(UnmanagedType.LPWStr)]string caption, uint type);

        public override Delegate ToCallDelegate()
        {
            ProcessModule module = InternalProcess.Instance.GetModule("USER32.DLL");

            return (module.BaseAddress + 0x78290).ToDelegate<MessageBoxWDelegate>();
        }
    }
}
