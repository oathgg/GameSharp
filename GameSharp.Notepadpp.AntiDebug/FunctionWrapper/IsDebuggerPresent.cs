using GameSharp.Core;
using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Internal;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Notepadpp.FunctionWrapper
{
    public class IsDebuggerPresent : SafeFunction
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool IsDebuggerPresentDelegate();

        protected override Delegate InitializeDelegate()
        {
            IProcess process = GameSharpProcess.Instance;
            ModulePointer kernel32 = process.Modules["kernel32.dll"];
            IMemoryPointer IsDebuggerPresentPtr = kernel32.GetProcAddress("IsDebuggerPresent");
            return IsDebuggerPresentPtr.ToDelegate<IsDebuggerPresentDelegate>();
        }

        public bool Call()
        {
            return Call<bool>(null);
        }
    }
}
