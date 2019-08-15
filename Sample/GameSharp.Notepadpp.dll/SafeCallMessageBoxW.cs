using GameSharp.Extensions;
using GameSharp.Interoperability;
using GameSharp.Module;
using GameSharp.Processes;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace GameSharp.Notepadpp.dll
{
    public class SafeCallMessageBoxW : SafeFunction
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int MessageBoxWDelegate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]string text, [MarshalAs(UnmanagedType.LPWStr)]string caption, uint type);

        public override Delegate ToCallDelegate()
        {
            InternalModule module = InternalProcess.Instance.Modules.FirstOrDefault(x => x.Name == "user32.dll");

            return (module.ProcessModule.BaseAddress + 0x78290).ToDelegate<MessageBoxWDelegate>();
        }
    }
}
