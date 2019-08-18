using GameSharp.Core.Memory;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.PInvoke;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Module;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameSharp.Internal.Memory
{
    public class MemoryAddress : IMemoryAddress
    {
        public IntPtr BaseAddress { get; }

        public MemoryAddress(IntPtr address)
        {
            BaseAddress = address;
        }

        public T Read<T>(int offset = 0) where T : struct
        {
            T result = Marshal.PtrToStructure<T>(BaseAddress + offset);
            return result;
        }

        public T Read<T>(int size, int offset = 0)
        {
            byte[] destination = new byte[size];
            Marshal.Copy(BaseAddress, destination, offset, destination.Length);
            return destination.CastTo<T>();
        }

        public void Write(byte[] data)
        {
            // Make sure we have Write access to the page.
            Kernel32.VirtualProtect(BaseAddress, data.Length, MemoryProtection.WriteCopy, out MemoryProtection old);
            Marshal.Copy(data, 0, BaseAddress, data.Length);
            Kernel32.VirtualProtect(BaseAddress, data.Length, old, out _);
        }

        public byte[] GetReturnToPtr()
        {
            // PUSH opcode http://ref.x86asm.net/coder32.html#x68
            List<byte> bytes = new List<byte> { 0x68 };

            // Push our hook address onto the stack
            byte[] hookPtrAddress = IntPtr.Size == 4 ? BitConverter.GetBytes(BaseAddress.ToInt32()) : BitConverter.GetBytes(BaseAddress.ToInt64());

            bytes.AddRange(hookPtrAddress);

            // RETN opcode http://ref.x86asm.net/coder32.html#xC3
            bytes.Add(0xC3);

            return bytes.ToArray();
        }

        public MemoryModule GetMyModule()
        {
            foreach (MemoryModule module in GameSharpProcess.Instance.Modules)
            {
                if (BaseAddress.ToInt64() > module.BaseAddress.ToInt64()
                    && BaseAddress.ToInt64() < module.BaseAddress.ToInt64() + module.Size)
                {
                    return module;
                }
            }
            return null;
        }

        public static MemoryAddress AllocateMemory(int size)
        {
            return new MemoryAddress(Marshal.AllocHGlobal(size));
        }
    }
}
