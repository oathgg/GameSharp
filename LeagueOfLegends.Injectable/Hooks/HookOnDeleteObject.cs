using CsInjection.Core.Hooks;
using Injectable.Helpers;
using System;
using System.Runtime.InteropServices;

namespace Injectable.Hooks
{
    class HookOnDeleteObject : HookBase
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, SetLastError = true)]
        public delegate void OnDeleteDelegate(IntPtr thisPtr, IntPtr obj);

        public override Delegate GetHookDelegate()
        {
            return Marshal.GetDelegateForFunctionPointer<OnDeleteDelegate>(Offsets.OnDeleteObject);
        }

        public override Delegate GetDetourDelegate()
        {
            return new OnDeleteDelegate(DetourMethod);
        }

        private void DetourMethod(IntPtr thisPtr, IntPtr obj)
        {
            Console.WriteLine($"Deleted object 0x{obj.ToString("X")}");
            CallOriginal(thisPtr, obj);
        }
    }
}
