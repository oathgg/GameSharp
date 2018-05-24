using CsInjection.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CsInjection.Core.Native
{
    public static class Kernel32
    {
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int AllocConsole();

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
