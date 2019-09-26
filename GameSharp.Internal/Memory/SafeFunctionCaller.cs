using GameSharp.Core.Memory;
using GameSharp.Core.Services;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace GameSharp.Internal.Memory
{
    /// <summary>
    ///     By extending from this class you're creating a somewhat safe way to call a function.
    ///     This class injects opcodes into the process where it can find a codecave.
    ///     The opcodes will become a jumptable between your injected library and the code block of where the function resides.
    ///     This bypass is to prevent a possible anti-cheat method which validates the return address of a function to reside in it's own module code section.
    /// </summary>
    public abstract class SafeFunction
    {
        private static Delegate SafeFunctionDelegate;

        public SafeFunction()
        {
            Delegate @delegate = ToCallDelegate();

            MemoryAddress originalFuncPtr = @delegate.ToFunctionPtr();

            MemoryModule module = originalFuncPtr.GetMyModule();

            List<byte> bytes = new List<byte>();

            bytes.AddRange(originalFuncPtr.GetReturnToPtr());

            IMemoryAddress codeCave = module.FindCodeCaveInModule((uint)bytes.Count);

            codeCave.Write(bytes.ToArray());

            Type typeOfDelegate = @delegate.GetType();

            SafeFunctionDelegate = Marshal.GetDelegateForFunctionPointer(codeCave.Address, typeOfDelegate);
        }

        public T Call<T>(params object[] parameters)
        {
            object a = SafeFunctionDelegate.DynamicInvoke(parameters);

            T ret = default;
            if (a is T)
            {
                ret = (T)a;
            }

            return ret;
        }

        /// <summary>
        ///     This should return an UnmanagedFunctionPointer delegate.
        /// </summary>
        /// <code>
        ///     return IMemoryAddress.ToDelegate<DELEGATE>();
        /// </code>
        /// <returns></returns>
        protected abstract Delegate ToCallDelegate();
    }
}
