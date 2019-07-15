// Part of the credits go to Lolp1 for giving the idea how to finish.
// https://github.com/lolp1/Process.NET/blob/master/src/Process.NET/Applied/Detours/Detour.cs
//

using GameSharp.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameSharp.Hooks
{
    internal class HookBase
    {
        /// <summary>
        ///     This var is not used within the detour itself. It is only here
        ///     to keep a reference, to avoid the GC from collecting the delegate instance!
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly Delegate HookDelegate;

        /// <summary>
        ///     Gets the pointer to be hooked/being hooked.
        /// </summary>
        private IntPtr HookPtr { get; }

        /// <summary>
        ///     Contains the data of our patch
        /// </summary>
        private Patch Patcher { get; }

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
        internal HookBase(Delegate target, Delegate hook)
        {
            TargetDelegate = target;
            TargetFuncPtr = Marshal.GetFunctionPointerForDelegate(target);

            HookDelegate = hook;
            HookPtr = Marshal.GetFunctionPointerForDelegate(hook);

            // PUSH opcode http://ref.x86asm.net/coder32.html#x68
            List<byte> bytes = new List<byte> { 0x68 };

            // Push our hook address onto the stack
            bytes.AddRange(BitConverter.GetBytes(IntPtr.Size == 4 ? HookPtr.ToInt32() : HookPtr.ToInt64()));

            // RETN opcode http://ref.x86asm.net/coder32.html#xC3
            bytes.Add(0xC3);

            Patcher = new Patch(TargetFuncPtr, bytes.ToArray());
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. In this
        ///     case, it will disable the <see cref="HookBase" /> instance and suppress the finalizer.
        /// </summary>
        internal void Dispose()
        {
            Disable();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Removes this Detour from memory. (Reverts the bytes back to their originals.)
        /// </summary>
        internal void Disable()
        {
            Patcher.Disable();
        }

        /// <summary>
        ///     Applies this Detour to memory. (Writes new bytes to memory)
        /// </summary>
        /// <returns></returns>
        internal void Enable()
        {
            Patcher.Enable();
        }

        /// <summary>
        ///     Calls the original function, and returns a return value.
        /// </summary>
        /// <param name="args">
        ///     The arguments to pass. If it is a 'void' argument list,
        ///     you MUST pass 'null'.
        /// </param>
        /// <returns>An object containing the original functions return value.</returns>
        internal T CallOriginal<T>(params object[] args)
        {
            Disable();

            object ret = TargetDelegate.DynamicInvoke(args);

            Enable();

            return (T) ret;
        }
    }
}