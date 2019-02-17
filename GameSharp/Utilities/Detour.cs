// Part of the credits go to Lolp1 for giving the idea how to finish.
// https://github.com/lolp1/Process.NET/blob/master/src/Process.NET/Applied/Detours/Detour.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameSharp.Utilities
{
    public class Detour
    {
        /// <summary>
        ///     This var is not used within the detour itself. It is only here
        ///     to keep a reference, to avoid the GC from collecting the delegate instance!
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly Delegate _hookDelegate;

        /// <summary>
        ///     Gets the pointer to be hooked/being hooked.
        /// </summary>
        private IntPtr _hook { get; }

        /// <summary>
        ///     Contains the data of our patch
        /// </summary>
        private BytePatcher _patcher { get; }

        /// <summary>
        ///     Gets the pointer of the target function.
        /// </summary>
        private IntPtr _target { get; }

        /// <summary>
        ///     Gets the targeted delegate instance.
        /// </summary>
        private Delegate _targetDelegate { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Detour" /> class.
        /// </summary>
        /// <param name="target">The target delegate we want to detour.</param>
        /// <param name="hook">The hook delegate where want it to go.</param>
        public Detour(Delegate target, Delegate hook)
        {
            _targetDelegate = target;
            _target = Marshal.GetFunctionPointerForDelegate(target);

            _hookDelegate = hook;
            _hook = Marshal.GetFunctionPointerForDelegate(hook);

            // PUSH opcode http://ref.x86asm.net/coder32.html#x68
            List<byte> bytes = new List<byte> { 0x68 };

            // Address of our hook.
            bytes.AddRange(BitConverter.GetBytes(_hook.ToInt32()));

            // RETN opcode http://ref.x86asm.net/coder32.html#xC3
            bytes.Add(0xC3);

            _patcher = new BytePatcher(new IntPtr(_target.ToInt32()), bytes.ToArray());
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. In this
        ///     case, it will disable the <see cref="Detour" /> instance and suppress the finalizer.
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
            _patcher.Disable();
        }

        /// <summary>
        ///     Applies this Detour to memory. (Writes new bytes to memory)
        /// </summary>
        /// <returns></returns>
        public void Enable()
        {
            _patcher.Enable();
        }

        /// <summary>
        ///     Calls the original function, and returns a return value.
        /// </summary>
        /// <param name="args">
        ///     The arguments to pass. If it is a 'void' argument list,
        ///     you MUST pass 'null'.
        /// </param>
        /// <returns>An object containing the original functions return value.</returns>
        public object CallOriginal(params object[] args)
        {
            Disable();
            var ret = _targetDelegate.DynamicInvoke(args);
            Enable();
            return ret;
        }
    }
}