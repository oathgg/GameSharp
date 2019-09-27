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

            return Marshal.GetDelegateForFunctionPointer<T>(memoryAddress.Address);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryAddress"></param>
        /// <param name="convention">Gets only used by the x64 architecture as we have to prepare the stack
        /// The reason why we're only using this in x64 is because we're using the ret operator in x86.
        /// 
        /// Windows defaults to FastCalls in x64.</param>
        /// <returns></returns>
        public static byte[] GetReturnToPtr(this IMemoryAddress memoryAddress, CallingConvention convention)
        {
            List<byte> bytes = new List<byte>();

            // 64-bit
            if (IntPtr.Size == 8)
            {
                //// Prepare the stack
                switch (convention)
                {
                    case CallingConvention.FastCall:
                        // SUB RSP, 28
                        bytes.AddRange(new byte[] { 0x48, 0x83, 0xEC, 0x28 });
                        break;

                    default:
                        throw new NotImplementedException();
                }

                // MOV RAX, FUNKPTR
                bytes.AddRange(new byte[] { 0x48, 0xB8 });

                // Our address which we wish to call
                bytes.AddRange(BitConverter.GetBytes(memoryAddress.Address.ToInt64()));

                // CALL RAX
                bytes.AddRange(new byte[] { 0xFF, 0xD0 });

                // Clean up the stack
                switch (convention)
                {
                    case CallingConvention.FastCall:
                        // ADD RSP, 28
                        bytes.AddRange(new byte[] { 0x48, 0x83, 0xC4, 0x28 });
                        break;
                }
            }
            else
            {
                // PUSH opcode http://ref.x86asm.net/coder32.html#x68
                bytes.Add(0x68);
                bytes.AddRange(BitConverter.GetBytes(memoryAddress.Address.ToInt32()));
            }

            // RETN opcode http://ref.x86asm.net/coder32.html#xC3
            bytes.Add(0xC3);

            return bytes.ToArray();
        }
    }
}
