using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Internal;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using GameSharp.Internal.Module;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace GameSharp.Notepadpp
{
    public class SafeCallMessageBoxW : SafeFunction
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int MessageBoxWDelegate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]string text, [MarshalAs(UnmanagedType.LPWStr)]string caption, int type);

        protected override Delegate ToCallDelegate()
        {
            GameSharpProcess process = GameSharpProcess.Instance;

            IMemoryModule user32dll = process.Modules["user32.dll"];

            IMemoryAddress messageBoxWPtr = user32dll.GetProcAddress("MessageBoxW");

            return messageBoxWPtr.ToDelegate<MessageBoxWDelegate>();
        }
    }
}
