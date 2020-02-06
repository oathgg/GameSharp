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
            ModulePointer ntdll = process.Modules["ntdll.dll"];
            IMemoryPointer ntQueryInformationProcessPtr = ntdll.GetProcAddress("NtQueryInformationProcess");
            return ntQueryInformationProcessPtr.ToDelegate<NtQueryInformationProcessDelegate>();
        }

        public uint Call(IntPtr handle, ProcessInformationClass pic, out IMemoryPointer result, int resultLength, out IMemoryPointer bytesRead)
        {
            IMemoryPointer bytesReadInternal = GameSharpProcess.Instance.AllocateManagedMemory(resultLength);
            IMemoryPointer resultInternal = GameSharpProcess.Instance.AllocateManagedMemory(resultLength);

            uint retval = Call<uint>(handle, pic, resultInternal.Address, (uint)resultLength, bytesReadInternal.Address);

            bytesRead = bytesReadInternal;
            result = resultInternal;

            return retval;
        }
    }
}
