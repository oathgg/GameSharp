using CsInjection.Core.Hooks;
using CsInjection.LeagueOfLegends.Helpers;
using System;
using System.Runtime.InteropServices;

namespace CsInjection.LeagueOfLegends.Hooks
{
    class HookOnUpdate : HookBase
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, SetLastError = true)]
        public delegate void OnUpdateDelegate(IntPtr thisPtr);

        public override Delegate GetHookDelegate()
        {
            return Marshal.GetDelegateForFunctionPointer<OnUpdateDelegate>(Offsets.OnUpdate);
        }

        public override Delegate GetDetourDelegate()
        {
            return new OnUpdateDelegate(DetourMethod);
        }

        private void DetourMethod(IntPtr thisPtr)
        {
            Console.WriteLine($"Event::OnUpdate");
            Detour.CallOriginal(thisPtr);
        }
    }
}
