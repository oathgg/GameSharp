using GameSharp.Extensions;
using GameSharp.Module;
using GameSharp.Native;
using GameSharp.Native.Enums;
using GameSharp.Processes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameSharp.Memory
{
    public class UnmanagedMemory
    {
        public IntPtr ManagedAddress { get; }

        public UnmanagedMemory() => new UnmanagedMemory(IntPtr.Zero);

        public UnmanagedMemory(IntPtr address)
        {
            ManagedAddress = address;
        }

        public T Read<T>(int offset = 0) where T : struct
        {
            T result = Marshal.PtrToStructure<T>(ManagedAddress + offset);
            return result;
        }

        public T Read<T>(int size, int offset = 0)
        {
            byte[] destination = new byte[size];
            Marshal.Copy(ManagedAddress, destination, offset, destination.Length);
            return destination.CastTo<T>();
        }

        public void Write(byte[] data)
        {
            // Make sure we have Write access to the page.
            Kernel32.VirtualProtect(ManagedAddress, data.Length, MemoryProtection.WriteCopy, out MemoryProtection old);
            Marshal.Copy(data, 0, ManagedAddress, data.Length);
            Kernel32.VirtualProtect(ManagedAddress, data.Length, old, out _);
        }

        public byte[] GetReturnToPtr()
        {
            // PUSH opcode http://ref.x86asm.net/coder32.html#x68
            List<byte> bytes = new List<byte> { 0x68 };

            // Push our hook address onto the stack
            byte[] hookPtrAddress = IntPtr.Size == 4 ? BitConverter.GetBytes(ManagedAddress.ToInt32()) : BitConverter.GetBytes(ManagedAddress.ToInt64());

            bytes.AddRange(hookPtrAddress);

            // RETN opcode http://ref.x86asm.net/coder32.html#xC3
            bytes.Add(0xC3);

            return bytes.ToArray();
        }

        public InternalModule GetMyModule()
        {
            ProcessModuleCollection modules = InternalProcess.Instance.Modules;
            foreach (ProcessModule module in modules)
            {
                if ((uint)ManagedAddress > (uint)module.BaseAddress && (uint)ManagedAddress < (uint)module.BaseAddress + module.ModuleMemorySize)
                {
                    return new InternalModule(module);
                }
            }
            return null;
        }

        public static UnmanagedMemory AllocateMemory(int size)
        {
            return new UnmanagedMemory(Marshal.AllocHGlobal(size));
        }
    }
}
