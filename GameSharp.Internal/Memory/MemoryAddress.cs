using GameSharp.Core.Memory;
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

        public T Read<T>() where T : struct
        {
            throw new NotImplementedException();
        }

        public void Write()
        {
            throw new NotImplementedException();
        }

        public T Read<T>(int size)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] bytes)
        {
            throw new NotImplementedException();
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
