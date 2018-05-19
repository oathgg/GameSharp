using CsInjection.Core.Helpers;
using System;
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

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesRead);

        public static T ReadProcessMemory<T>(IntPtr memoryAddress, int size)
        {
            byte[] buffer = new byte[size];
            ReadProcessMemory(ProcessHelper.GetCurrentProcess.Handle, memoryAddress, buffer, size, out IntPtr outVar);
            return ConvertHelper.FromByteArray<T>(buffer);
        }
    }
}
