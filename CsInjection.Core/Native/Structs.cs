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

        [StructLayout(LayoutKind.Sequential)]
        public struct FloatingSaveArea
        {
            public uint ControlWord;
            public uint StatusWord;
            public uint TagWord;
            public uint ErrorOffset;
            public uint ErrorSelector;
            public uint DataOffset;
            public uint DataSelector;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            public byte[] RegisterArea;

            public uint Cr0NpxState;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Context
        {
            public uint ContextFlags; //set this to an appropriate value

            // Retrieved by CONTEXT_DEBUG_REGISTERS
            public uint Dr0;

            public uint Dr1;
            public uint Dr2;
            public uint Dr3;
            public uint Dr6;
            public uint Dr7;

            // Retrieved by CONTEXT_FLOATING_POINT
            public FloatingSaveArea FloatSave;

            // Retrieved by CONTEXT_SEGMENTS
            public uint SegGs;

            public uint SegFs;
            public uint SegEs;
            public uint SegDs;

            // Retrieved by CONTEXT_INTEGER
            public uint Edi;

            public uint Esi;
            public uint Ebx;
            public uint Edx;
            public uint Ecx;
            public uint Eax;

            // Retrieved by CONTEXT_CONTROL
            public uint Ebp;

            public uint Eip;
            public uint SegCs;
            public uint EFlags;
            public uint Esp;
            public uint SegSs;

            // Retrieved by CONTEXT_EXTENDED_REGISTERS
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] ExtendedRegisters;
        }
    }
}
