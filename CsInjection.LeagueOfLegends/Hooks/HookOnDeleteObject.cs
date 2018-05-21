using CsInjection.Core.Hooks;
using CsInjection.LeagueOfLegends.Helpers;
using System;
using System.Runtime.InteropServices;

namespace CsInjection.LeagueOfLegends.Hooks
{
    class HookOnDeleteObject : HookBase
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, SetLastError = true)]
        public delegate void OnDeleteDelegate(IntPtr thisPtr, IntPtr playerObject);

        public override Delegate GetToHookDelegate()
        {
            return Marshal.GetDelegateForFunctionPointer<OnDeleteDelegate>(Offsets.OnDeleteObject);
        }

        public override Delegate GetToDetourDelegate()
        {
            return new OnDeleteDelegate(DetourMethod);
        }

        private void DetourMethod(IntPtr thisPtr, IntPtr playerObject)
        {
            Console.WriteLine($"Deleted object 0x{playerObject.ToString("X")}");
            Detour.CallOriginal(thisPtr, playerObject);
        }
    }
}
