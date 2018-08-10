using CsInjection.Core.Utilities;
using System;

namespace CsInjection.Core.Hooks
{
    /// <summary>
    ///     Extend from <see cref="HookBase"/> with a hook you want to use.
    /// </summary>
    public abstract class HookBase
    {
        private Detour Detour;

        /// <summary>
        ///     Will be used to install the <c>Hook</c> and enable it.
        /// </summary>
        public void InstallHook()
        {
            if (Detour == null)
                Detour = new Detour(GetHookDelegate(), GetDetourDelegate());
            Detour.Enable();
        }

        /// <summary>
        ///     Will be used to uninstall the <c>Hook</c>.
        /// </summary>
        public void UninstallHook()
        {
            Detour.Disable();
        }

        /// <summary>
        ///     This should return an UnmanagedFunctionPointer delegate.
        ///
        ///     e.g. Marshal.GetDelegateForFunctionPointer<DELEGATE>(ADDRESS);
        /// </summary>
        /// <returns></returns>
        public abstract Delegate GetHookDelegate();

        /// <summary>
        ///     This should return the delegate to the hook with the function where it needs to go.
        ///
        ///     e.g. return new OnAfkDelegate(DetourMethod);
        /// </summary>
        public abstract Delegate GetDetourDelegate();

        /// <summary>
        ///     Call the original function, parameters are taken right to left.
        /// </summary>
        /// <param name="parms"></param>
        protected void CallOriginal(params object[] parms)
        {
            Detour.CallOriginal(parms);
        }
    }
}