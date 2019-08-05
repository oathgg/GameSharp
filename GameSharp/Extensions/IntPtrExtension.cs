using GameSharp.Native;
using GameSharp.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace GameSharp.Extensions
{
    public static class IntPtrExtension
    {
        public static T Read<T>(this IntPtr memoryAddress, int offset = 0) where T : struct
        {
            T result = Marshal.PtrToStructure<T>(memoryAddress + offset);
            return result;
        }

        public static T Read<T>(this IntPtr addr, int size, int offset = 0)
        {
            byte[] destination = new byte[size];

            // Copy the memory to our own object
            Marshal.Copy(addr, destination, offset, destination.Length);

            return destination.CastTo<T>();
        }

        public static void Write(this IntPtr addr, byte[] data)
        {
            // Update the memory section so we can write to it if not writeable.
            Kernel32.VirtualProtect(addr, data.Length, Enums.Protection.PAGE_EXECUTE_READWRITE, out Enums.Protection old);

            Marshal.Copy(data, 0, addr, data.Length);

            // Restore the page execution permissions.
            Kernel32.VirtualProtect(addr, data.Length, old, out _);
        }

        public static ProcessModule GetModuleWhichBelongsToAddress(this IntPtr address)
        {
            ProcessModuleCollection modules = Process.GetCurrentProcess().Modules;
            foreach (ProcessModule module in modules)
            {
                if ((uint)address > (uint)module.BaseAddress && (uint)address < (uint)module.BaseAddress + module.ModuleMemorySize)
                {
                    return module;
                }
            }
            return null;
        }

        public static byte[] GetReturnToPtr(this IntPtr ptrToJumpTo)
        {
            // PUSH opcode http://ref.x86asm.net/coder32.html#x68
            List<byte> bytes = new List<byte> { 0x68 };

            // Push our hook address onto the stack
            byte[] hookPtrAddress = IntPtr.Size == 4 ? BitConverter.GetBytes(ptrToJumpTo.ToInt32()) : BitConverter.GetBytes(ptrToJumpTo.ToInt64());

            bytes.AddRange(hookPtrAddress);

            // RETN opcode http://ref.x86asm.net/coder32.html#xC3
            bytes.Add(0xC3);

            return bytes.ToArray();
        }
    }
}
