using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Core.Native.Enums;
using GameSharp.Internal;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Notepadpp.FunctionWrapper
{
    public class InjectedNtQueryInformationProcess : SafeFunction
    {
        static readonly IMemoryAddress Allocation = GameSharpProcess.Instance.AllocateManagedMemory(100);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint InjectedNtQueryInformationProcessDelegate(IntPtr processHandle, int processInformationClass, [Out] IntPtr processInformation, 
            uint processInformationLength, [Out] IntPtr returnLength);

        protected override Delegate InitializeDelegate()
        {
            Allocation.Write(new byte[]
            {
                0x4C, 0x8B, 0xD1, 0xB8, 0x19, 0x00, 0x00, 0x00, 0xF6, 0x04, 0x25, 0x08, 0x03, 0xFE, 0x7F, 0x01, 0x75, 0x03, 0x0F, 0x05, 0xC3, 0xCD, 0x2E, 0xC3
            });

            return Allocation.ToDelegate<InjectedNtQueryInformationProcessDelegate>();
        }

        public uint Call(IntPtr handle, ProcessInformationClass pic, out IMemoryAddress result, int resultLength, out IMemoryAddress bytesRead)
        {
            IMemoryAddress bytesReadInternal = GameSharpProcess.Instance.AllocateManagedMemory(resultLength);
            IMemoryAddress resultInternal = GameSharpProcess.Instance.AllocateManagedMemory(resultLength);

            uint retval = this.BaseCall<uint>(handle, pic, resultInternal.Address, (uint)resultLength, bytesReadInternal.Address);

            bytesRead = bytesReadInternal;
            result = resultInternal;

            return retval;
        }
    }
}
