using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Internal;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Notepadpp
{
    public class SafeCallMessageBoxW : SafeFunction
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        private delegate int MessageBoxWDelegate(IntPtr hWnd, string text, string caption, uint type);

        protected override Delegate ToCallDelegate()
        {
            GameSharpProcess process = GameSharpProcess.Instance;

            IMemoryModule user32dll = process.Modules["user32.dll"];

            IMemoryAddress messageBoxWPtr = user32dll.GetProcAddress("MessageBoxW");

            return messageBoxWPtr.ToDelegate<MessageBoxWDelegate>();
        }
    }
}
