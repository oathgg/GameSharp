using GameSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameSharp.Memory
{
    /// <summary>
    ///     By extending from this class you're creating a somewhat safe way to call a function.
    ///     This class injects opcodes into the process where it can find a codecave.
    ///     The opcodes will become a jumptable between your malicious module and the code block of where the function resides.
    ///     This bypass is to prevent a possible anti-cheat method which validates the return address of a function to reside in it's own module code section.
    /// </summary>
    public abstract class SafeFunction
    {
        private readonly Delegate SafeFunctionDelegate;

        public SafeFunction()
        {
            IntPtr address = ToCallDelegate().ToFunctionPtr();

            ProcessModule module = address.GetModuleWhichBelongsToAddress();

            List<byte> bytes = new List<byte>();

            bytes.AddRange(address.GetReturnToPtr());

            IntPtr codeCave = module.FindCodeCaveInModule((uint)bytes.Count);

            // TODO: Refactor since this is now a detection vector as we are now writing the 'JumpTable' into the process.
            codeCave.Write(bytes.ToArray());

            Type typeOfDelegate = ToCallDelegate().GetType();

            SafeFunctionDelegate = Marshal.GetDelegateForFunctionPointer(codeCave, typeOfDelegate);
        }

        public T Call<T>(params object[] parameters)
        {
            T ret = (T)SafeFunctionDelegate.DynamicInvoke(parameters);

            return ret;
        }

        /// <summary>
        ///     This should return an UnmanagedFunctionPointer delegate.
        ///
        ///     e.g. Marshal.GetDelegateForFunctionPointer<DELEGATE>(ADDRESS);
        /// </summary>
        /// <returns></returns>
        public abstract Delegate ToCallDelegate();
    }
}
