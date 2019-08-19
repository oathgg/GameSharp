using GameSharp.Core.Services;
using GameSharp.Internal;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using GameSharp.Internal.Module;
using System;
using System.Linq;
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
            GameSharpProcess process = GameSharpProcess.Instance;

            process.RefreshModules();

            MemoryModule module = process.Modules.FirstOrDefault(x => x.Name == "user32.dll") as MemoryModule;

            return (module.ProcessModule.BaseAddress + 0x807B0).ToDelegate<HookMessageBoxWDelegate>();
        }
    }
}
