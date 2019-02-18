using GameSharp.Utilities;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Extensions
{
    public static class IntPtrExtension
    {
        public static T Read<T>(this IntPtr memoryAddress) where T : struct
        {
            return Marshal.PtrToStructure<T>(memoryAddress);
        }

        public static T Read<T>(this IntPtr addr, int size, int offset = 0)
        {
            byte[] destination = new byte[size];
            // Copy the memory to our own object
            Marshal.Copy(addr, destination, offset, destination.Length);
            return destination.Cast<T>();
        }

        public static void Write(this IntPtr addr, byte[] data)
        {
            Marshal.Copy(data, 0, addr, data.Length);
        }

        public static void Write<T>(this IntPtr addr, T data)
        {
            Marshal.StructureToPtr(data, addr, false);
        }
    }
}
