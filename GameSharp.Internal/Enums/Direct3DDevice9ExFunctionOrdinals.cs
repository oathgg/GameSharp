using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Internal.Enums
{
    public enum Direct3DDevice9ExFunctionOrdinals : short
    {
        SetConvolutionMonoKernel = 119,
        ComposeRects = 120,
        PresentEx = 121,
        GetGPUThreadPriority = 122,
        SetGPUThreadPriority = 123,
        WaitForVBlank = 124,
        CheckResourceResidency = 125,
        SetMaximumFrameLatency = 126,
        GetMaximumFrameLatency = 127,
        CheckDeviceState_ = 128,
        CreateRenderTargetEx = 129,
        CreateOffscreenPlainSurfaceEx = 130,
        CreateDepthStencilSurfaceEx = 131,
        ResetEx = 132,
        GetDisplayModeEx = 133,
    }
}
