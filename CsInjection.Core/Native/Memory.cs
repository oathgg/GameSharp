using CsInjection.Core.Helpers;
using System;
using System.Runtime.InteropServices;

namespace CsInjection.Core.Native
{
    public class Memory
    {
        public static void WriteProcessMemory(IntPtr destination, byte[] nBytes)
        {
            // Update the memory section so we can write to it.
            Kernel32.VirtualProtect(destination, nBytes.Length, Kernel32.Protection.PAGE_EXECUTE_READWRITE, out Kernel32.Protection old);

            // Write to buffer to the memory destination.
            Marshal.Copy(nBytes, 0, destination, nBytes.Length);

            // Restore the page execution permissions.
            Kernel32.VirtualProtect(destination, nBytes.Length, old, out var x);
        }

        public static T ReadProcessMemory<T>(IntPtr memoryAddress, int size)
        {
            byte[] destination = new byte[size];

            // Copy the memory to our own object
            Marshal.Copy(memoryAddress, destination, 0, destination.Length);

            return ConvertHelper.FromByteArray<T>(destination);
        }

        public static T ReadProcessMemory<T>(IntPtr memoryAddress) where T : struct
        {
            return Marshal.PtrToStructure<T>(memoryAddress);
        }
    }
}
