using GameSharp.Core.Extensions;
using GameSharp.Core.Memory;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.PInvoke;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

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
        private readonly Delegate SafeFunctionDelegate;
        private readonly IMemoryAddress CodeCaveJumpTable;
        private readonly int CodeCaveSize;

        public SafeFunction()
        {
            Delegate @delegate = InitializeDelegate();

            MemoryAddress originalFuncPtr = @delegate.ToFunctionPtr();

            MemoryModule module = originalFuncPtr.GetMyModule();

            List<byte> bytes = new List<byte>();

            bytes.AddRange(originalFuncPtr.GetReturnToPtr(GetCallingConvention()));

            CodeCaveSize = bytes.Count < 12 ? 12 : bytes.Count;

            Type typeOfDelegate = @delegate.GetType();

            // If the function belongs to a module we want to inject our code bytes into that module before calling it.
            if (module != null)
            {
                CodeCaveJumpTable = module.FindCodeCaveInModule((uint)CodeCaveSize);
                CodeCaveJumpTable.Write(bytes.ToArray());
                SafeFunctionDelegate = Marshal.GetDelegateForFunctionPointer(CodeCaveJumpTable.Address, typeOfDelegate);
            }
            // Otherwise we just call the function.
            else
            {
                SafeFunctionDelegate = Marshal.GetDelegateForFunctionPointer(originalFuncPtr.Address, typeOfDelegate);
            }
        }

        public T Call<T>(params object[] parameters)
        {
            if (SafeFunctionDelegate == null)
                return default;

            if (!Kernel32.VirtualProtect(SafeFunctionDelegate.ToFunctionPtr().Address, CodeCaveSize, MemoryProtection.ExecuteReadWrite, out MemoryProtection old))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            object invokeResult = SafeFunctionDelegate.DynamicInvoke(parameters);

            Kernel32.VirtualProtect(SafeFunctionDelegate.ToFunctionPtr().Address, CodeCaveSize, old, out MemoryProtection _);

            T ret = invokeResult.CastTo<T>();

            return ret;
        }

        /// <summary>
        ///     This should return an UnmanagedFunctionPointer delegate.
        /// </summary>
        /// <code>
        ///     return IMemoryAddress.ToDelegate<DELEGATE>();
        /// </code>
        /// <returns></returns>
        protected abstract Delegate InitializeDelegate();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Defaults to FastCall as this is the x64 architecture standard routine</returns>
        protected virtual CallingConvention GetCallingConvention()
        {
            return CallingConvention.FastCall;
        }
    }
}
