using GameSharp.Extensions;
using GameSharp.Module;
using GameSharp.Native;
using GameSharp.Native.Enums;
using GameSharp.Processes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameSharp.Memory
{
    public class MemoryAddress
    {
        public IntPtr IntPtr { get; }

        public MemoryAddress() => new MemoryAddress(IntPtr.Zero);

        public MemoryAddress(IntPtr address)
        {
            IntPtr = address;
        }

        public T Read<T>(int offset = 0) where T : struct
        {
            T result = Marshal.PtrToStructure<T>(IntPtr + offset);
            return result;
        }

        public T Read<T>(int size, int offset = 0)
        {
            byte[] destination = new byte[size];
            Marshal.Copy(IntPtr, destination, offset, destination.Length);
            return destination.CastTo<T>();
        }

        public void Write(byte[] data)
        {
            // Make sure we have Write access to the page.
            Kernel32.VirtualProtect(IntPtr, data.Length, MemoryProtection.WriteCopy, out MemoryProtection old);
            Marshal.Copy(data, 0, IntPtr, data.Length);
            Kernel32.VirtualProtect(IntPtr, data.Length, old, out _);
        }

        public byte[] GetReturnToPtr()
        {
            // PUSH opcode http://ref.x86asm.net/coder32.html#x68
            List<byte> bytes = new List<byte> { 0x68 };

            // Push our hook address onto the stack
            byte[] hookPtrAddress = IntPtr.Size == 4 ? BitConverter.GetBytes(IntPtr.ToInt32()) : BitConverter.GetBytes(IntPtr.ToInt64());

            bytes.AddRange(hookPtrAddress);

            // RETN opcode http://ref.x86asm.net/coder32.html#xC3
            bytes.Add(0xC3);

            return bytes.ToArray();
        }

        public InternalModule GetMyModule()
        {
            foreach (InternalModule module in InternalProcess.Instance.Modules)
            {
                if (IntPtr.ToInt64() > module.ModuleBaseAddress.IntPtr.ToInt64()
                    && IntPtr.ToInt64() < module.ModuleBaseAddress.IntPtr.ToInt64() + module.Size)
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
