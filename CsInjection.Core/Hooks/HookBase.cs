using System;
using CsInjection.Core.Utilities;
using System.Diagnostics;

namespace CsInjection.Core.Hooks
{
    /// <summary>
    ///     Extend from <see cref="HookBase"/> with a hook you want to use.
    /// </summary>
    public abstract class HookBase
    {
        protected Detour Detour;

        /// <summary>
        ///     Will be used to install the <see cref="IHook"/> and enable it.
        /// </summary>
        public void InstallHook()
        {
            Detour = new Detour(GetHookDelegate(), GetDetourDelegate());
            Detour.Enable();
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
        ///     In this function you can call the <see cref="Detour.CallOriginal(object[])"/> of the <see cref="Detour"/> class
        ///     to also call the original function of the program.
        /// </summary>
        public abstract Delegate GetDetourDelegate();
    }
}
