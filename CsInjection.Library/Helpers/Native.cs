using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace CsInjection.Library.Helpers
{    
    public static class Native
    {
        #region AllocConsole
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int AllocConsole();
        #endregion

        #region MemoryShit
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);
        public static void WriteToMemory(IntPtr memoryAddress, byte[] newBytes)
        {
            WriteProcessMemory(ProcessHelper.GetCurrentProcess.Handle, memoryAddress, newBytes, newBytes.Length, out IntPtr outVar);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesRead);
        public static T ReadFromMemory<T>(IntPtr memoryAddress, int size)
        {
            byte[] buffer = new byte[Marshal.SizeOf<T>()];
            ReadProcessMemory(ProcessHelper.GetCurrentProcess.Handle, memoryAddress, buffer, size, out IntPtr outVar);
            return Convert.FromByteArray<T>(buffer);
        }
        #endregion
    }
}
