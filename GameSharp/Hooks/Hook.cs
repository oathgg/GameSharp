// Part of the credits go to Lolp1 for giving the idea how to finish.
// https://github.com/lolp1/Process.NET/blob/master/src/Process.NET/Applied/Detours/Detour.cs
//

using GameSharp.Extensions;
using GameSharp.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameSharp.Hooks
{
    public abstract class Hook
    {
        /// <summary>
        ///     This var is not used within the detour itself. It is only here
        ///     to keep a reference, to avoid the GC from collecting the delegate instance!
        /// </summary>
        private readonly Delegate HookDelegate;

        /// <summary>
        ///     Gets the pointer to be hooked/being hooked.
        /// </summary>
        private IntPtr HookPtr { get; }

        /// <summary>
        ///     Contains the data of our patch
        /// </summary>
        private Patch Patch { get; }

        /// <summary>
        ///     Gets the pointer of the target function.
        /// </summary>
        private IntPtr TargetFuncPtr { get; }

        /// <summary>
        ///     Gets the targeted delegate instance.
        /// </summary>
        private Delegate TargetDelegate { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HookBase" /> class.
        /// </summary>
        /// <param name="target">The target delegate we want to detour.</param>
        /// <param name="hook">The hook delegate where want it to go.</param>
        public Hook()
        { 
            TargetDelegate = GetHookDelegate();
            TargetFuncPtr = TargetDelegate.ToFunctionPtr();

            HookDelegate = GetDetourDelegate();
            HookPtr = HookDelegate.ToFunctionPtr();

            // PUSH opcode http://ref.x86asm.net/coder32.html#x68
            List<byte> bytes = new List<byte> { 0x68 };

            // Push our hook address onto the stack
            byte[] hookPtrAddress = IntPtr.Size == 4 ? BitConverter.GetBytes(HookPtr.ToInt32()) : BitConverter.GetBytes(HookPtr.ToInt64());

            bytes.AddRange(hookPtrAddress);

            // RETN opcode http://ref.x86asm.net/coder32.html#xC3
            bytes.Add(0xC3);

            Patch = new Patch(TargetFuncPtr, bytes.ToArray());
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. In this
        ///     case, it will disable the <see cref="HookBase" /> instance and suppress the finalizer.
        /// </summary>
        public void Dispose()
        {
            Disable();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Removes this Detour from memory. (Reverts the bytes back to their originals.)
        /// </summary>
        public void Disable()
        {
            Patch.Disable();
        }

        /// <summary>
        ///     Applies this Detour to memory. (Writes new bytes to memory)
        /// </summary>
        /// <returns></returns>
        public void Enable()
        {
            Patch.Enable();
        }

        /// <summary>
        ///     Source code found on
        ///     https://github.com/lolp1/Process.NET/blob/master/src/Process.NET/Applied/Detours/Detour.cs#L215
        ///     
        ///     Calls the original function, and returns a return value.
        /// </summary>
        /// <param name="args">
        ///     The arguments to pass. If it is a 'void' argument list,
        ///     you MUST pass 'null'.
        /// </param>
        /// <returns>An object containing the original functions return value.</returns>
        public T CallOriginal<T>(params object[] args)
        {
            Disable();

            object ret = TargetDelegate.DynamicInvoke(args);

            Enable();

            return (T) ret;
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