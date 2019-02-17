using CsInjection.Utilities;
using System;

namespace CsInjection.Extensions
{
    public static class IntPtrExtension
    {
        public static T Read<T>(this IntPtr addr, int size, int offset = 0)
        {
            return Memory.Read<T>(addr + offset, size);
        }

        public static void Write(this IntPtr addr, byte[] newBytes)
        {
            Memory.Write(addr, newBytes);
        }
    }
}
