using CsInjection.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CsInjection.Tests
{
    public static class Utils
    {
        public static IntPtr GenerateRandomMemoryBlock()
        {
            IntPtr ptrAllocatedMemory = Marshal.AllocHGlobal(1024);
            byte[] rndBytes = new byte[1024];
            new Random().NextBytes(rndBytes);
            Memory.Write(ptrAllocatedMemory, rndBytes);

            return ptrAllocatedMemory;
        }
    }
}
