using CsInjection.Core.Hooks;
using Injectable.Helpers;
using System;
using System.Runtime.InteropServices;

namespace Injectable.Hooks
{
    internal class HookOnCreateObject : HookBase
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, SetLastError = true)]
        public delegate void OnCreateDelegate(IntPtr thisPtr, IntPtr obj);

        public override Delegate GetHookDelegate()
        {
            return Marshal.GetDelegateForFunctionPointer<OnCreateDelegate>(Offsets.OnCreateObject);
        }

        public override Delegate GetDetourDelegate()
        {
            return new OnCreateDelegate(DetourMethod);
        }

        private void DetourMethod(IntPtr thisPtr, IntPtr obj)
        {
            Console.WriteLine($"Created object {obj.ToString("X")}");
            CallOriginal(thisPtr, obj);
        }
    }
}