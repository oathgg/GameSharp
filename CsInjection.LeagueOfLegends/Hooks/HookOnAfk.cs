using CsInjection.Core.Hooks;
using System;
using System.Runtime.InteropServices;

namespace CsInjection.LeagueOfLegends.Hooks
{
    class HookOnAfk : HookBase
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, SetLastError = true)]
        public delegate void OnAfkDelegate(IntPtr thisPtr);

        public override string GetHookName()
        {
            return "OnAfk";
        }

        public override int GetHookAddress()
        {
            // Patch 8.10.229.7328
            return 0x005D6BC0;
        }

        public override Delegate GetDetourDelegate()
        {
            return new OnAfkDelegate(OnDetour);
        }

        private void OnDetour(IntPtr thisPtr)
        {
            Console.WriteLine($"Event::{GetHookName()}");

            Marshal.GetDelegateForFunctionPointer<OnAfkDelegate>(Address)(thisPtr);
        }
    }
}
