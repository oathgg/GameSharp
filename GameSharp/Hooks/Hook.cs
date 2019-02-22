using GameSharp.Utilities;
using System;

namespace GameSharp.Hooks
{
    /// <summary>
    ///     Extend from <see cref="Hook"/> with a hook you want to use.
    /// </summary>
    public abstract class Hook
    {
        private HookBase _hookBase { get; set; }

        public Hook()
        {
            _hookBase = new HookBase(GetHookDelegate(), GetDetourDelegate());
        }

        public void Enable()
        {
            _hookBase.Enable();
        }

        public void Disable()
        {
            _hookBase.Disable();
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
    }
}