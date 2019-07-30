using GameSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
        private static Delegate SafeFunction { get; set; }

        /// <summary>
        ///     Contains the key: "Function address which we are calling" and the value "CodeCave of where we are calling the function from".
        /// </summary>
        private static Dictionary<uint, uint> CachedCodeCaves = new Dictionary<uint, uint>();

        public T Call<T>(params object[] parameters)
        {
            IntPtr address = ToCallDelegate().ToFunctionPtr();

            CachedCodeCaves.TryGetValue((uint)address, out uint cachedCodeCaveValue);
            Delegate safeFunction;

            // If we don't have a code cave jump address yet we create one, otherwise cache it to improve performance.
            if (cachedCodeCaveValue == 0)
            {
                ProcessModule module = address.GetModuleWhichBelongsToAddress();
                List<byte> bytes = new List<byte>();
                bytes.AddRange(address.GetReturnToPtr());
                IntPtr codeCave = module.FindCodeCaveInModule((uint) bytes.Count);

                // TODO: Refactor since this is now a detection vector as we are now writing the JumpTable into the process.
                codeCave.Write(bytes.ToArray());
                CachedCodeCaves.Add((uint) address, (uint) codeCave);

                Type typeOfDelegate = ToCallDelegate().GetType();
                safeFunction = Marshal.GetDelegateForFunctionPointer(codeCave, typeOfDelegate);
            }
            else
            {
                safeFunction = Marshal.GetDelegateForFunctionPointer(new IntPtr(cachedCodeCaveValue), ToCallDelegate().GetType());
            }

            T ret = (T)safeFunction.DynamicInvoke(parameters);

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
