using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Internal;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Notepadpp.FunctionWrapper
{
    public class MessageBoxW : SafeFunction
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        private delegate int MessageBoxWDelegate(IntPtr hWnd, string text, string caption, uint type);

        protected override Delegate InitializeDelegate()
        {
            GameSharpProcess process = GameSharpProcess.Instance;

            ModulePointer user32dll = process.Modules["user32.dll"];

            IMemoryPointer messageBoxWPtr = user32dll.GetProcAddress("MessageBoxW");

            return messageBoxWPtr.ToDelegate<MessageBoxWDelegate>();
        }

        public int Call(IntPtr hWnd, string text, string caption, uint type)
        {
            return BaseCall<int>(hWnd, text, caption, type);
        }
    }
}
