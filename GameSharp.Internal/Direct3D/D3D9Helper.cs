using GameSharp.Internal.Direct3D;
using GameSharp.Internal.Memory;
using System;

namespace GameSharp.Internal.Functions
{
    public static class D3D9Helper
    {
        public static MemoryPointer GetD3D9Endscene()
        {
            D3DDevice Device = new D3D9Device(GameSharpProcess.Instance.Native);
            IntPtr endsceneAddress = Device.GetDeviceVTableFuncAbsoluteAddress(Device.EndSceneVtableIndex);
            return new MemoryPointer(endsceneAddress);
        }
    }
}
