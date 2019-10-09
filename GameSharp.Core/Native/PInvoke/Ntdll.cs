using GameSharp.Core.Native.Enums;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Core.Native.PInvoke
{
    public static class Ntdll
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern uint NtQueryInformationProcess(IntPtr processHandle, ProcessInformationClass processInformationClass, IntPtr processInformation, uint processInformationLength, out IntPtr returnLength);
    }
}
