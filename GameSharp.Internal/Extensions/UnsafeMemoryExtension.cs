using GameSharp.Core.Memory;
using GameSharp.Internal.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameSharp.Internal.Extensions
{
    /// <summary>
    /// https://github.com/lolp1/Process.NET/blob/fbe6fd3f865dc0e2e2f62bb5dfc41a423952aebc/src/Process.NET/Extensions/UnsafeMemoryExtensions.cs
    /// </summary>
    public static class UnsafeMemoryExtension
    {
        public static MemoryAddress ToFunctionPtr(this Delegate d)
        {
            return new MemoryAddress(Marshal.GetFunctionPointerForDelegate(d));
        }

        public static T ToDelegate<T>(this IMemoryAddress memoryAddress) where T : class
        {
            if (typeof(T).GetCustomAttributes(typeof(UnmanagedFunctionPointerAttribute), true).Length == 0)
            {
                throw new InvalidOperationException("This operation can only convert to delegates adorned with the UnmanagedFunctionPointerAttribute");
            }

            return Marshal.GetDelegateForFunctionPointer(memoryAddress.Address, typeof(T)) as T;
        }

        public static byte[] GetReturnToPtr(this IMemoryAddress memoryAddress)
        {
            // PUSH opcode http://ref.x86asm.net/coder32.html#x68
            List<byte> bytes = new List<byte> { 0x68 };

            // Push our hook address onto the stack
            byte[] hookPtrAddress = IntPtr.Size == 4 
                ? BitConverter.GetBytes(memoryAddress.Address.ToInt32()) 
                : BitConverter.GetBytes(memoryAddress.Address.ToInt64());

            bytes.AddRange(hookPtrAddress);

            // RETN opcode http://ref.x86asm.net/coder32.html#xC3
            bytes.Add(0xC3);

            return bytes.ToArray();
        }
    }
}
