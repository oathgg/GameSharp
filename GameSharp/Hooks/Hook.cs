using GameSharp.Utilities;
using System;

namespace GameSharp.Hooks
{
    /// <summary>
    ///     This is a simple HookBase wrapper, extend from this class.
    /// </summary>
    public abstract class Hook
    {
        private HookBase HookBase { get; set; }

        public Hook()
        {
            HookBase = new HookBase(GetHookDelegate(), GetDetourDelegate());
        }

        public void Enable()
        {
            HookBase.Enable();
        }

        public void Disable()
        {
            HookBase.Disable();
        }

        public T CallOriginal<T>(params object[] args)
        {
            return HookBase.CallOriginal<T>(args);
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