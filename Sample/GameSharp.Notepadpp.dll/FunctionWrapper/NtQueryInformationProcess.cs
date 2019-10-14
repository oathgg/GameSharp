using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Core.Native.Enums;
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
        private delegate uint NtQueryInformationProcessDelegate(IntPtr handle, ProcessInformationClass pic, [Out] IntPtr result, uint resultLength, [Out] IntPtr bytesRead);

        protected override Delegate InitializeDelegate()
        {
            GameSharpProcess process = GameSharpProcess.Instance;
            IMemoryModule ntdll = process.Modules["ntdll.dll"];
            IMemoryAddress ntQueryInformationProcessPtr = ntdll.GetProcAddress("NtQueryInformationProcess");
            return ntQueryInformationProcessPtr.ToDelegate<NtQueryInformationProcessDelegate>();
        }
    }
}
