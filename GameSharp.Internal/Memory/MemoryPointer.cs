using GameSharp.Core;
using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.PInvoke;
using GameSharp.Internal.Module;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameSharp.Internal.Memory
{
    public class MemoryPointer : IMemoryPointer
    {
        public IntPtr Address { get; }
        public IProcess GameSharpProcess => Internal.GameSharpProcess.Instance;
        public bool IsValid => Address != IntPtr.Zero;

        public MemoryPointer(IntPtr address)
        {
            Address = address;
        }

        public InternalModulePointer GetMyModule()
        {
            Dictionary<string, ModulePointer>.Enumerator modules = Internal.GameSharpProcess.Instance.Modules.GetEnumerator();
            while (modules.MoveNext())
            {
                InternalModulePointer module = modules.Current.Value as InternalModulePointer;

                // Address has to be between the start address of the module and the end address of the module.
                if (Address.ToInt64() > module.Address.ToInt64()
                    && Address.ToInt64() < module.Address.ToInt64() + module.Size)
                {
                    return module;
                }
            }

            return null;
        }

        public static MemoryPointer AllocateMemory(int size)
        {
            return new MemoryPointer(Marshal.AllocHGlobal(size));
        }

        public T Read<T>(int offset = 0) where T : struct
        {
            T result = Marshal.PtrToStructure<T>(Address + offset);

            return result;
        }

        public byte[] Read(int size, int offset = 0)
        {
            byte[] destination = new byte[size];

            Marshal.Copy(Address + offset, destination, 0, destination.Length);

            return destination;
        }

        public void Write(byte[] data, int offset = 0)
        {
            Kernel32.VirtualProtect(Address + offset, data.Length, MemoryProtection.ExecuteReadWrite, out MemoryProtection old);
            Marshal.Copy(data, 0, Address + offset, data.Length);
            Kernel32.VirtualProtect(Address + offset, data.Length, old, out MemoryProtection _);
        }

        public void Write(bool value, int offset = 0)
        {
            byte[] bArray = BitConverter.GetBytes(value);
            Write(bArray, offset);
        }

        public void Write(byte value, int offset = 0)
        {
            byte[] bArray = BitConverter.GetBytes(value);
            Write(bArray, offset);
        }

        public void Write(long value, int offset = 0)
        {
            byte[] bArray = BitConverter.GetBytes(value);
            Write(bArray, offset);
        }

        public void Write(IntPtr value, int offset = 0)
        {
            byte[] bArray;

            if (GameSharpProcess.Is64Bit)
            {
                bArray = BitConverter.GetBytes(value.ToInt64());
            }
            else
            {
                bArray = BitConverter.GetBytes(value.ToInt32());
            }

            Write(bArray, offset);
        }

        public void Write(IntPtr[] value, int offset = 0)
        {
            for (int i = 0; i < value.Length; i++)
            {
                Write(value[i], IntPtr.Size * i);
            }
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(Address);
        }

        public override string ToString()
        {
            return $"0x{Address.ToString("X")}";
        }


    }
}
