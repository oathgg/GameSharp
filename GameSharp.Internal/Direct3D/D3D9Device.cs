using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

/// <summary>
/// CREDITS: https://github.com/lolp1/GetD3DHookOffset
/// </summary>
namespace GameSharp.Internal.Direct3D
{
    public sealed class D3D9Device : D3DDevice
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int CreateDeviceDelegate(
            IntPtr instance,
            uint adapter,
            uint deviceType,
            IntPtr focusWindow,
            uint behaviorFlags,
            [In] ref D3DPresentParameters presentationParameters,
            out IntPtr returnedDeviceInterface);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int Direct3D9EndScene(IntPtr device);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int Direct3D9Reset(IntPtr device, D3DPresentParameters presentationParameters);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int Direct3D9ResetEx(IntPtr presentationParameters, IntPtr displayModeEx);

        private const int D3D9SdkVersion = 0x20;
        private const int D3DCREATE_SOFTWARE_VERTEXPROCESSING = 0x20;
        private VTableFuncDelegate _d3DDeviceRelease;
        private VTableFuncDelegate _d3DRelease;
        private IntPtr _pD3D;

        public D3D9Device(Process targetProc) : base(targetProc, "d3d9.dll")
        {
        }

        public override int EndSceneVtableIndex => VTableIndexes.Direct3DDevice9EndScene;

        public override int PresentVtableIndex => VTableIndexes.Direct3DDevice9Present;

        protected override void InitD3D(out IntPtr d3DDevicePtr)
        {
            _pD3D = Direct3DCreate9(D3D9SdkVersion);

            if (_pD3D == IntPtr.Zero)
            {
                throw new Exception("Failed to create D3D.");
            }

            D3DPresentParameters parameters = new D3DPresentParameters
            {
                Windowed = true,
                SwapEffect = 1,
                BackBufferFormat = 0
            };

            IntPtr createDevicePtr = GetVTableFuncAddress(_pD3D, VTableIndexes.Direct3D9CreateDevice);

            CreateDeviceDelegate createDevice = Marshal.GetDelegateForFunctionPointer<CreateDeviceDelegate>(createDevicePtr);

            if (createDevice(_pD3D, 0, 1, Form.Handle, D3DCREATE_SOFTWARE_VERTEXPROCESSING, ref parameters,
                    out d3DDevicePtr) < 0)
            {
                throw new Exception("Failed to create device.");
            }

            IntPtr deviceReleasePtr = GetVTableFuncAddress(D3DDevicePtr, VTableIndexes.Direct3DDevice9Release);
            _d3DDeviceRelease = Marshal.GetDelegateForFunctionPointer<VTableFuncDelegate>(deviceReleasePtr);

            IntPtr releasePtr = GetVTableFuncAddress(_pD3D, VTableIndexes.Direct3D9Release);
            _d3DRelease = Marshal.GetDelegateForFunctionPointer<VTableFuncDelegate>(releasePtr);
        }

        protected override void CleanD3D()
        {
            if (D3DDevicePtr != IntPtr.Zero)
            {
                _d3DDeviceRelease(D3DDevicePtr);
            }

            if (_pD3D != IntPtr.Zero)
            {
                _d3DRelease(_pD3D);
            }
        }

        [DllImport("d3d9.dll")]
        public static extern IntPtr Direct3DCreate9(uint sdkVersion);

        [StructLayout(LayoutKind.Sequential)]
        public struct D3DPresentParameters
        {
            private readonly uint BackBufferWidth;
            private readonly uint BackBufferHeight;
            public uint BackBufferFormat;
            private readonly uint BackBufferCount;
            private readonly uint MultiSampleType;
            private readonly uint MultiSampleQuality;
            public uint SwapEffect;
            private readonly IntPtr hDeviceWindow;
            [MarshalAs(UnmanagedType.Bool)] public bool Windowed;
            [MarshalAs(UnmanagedType.Bool)] private readonly bool EnableAutoDepthStencil;
            private readonly uint AutoDepthStencilFormat;
            private readonly uint Flags;
            private readonly uint FullScreen_RefreshRateInHz;
            private readonly uint PresentationInterval;
        }

        public struct VTableIndexes
        {
            public const int Direct3D9Release = 2;
            public const int Direct3D9CreateDevice = 0x10;
            public const int Direct3DDevice9Release = 2;
            public const int Direct3DDevice9Present = 0x11;
            public const int Direct3DDevice9BeginScene = 0x29;
            public const int Direct3DDevice9EndScene = 0x2a;
        }
    }
}