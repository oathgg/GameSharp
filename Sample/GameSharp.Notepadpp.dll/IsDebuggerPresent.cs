using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Internal;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Notepadpp
{
    public class IsDebuggerPresent : SafeFunction
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool IsDebuggerPresentDelegate();

        protected override Delegate ToCallDelegate()
        {
            GameSharpProcess process = GameSharpProcess.Instance;
            IMemoryModule kernel32 = process.Modules["kernel32.dll"];
            IMemoryAddress IsDebuggerPresentPtr = kernel32.GetProcAddress("IsDebuggerPresent");
            return IsDebuggerPresentPtr.ToDelegate<IsDebuggerPresentDelegate>();
        }
    }
}
