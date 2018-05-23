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

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        public static void WriteProcessMemory(IntPtr memoryAddress, byte[] newBytes)
        {
            WriteProcessMemory(ProcessHelper.GetCurrentProcess.Handle, memoryAddress, newBytes, newBytes.Length, out IntPtr outVar);
        }

        public static T ReadProcessMemory<T>(IntPtr memoryAddress, int size)
        {
            byte[] data = new byte[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = Marshal.ReadByte(memoryAddress, i);
            }

            if (!ConvertHelper.FromByteArray<T>(data, out T result))
            {
                // Last resort
                result = Marshal.PtrToStructure<T>(memoryAddress);
            }
            return result;
        }
    }
}
