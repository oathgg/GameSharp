using GameSharp.Internal;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using GameSharp.Internal.Module;
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
            GameSharpProcess process = GameSharpProcess.Instance;

            process.RefreshModules();

            MemoryModule module = process.Modules["user32.dll"] as MemoryModule;

            return (module.BaseAddress + 0x807B0).ToDelegate<MessageBoxWDelegate>();
        }
    }
}
