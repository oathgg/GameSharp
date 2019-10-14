using GameSharp.Core.Native.Enums;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Core.Native.PInvoke
{
    public static class Ntdll
    {
        // https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-erref/596a1078-e883-4972-9bbc-49e60bebca55
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern uint NtQueryInformationProcess(IntPtr processHandle, ProcessInformationClass processInformationClass, IntPtr processInformation, int bufferSize, out int returnLength);
    }
}
