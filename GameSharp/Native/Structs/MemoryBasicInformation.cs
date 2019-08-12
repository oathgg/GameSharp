using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Native.Structs
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
