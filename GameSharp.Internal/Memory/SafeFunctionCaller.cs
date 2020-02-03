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
        private readonly IMemoryPointer CodeCaveJumpTable;
        private readonly int CodeCaveSize;

        public SafeFunction()
        {
            Delegate motherDelegate = InitializeDelegate();

            MemoryPointer originalFuncPtr = motherDelegate.ToFunctionPtr();

            InternalModulePointer module = originalFuncPtr.GetMyModule();

            Type typeOfDelegate = motherDelegate.GetType();

            List<byte> bytes = new List<byte>();

            bytes.AddRange(originalFuncPtr.GetReturnToPtr());

            CodeCaveSize = bytes.Count < 12 ? 12 : bytes.Count;

            if (module != null)
            {
                // If the function belongs to a module we can find we want to inject our code bytes into that module before calling it.
                CodeCaveJumpTable = module.FindCodeCaveInModule((uint)CodeCaveSize);
            }
            else
            {
                // Otherwise we create a fictional jump table...
                CodeCaveJumpTable = GameSharpProcess.Instance.AllocateManagedMemory(CodeCaveSize);
            }

            CodeCaveJumpTable.Write(bytes.ToArray());

            SafeFunctionDelegate = Marshal.GetDelegateForFunctionPointer(originalFuncPtr.Address, typeOfDelegate);
        }

        /// <summary>
        ///     Call this method from your class which extends from the SafeFunction class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected T BaseCall<T>(params object[] parameters)
        {
            if (SafeFunctionDelegate == null)
            {
                return default;
            }

            if (!Kernel32.VirtualProtect(SafeFunctionDelegate.ToFunctionPtr().Address, CodeCaveSize, MemoryProtection.ExecuteReadWrite, out MemoryProtection old))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

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
    }
}
