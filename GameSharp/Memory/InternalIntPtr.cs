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
    public class InternalIntPtr
    {
        public IntPtr Address { get; }

        public InternalIntPtr() => new InternalIntPtr(IntPtr.Zero);

        public InternalIntPtr(IntPtr address)
        {
            Address = address;
        }

        public T Read<T>(int offset = 0) where T : struct
        {
            T result = Marshal.PtrToStructure<T>(Address + offset);
            return result;
        }

        public T Read<T>(int size, int offset = 0)
        {
            byte[] destination = new byte[size];

            // Copy the memory to our own object
            Marshal.Copy(Address, destination, offset, destination.Length);

            return destination.CastTo<T>();
        }

        public void Write(byte[] data)
        {
            // Update the memory section so we can write to it if not writeable.
            Kernel32.VirtualProtect(Address, data.Length, MemoryProtection.ExecuteReadWrite, out MemoryProtection old);

            Marshal.Copy(data, 0, Address, data.Length);

            // Restore the page execution permissions.
            Kernel32.VirtualProtect(Address, data.Length, old, out _);
        }

        public byte[] GetReturnToPtr()
        {
            // PUSH opcode http://ref.x86asm.net/coder32.html#x68
            List<byte> bytes = new List<byte> { 0x68 };

            // Push our hook address onto the stack
            byte[] hookPtrAddress = IntPtr.Size == 4 ? BitConverter.GetBytes(Address.ToInt32()) : BitConverter.GetBytes(Address.ToInt64());

            bytes.AddRange(hookPtrAddress);

            // RETN opcode http://ref.x86asm.net/coder32.html#xC3
            bytes.Add(0xC3);

            return bytes.ToArray();
        }

        public InternalModule GetModuleWhichBelongsToAddress()
        {
            ProcessModuleCollection modules = InternalProcess.Instance.Modules;
            foreach (ProcessModule module in modules)
            {
                if ((uint)Address > (uint)module.BaseAddress && (uint)Address < (uint)module.BaseAddress + module.ModuleMemorySize)
                {
                    return new InternalModule(module);
                }
            }
            return null;
        }

        public static InternalIntPtr AllocateMemory(int size)
        {
            return new InternalIntPtr(Marshal.AllocHGlobal(size));
        }
    }
}
