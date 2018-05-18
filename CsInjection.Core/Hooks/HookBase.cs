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
            Address = GetModuleAddress() + GetHookAddress();
        }

        public virtual IntPtr GetModuleAddress()
        {
            return Process.GetCurrentProcess().MainModule.BaseAddress;
        }

        public virtual int GetHookAddress()
        {
            throw new NotImplementedException();
        }

        public virtual Delegate GetDetourDelegate()
        {
            throw new NotImplementedException();
        }

        public virtual string GetHookName()
        {
            throw new NotImplementedException();
        }

        public void InstallHook()
        {
            //LocalHook hook = LocalHook.Create(Address, GetDetourDelegate(), this);
            //hook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            //NativeAPI.RhWakeUpProcess();
        }
    }
}
