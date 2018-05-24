using CsInjection.Core.Helpers;
using System;

namespace CsInjection.Core.Native
{
    public class Memory
    {
        public static void WriteProcessMemory(IntPtr memoryAddress, byte[] newBytes)
        {
            for (int i = 0; i < newBytes.Length; i++)
            {
                System.Runtime.InteropServices.Marshal.WriteByte(memoryAddress, i, newBytes[i]);
            }
        }

        public static T ReadProcessMemory<T>(IntPtr memoryAddress, int size)
        {
            byte[] data = new byte[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = System.Runtime.InteropServices.Marshal.ReadByte(memoryAddress, i);
            }

            if (!ConvertHelper.FromByteArray<T>(data, out T result))
            {
                // Last resort to resolve the object
                result = System.Runtime.InteropServices.Marshal.PtrToStructure<T>(memoryAddress);
            }
            return result;
        }
    }
}
