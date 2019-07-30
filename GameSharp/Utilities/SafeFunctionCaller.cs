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

            // If we don't have a code cave jump table yet we create one, otherwise cache it to improve performance.
            if (cachedCodeCaveValue == 0)
            {
                ProcessModule module = address.GetModuleWhichBelongsToAddress();
                List<byte> bytes = new List<byte>();
                bytes.AddRange(GetHookBytes(address));
                IntPtr codeCave = module.FindCodeCaveInModule((uint) bytes.Count);

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

        private byte[] GetHookBytes(IntPtr ptrToJumpTo)
        {
            // PUSH opcode http://ref.x86asm.net/coder32.html#x68
            List<byte> bytes = new List<byte> { 0x68 };

            // Push our hook address onto the stack
            byte[] hookPtrAddress = IntPtr.Size == 4 ? BitConverter.GetBytes(ptrToJumpTo.ToInt32()) : BitConverter.GetBytes(ptrToJumpTo.ToInt64());

            bytes.AddRange(hookPtrAddress);

            // RETN opcode http://ref.x86asm.net/coder32.html#xC3
            bytes.Add(0xC3);

            return bytes.ToArray();
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
