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
        /// <summary>
        /// Gets an address from a vtable index. Since it uses index * IntPtr, it should work for both x64 and x32. 
        /// </summary>
        /// <param name="intPtr">The int PTR.</param>
        /// <param name="functionIndex">Index of the function.</param>
        /// <returns>IntPtr.</returns>
        public static InternalMemoryPointer GetVtableAddress(this MemoryPointer intPtr, int functionIndex)
        {
            IntPtr vftable = intPtr.Read<IntPtr>();
            IntPtr result = new InternalMemoryPointer(vftable + (functionIndex * IntPtr.Size)).Read<IntPtr>();
            return new InternalMemoryPointer(result);
        }

        public static InternalMemoryPointer ToFunctionPtr(this Delegate d)
        {
            return new InternalMemoryPointer(Marshal.GetFunctionPointerForDelegate(d));
        }

        public static T ToDelegate<T>(this MemoryPointer memoryAddress) where T : class
        {
            if (typeof(T).GetCustomAttributes(typeof(UnmanagedFunctionPointerAttribute), true).Length == 0)
            {
                throw new InvalidOperationException("This operation can only convert to delegates adorned with the UnmanagedFunctionPointerAttribute");
            }

            return Marshal.GetDelegateForFunctionPointer<T>(memoryAddress.Address);
        }

        public static byte[] CreateFunctionCall(this MemoryPointer memoryAddress)
        {
            List<byte> bytes = new List<byte>();

            // 64-bit
            if (GameSharpProcess.Instance.Is64Bit)
            {
                // MOV RAX, 
                bytes.AddRange(new byte[] { 0x48, 0xB8 });

                // FUNCTION PTR
                bytes.AddRange(BitConverter.GetBytes(memoryAddress.Address.ToInt64()));

                // CALL RAX
                bytes.AddRange(new byte[] { 0xFF, 0xD0 });
            }
            else
            {
                // PUSH opcode http://ref.x86asm.net/coder32.html#x68
                bytes.Add(0x68);

                // FUNCTION TO PUSH
                bytes.AddRange(BitConverter.GetBytes(memoryAddress.Address.ToInt32()));
            }

            // RETN opcode http://ref.x86asm.net/coder32.html#xC3
            bytes.Add(0xC3);

            return bytes.ToArray();
        }
    }
}
