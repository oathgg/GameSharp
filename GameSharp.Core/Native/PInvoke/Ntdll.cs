using System;
using System.Runtime.InteropServices;

namespace GameSharp.Core.Native.PInvoke
{
    public static class Ntdll
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, IntPtr processInformation, uint processInformationLength, out IntPtr returnLength);
    }
}
