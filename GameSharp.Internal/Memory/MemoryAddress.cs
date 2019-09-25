using GameSharp.Core;
using GameSharp.Core.Memory;
using GameSharp.Core.Module;
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

        public IProcess Process => GameSharpProcess.Instance;

        public MemoryAddress(IntPtr address)
        {
            BaseAddress = address;
        }

        public T Read<T>(int offset = 0) where T : struct
        {
            T result = Marshal.PtrToStructure<T>(BaseAddress + offset);

            return result;
        }

        public byte[] Read(int size, int offset = 0)
        {
            byte[] destination = new byte[size];

            Kernel32.VirtualProtect(BaseAddress, size, MemoryProtection.WriteCopy, out MemoryProtection old);

            Marshal.Copy(BaseAddress, destination, offset, destination.Length);

            Kernel32.VirtualProtect(BaseAddress, size, old, out _);

            return destination;
        }

        public void Write(byte[] data)
        {
            // Make sure we have Write access to the page.
            Kernel32.VirtualProtect(BaseAddress, data.Length, MemoryProtection.WriteCopy, out MemoryProtection old);
            Marshal.Copy(data, 0, BaseAddress, data.Length);
        }

        public MemoryModule GetMyModule()
        {
            Dictionary<string, IMemoryModule>.Enumerator modules = GameSharpProcess.Instance.Modules.GetEnumerator();
            while (modules.MoveNext())
            {
                MemoryModule module = modules.Current.Value as MemoryModule;

                // Address has to be between the start address of the module and the end address of the module.
                if (BaseAddress.ToInt64() > module.BaseAddress.ToInt64()
                    && BaseAddress.ToInt64() < module.BaseAddress.ToInt64() + module.ModuleMemorySize)
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
