using System;
using CsInjection.Core.Utilities;
using System.Diagnostics;

namespace CsInjection.Core.Hooks
{
    /// <summary>
    ///     Extend from <see cref="HookBase"/> with a hook you want to use.
    /// </summary>
    public class HookBase : IHook
    {
        protected Detour Detour;

        public void InstallHook()
        {
            Detour = new Detour(GetHookDelegate(), GetDetourDelegate());
            Detour.Enable();
        }

        public virtual Delegate GetHookDelegate()
        {
            throw new NotImplementedException();
        }

        public virtual Delegate GetDetourDelegate()
        {
            throw new NotImplementedException();
        }
    }
}
