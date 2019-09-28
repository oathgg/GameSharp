using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Internal;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Notepadpp.FunctionWrapper
{
    public class NtQueryInformationProcess : SafeFunction
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int NtQueryInformationProcessDelegate(IntPtr processHandle, int processInformationClass, [Out] IntPtr processInformation, uint processInformationLength, IntPtr returnLength);

        protected override Delegate ToCallDelegate()
        {
            GameSharpProcess process = GameSharpProcess.Instance;

            IMemoryModule ntdll = process.Modules["ntdll.dll"];

            IMemoryAddress ntQueryInformationProcessPtr = ntdll.GetProcAddress("NtQueryInformationProcess");

            return ntQueryInformationProcessPtr.ToDelegate<NtQueryInformationProcessDelegate>();
        }
    }
}
