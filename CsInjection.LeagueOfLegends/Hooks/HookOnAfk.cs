using CsInjection.Core.Hooks;
using CsInjection.LeagueOfLegends.Helpers;
using System;
using System.Runtime.InteropServices;

namespace CsInjection.LeagueOfLegends.Hooks
{
    class HookOnAfk : HookBase
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, SetLastError = true)]
        public delegate void OnAfkDelegate(IntPtr thisPtr);

        public override Delegate GetToHookDelegate()
        {
            return Marshal.GetDelegateForFunctionPointer<OnAfkDelegate>(Offsets.OnAfk);
        }

        public override Delegate GetToDetourDelegate()
        {
            return new OnAfkDelegate(DetourMethod);
        }

        private void DetourMethod(IntPtr thisPtr)
        {
            Console.WriteLine($"Event::ONAFK has been triggered");
            Detour.CallOriginal(thisPtr);
        }
    }
}
