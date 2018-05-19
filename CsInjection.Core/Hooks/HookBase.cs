using System;
using CsInjection.Core.Helpers;
using System.Diagnostics;

namespace CsInjection.Core.Hooks
{
    public class HookBase : IHook
    {
        protected IntPtr Address;

        public HookBase()
        {
            Address = GetModuleAddress() + GetAddress();
        }

        public virtual IntPtr GetModuleAddress()
        {
            return Process.GetCurrentProcess().MainModule.BaseAddress;
        }

        public virtual int GetAddress()
        {
            throw new NotImplementedException();
        }

        public virtual Delegate GetDelegate()
        {
            throw new NotImplementedException();
        }

        public virtual string GetName()
        {
            throw new NotImplementedException();
        }

        public void InstallHook()
        {
            // Allocate memory (Needed for our detour and old function?)
            // Create a copy of the first 10 bytes of the to be hooked function to the allocation
            // Create a jump back to the original function + 10 bytes
            // Create a jump in the function to our detour
            // Do our stuff and do a call do a delegate call of the original function
        }
    }
}
