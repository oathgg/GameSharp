using GameSharp.Internal.Direct3D;
using GameSharp.Internal.Memory;
using System;

namespace GameSharp.Internal.Direct3D
{
    public static class D3DHelper
    {
        public static InternalMemoryPointer GetD3D9Endscene()
        {
            D3DDevice Device = new D3D9Device(GameSharpProcess.Instance.Native);
            IntPtr address = Device.GetDeviceVTableFuncAbsoluteAddress(Device.EndSceneVtableIndex);
            return new InternalMemoryPointer(address);
        }

        public static InternalMemoryPointer GetD3D9Present()
        {
            D3DDevice Device = new D3D9Device(GameSharpProcess.Instance.Native);
            IntPtr address = Device.GetDeviceVTableFuncAbsoluteAddress(Device.PresentVtableIndex);
            return new InternalMemoryPointer(address);
        }

        public static InternalMemoryPointer GetD3D11Endscene()
        {
            D3DDevice Device = new D3D11Device(GameSharpProcess.Instance.Native);
            IntPtr address = Device.GetDeviceVTableFuncAbsoluteAddress(Device.EndSceneVtableIndex);
            return new InternalMemoryPointer(address);
        }
    }
}
