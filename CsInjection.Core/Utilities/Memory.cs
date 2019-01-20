using CsInjection.Core.Helpers;
using System;
using System.Runtime.InteropServices;

namespace CsInjection.Core.Utilities
{
    public class Memory
    {
        public static void Write(IntPtr destination, byte[] nBytes)
        {
            throw new Exception("Refactor code");

            //// Update the memory section so we can write to it.
            //Kernel32.VirtualProtect(destination, nBytes.Length, Enums.Protection.PAGE_EXECUTE_READWRITE, out Enums.Protection old);

            //// Write to buffer to the memory destination.
            //Marshal.Copy(nBytes, 0, destination, nBytes.Length);

            //// Restore the page execution permissions.
            //Kernel32.VirtualProtect(destination, nBytes.Length, old, out Enums.Protection x);
        }

        public static T Read<T>(IntPtr memoryAddress) where T : struct
        {
            return Marshal.PtrToStructure<T>(memoryAddress);
        }

        public static T Read<T>(IntPtr memoryAddress, int size)
        {
            return Read<T>(memoryAddress, 0, size);
        }

        public static T Read<T>(IntPtr memoryAddress, int offset, int size)
        {
            byte[] destination = new byte[size];

            // Copy the memory to our own object
            Marshal.Copy(memoryAddress, destination, offset, destination.Length);

            return ConvertHelper.FromByteArray<T>(destination);
        }
    }
}