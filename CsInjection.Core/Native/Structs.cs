using System;
using System.Runtime.InteropServices;

namespace CsInjection.Core.Native
{
    public class Structs
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct MemoryBasicInformation
        {
            private readonly IntPtr BaseAddress;

            private readonly IntPtr AllocationBase;
            private readonly uint AllocationProtect;

            internal readonly IntPtr RegionSize;

            private readonly uint State;
            private readonly uint Protect;
            private readonly uint Type;
        }
    }
}
