using CsInjection.Core.Helpers;
using System;
using System.Runtime.InteropServices;

namespace CsInjection.Core.Native
{
    public class Memory
    {
        public static void WriteProcessMemory(IntPtr memoryAddress, byte[] newBytes)
        {
            for (int i = 0; i < newBytes.Length; i++)
            {
                Marshal.WriteByte(memoryAddress, i, newBytes[i]);
            }
        }

        public static T ReadProcessMemory<T>(IntPtr memoryAddress, int size)
        {
            byte[] data = new byte[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = Marshal.ReadByte(memoryAddress, i);
            }

            if (!ConvertHelper.FromByteArray(data, out T result))
            {
                // Last resort to resolve the object
                result = Marshal.PtrToStructure<T>(memoryAddress);
            }
            return result;
        }

        // Slower tha
        public static T ReadProcessMemory<T>(IntPtr memoryAddress) where T : struct
        {
            return Marshal.PtrToStructure<T>(memoryAddress);
        }
    }
}
