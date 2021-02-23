using GameSharp.Core.Memory;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Internal.Direct3D.Hooks
{
    public abstract class D3D11Endscene : Hook
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        private delegate int D3D11EndSceneDelegate(IntPtr device);

        private int DetourMethod(IntPtr device)
        {
            DoWork(device);
            return CallOriginal<int>(device);
        }

        public override Delegate GetDetourDelegate()
        {
            return new D3D11EndSceneDelegate(DetourMethod);
        }

        public override Delegate GetHookDelegate()
        {
            MemoryPointer ptr = D3DHelper.GetD3D11Endscene();
            return ptr.ToDelegate<D3D11EndSceneDelegate>();
        }

        public abstract void DoWork(IntPtr device);
    }
}
