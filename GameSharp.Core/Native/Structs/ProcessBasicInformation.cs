using System;
using System.Runtime.InteropServices;

namespace GameSharp.Core.Native.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ProcessBasicInformation
    {
        public IntPtr ExitStatus;
        public IntPtr PebBaseAddress;
        public IntPtr AffinityMask;
        public IntPtr BasePriority;
        public UIntPtr UniqueProcessId;
        public IntPtr InheritedFromUniqueProcessId;

        public int Size => Marshal.SizeOf(typeof(ProcessBasicInformation));
    }
}
