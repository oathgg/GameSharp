using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Core.Services;
using GameSharp.Internal;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Notepadpp.Hooks
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

            int result = CallOriginal<int>(hWnd, text, caption, type);

            return result;
        }

        public override Delegate GetDetourDelegate()
        {
            return new HookMessageBoxWDelegate(DetourMethod);
        }

        public override Delegate GetHookDelegate()
        {
            GameSharpProcess process = GameSharpProcess.Instance;

            IMemoryModule user32dll = process.Modules["user32.dll"];

            IMemoryAddress messageBoxWPtr = user32dll.GetProcAddress("MessageBoxW");

            return messageBoxWPtr.ToDelegate<HookMessageBoxWDelegate>();
        }
    }
}
