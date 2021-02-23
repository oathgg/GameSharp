using GameSharp.Core.Memory;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Internal.Direct3D.Hooks
{
    public abstract class D3D9Present : Hook
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        private delegate int D3D9PresentDelegate(IntPtr device);

        private int DetourMethod(IntPtr device)
        {
            DoWork(device);
            return CallOriginal<int>(device);
        }

        public override Delegate GetDetourDelegate()
        {
            return new D3D9PresentDelegate(DetourMethod);
        }

        public override Delegate GetHookDelegate()
        {
            MemoryPointer ptr = D3DHelper.GetD3D9Present();
            return ptr.ToDelegate<D3D9PresentDelegate>();
        }

        public abstract void DoWork(IntPtr device);
    }
}
