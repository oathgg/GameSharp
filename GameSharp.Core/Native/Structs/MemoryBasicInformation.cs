using GameSharp.Core.Native.Enums;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Core.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MemoryBasicInformation
    {
        public readonly IntPtr BaseAddress;
        public readonly IntPtr AllocationBase;
        public readonly MemoryProtection AllocationProtect;
        public readonly IntPtr RegionSize;
        public readonly uint State;
        public readonly uint Protect;
        public readonly uint Type;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryBasicInformation64
    {
        public ulong BaseAddress;
        public ulong AllocationBase;
        public MemoryProtection AllocationProtect;
        public int __alignment1;
        public ulong RegionSize;
        public int State;
        public int Protect;
        public int Type;
        public int __alignment2;
    }
}
