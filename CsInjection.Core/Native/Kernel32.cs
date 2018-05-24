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
    }
}
