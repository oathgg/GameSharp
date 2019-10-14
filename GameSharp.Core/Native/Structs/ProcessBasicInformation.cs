using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public int Size
        {
            get { return Marshal.SizeOf(typeof(ProcessBasicInformation)); }
        }
    }
}
