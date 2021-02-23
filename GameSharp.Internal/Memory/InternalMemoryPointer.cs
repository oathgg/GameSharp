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
    public class InternalMemoryPointer : MemoryPointer
    {
        public override IntPtr Address { get; }
        public override IProcess GameSharpProcess => Internal.GameSharpProcess.Instance;
        public override bool IsValid => Address != IntPtr.Zero;

        public InternalMemoryPointer(IntPtr address)
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

        public static InternalMemoryPointer AllocateMemory(int size)
        {
            return new InternalMemoryPointer(Marshal.AllocHGlobal(size));
        }

        public override T Read<T>(int offset = 0)
        {
            T result = Marshal.PtrToStructure<T>(Address + offset);

            return result;
        }

        public override byte[] Read(int size, int offset = 0)
        {
            byte[] destination = new byte[size];

            Marshal.Copy(Address + offset, destination, 0, destination.Length);

            return destination;
        }

        public override void Write(byte[] data, int offset = 0)
        {
            Kernel32.VirtualProtect(Address + offset, data.Length, MemoryProtection.ExecuteReadWrite, out MemoryProtection old);
            Marshal.Copy(data, 0, Address + offset, data.Length);
            Kernel32.VirtualProtect(Address + offset, data.Length, old, out MemoryProtection _);
        }

        public override void Write(bool value, int offset = 0)
        {
            byte[] bArray = BitConverter.GetBytes(value);
            Write(bArray, offset);
        }

        public override void Write(byte value, int offset = 0)
        {
            byte[] bArray = BitConverter.GetBytes(value);
            Write(bArray, offset);
        }

        public override void Write(long value, int offset = 0)
        {
            byte[] bArray = BitConverter.GetBytes(value);
            Write(bArray, offset);
        }

        public override void Write(IntPtr value, int offset = 0)
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

        public override void Write(IntPtr[] value, int offset = 0)
        {
            for (int i = 0; i < value.Length; i++)
            {
                Write(value[i], IntPtr.Size * i);
            }
        }

        public override void Dispose()
        {
            Marshal.FreeHGlobal(Address);
        }
    }
}
