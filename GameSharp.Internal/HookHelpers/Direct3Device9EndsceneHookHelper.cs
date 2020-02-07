using GameSharp.Internal.Enums;
using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Internal;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Internal.HookHelpers
{
    public abstract class Direct3Device9EndsceneHookHelper : Hook
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int EndSceneDelegate(IntPtr device);

        private int DetourMethod(IntPtr device)
        {
            return CallOriginal<int>(device);
        }

        public override Delegate GetDetourDelegate()
        {
            return new EndSceneDelegate(DetourMethod);
        }

        public override Delegate GetHookDelegate()
        {
            IMemoryPointer ptr = Functions.Direct3DDevice9.GetAddress(Direct3DDevice9FunctionOrdinals.EndScene);
            return ptr.ToDelegate<EndSceneDelegate>();
        }

        public abstract void DoWork(IntPtr device);
    }
}
