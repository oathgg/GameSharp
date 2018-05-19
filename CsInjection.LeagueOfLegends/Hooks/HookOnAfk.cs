using CsInjection.Core.Hooks;
using System;
using System.Runtime.InteropServices;

namespace CsInjection.LeagueOfLegends.Hooks
{
    class HookOnAfk : HookBase
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, SetLastError = true)]
        public delegate void OnAfkDelegate(IntPtr thisPtr);

        public override string GetName()
        {
            return "OnAfk";
        }

        public override int GetAddress()
        {
            // Patch 8.10.229.7328
            return 0x005D6BC0;
        }

        public override Delegate GetDelegate()
        {
            return new OnAfkDelegate(Detour);
        }

        private void Detour(IntPtr thisPtr)
        {
            Console.WriteLine($"Event::{GetName()}");
        }
    }
}
