using CsInjection.Core.Helpers;
using System;
using System.Runtime.InteropServices;

namespace Sandbox.Injectable
{
    public class CallAbout
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        private delegate IntPtr AboutDelegate();
        public static void Run()
        {
            Marshal.GetDelegateForFunctionPointer<AboutDelegate>(ProcessHelper.GetMainModuleBaseAddress + 0x11AB0).DynamicInvoke();
        }
    }
}
