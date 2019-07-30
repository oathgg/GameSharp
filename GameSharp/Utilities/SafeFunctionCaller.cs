using GameSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameSharp.Utilities
{
    /// <summary>
    ///     By extending from this class you're creating a somewhat safe way to call a function.
    ///     This class injects opcodes into the process where it can find a codecave.
    ///     The opcodes will become a jumptable between your malicious module and the code block of where the function resides.
    ///     Bypasses the anti-cheat which is validating the return address of the calling function.
    /// </summary>
    public abstract class SafeFunctionCaller
    {
        private Delegate SafeFunction { get; set; }

        /// <summary>
        ///     
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T Call<T>(params object[] parameters)
        {
            if (SafeFunction == null)
            {
                IntPtr address = ToCallDelegate().ToFunctionPtr();

                ProcessModule module = address.GetModuleWhichBelongsToAddress();

                List<byte> bytes = new List<byte>();

                bytes.AddRange(address.GetReturnToPtr());

                IntPtr codeCave = module.FindCodeCaveInModule((uint) bytes.Count);

                // TODO: Refactor since this is now a detection vector as we are now writing the 'JumpTable' into the process.
                codeCave.Write(bytes.ToArray());

                Type typeOfDelegate = ToCallDelegate().GetType();

                SafeFunction = Marshal.GetDelegateForFunctionPointer(codeCave, typeOfDelegate);
            }

            T ret = (T)SafeFunction.DynamicInvoke(parameters);

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
