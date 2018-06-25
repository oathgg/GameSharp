using CsInjection.Core.Hooks;
using System;
using System.Runtime.InteropServices;

namespace Sandbox.Injectable
{
    class HookOnAbout : HookBase
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        public delegate void OnAboutDelegate();

        public override Delegate GetHookDelegate()
        {
            return Marshal.GetDelegateForFunctionPointer<OnAboutDelegate>
                (CsInjection.Core.Helpers.ProcessHelper.GetMainModuleBaseAddress + 0x11AB0);
        }

        public override Delegate GetDetourDelegate()
        {
            return new OnAboutDelegate(DetourMethod);
        }

        private void DetourMethod()
        {
            Console.WriteLine($"About has been pressed.");
            Detour.CallOriginal();
        }
    }
}
