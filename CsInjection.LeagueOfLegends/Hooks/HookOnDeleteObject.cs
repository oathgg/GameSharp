using CsInjection.Core.Hooks;
using CsInjection.LeagueOfLegends.Helpers;
using System;
using System.Runtime.InteropServices;

namespace CsInjection.LeagueOfLegends.Hooks
{
    class HookOnDeleteObject : HookBase
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, SetLastError = true)]
        public delegate void OnDeleteDelegate(IntPtr thisPtr, IntPtr @object);

        public override Delegate GetHookDelegate()
        {
            return Marshal.GetDelegateForFunctionPointer<OnDeleteDelegate>(Offsets.OnDeleteObject);
        }

        public override Delegate GetDetourDelegate()
        {
            return new OnDeleteDelegate(DetourMethod);
        }

        private void DetourMethod(IntPtr thisPtr, IntPtr @object)
        {
            Console.WriteLine($"Deleted object 0x{@object.ToString("X")}");
            Detour.CallOriginal(thisPtr, @object);
        }
    }
}
