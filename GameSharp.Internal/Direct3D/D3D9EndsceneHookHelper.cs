using GameSharp.Core.Memory;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Internal.Direct3D
{
    public abstract class D3D9EndsceneHookHelper : Hook
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        private delegate int EndSceneDelegate(IntPtr device);

        private int DetourMethod(IntPtr device)
        {
            DoWork(device);
            return CallOriginal<int>(device);
        }

        public override Delegate GetDetourDelegate()
        {
            return new EndSceneDelegate(DetourMethod);
        }

        public override Delegate GetHookDelegate()
        {
            IMemoryPointer ptr = Functions.D3D9Helper.GetD3D9Endscene();
            return ptr.ToDelegate<EndSceneDelegate>();
        }

        public abstract void DoWork(IntPtr device);
    }
}
