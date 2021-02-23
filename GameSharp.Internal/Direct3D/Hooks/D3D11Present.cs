using GameSharp.Core.Memory;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Internal.Direct3D.Hooks
{
    public abstract class D3D11Present : Hook
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        private delegate int delegateObject(IntPtr swapChainPtr, int syncInterval, int flags);

        private int DetourMethod(IntPtr swapChainPtr, int syncInterval, int flags)
        {
            DoWork(swapChainPtr, syncInterval, flags);
            return CallOriginal<int>(swapChainPtr, syncInterval, flags);
        }

        public override Delegate GetDetourDelegate()
        {
            return new delegateObject(DetourMethod);
        }

        public override Delegate GetHookDelegate()
        {
            MemoryPointer ptr = D3DHelper.GetD3D11Endscene();
            return ptr.ToDelegate<delegateObject>();
        }

        public abstract void DoWork(IntPtr swapChainPtr, int syncInterval, int flags);
    }
}
