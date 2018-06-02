using CsInjection.Core.Hooks;
using CsInjection.LeagueOfLegends.Helpers;
using System;
using System.Runtime.InteropServices;

namespace CsInjection.LeagueOfLegends.Hooks
{
    class HookOnCreateObject : HookBase
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, SetLastError = true)]
        public delegate void OnCreateDelegate(IntPtr thisPtr, IntPtr @object);

        public override Delegate GetHookDelegate()
        {
            return Marshal.GetDelegateForFunctionPointer<OnCreateDelegate>(Offsets.OnCreateObject);
        }

        public override Delegate GetDetourDelegate()
        {
            return new OnCreateDelegate(DetourMethod);
        }

        private void DetourMethod(IntPtr thisPtr, IntPtr @object)
        {
            Console.WriteLine($"Created object {@object.ToString("X")}");
            Detour.CallOriginal(thisPtr, @object);
        }
    }
}
