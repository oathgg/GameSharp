using System;

namespace CsInjection.Core.Models
{
    public class MemoryAddress
    {
        public IntPtr Address { get; private set; }
        public MemoryAddress(IntPtr address)
        {
            Address = address;
        }

        public T Read<T>(int size, int offset = 0)
        {
            return NativeAPI.ReadFromMemory<T>(Address + offset, size);
        }

        public void Write(byte[] newBytes)
        {
            NativeAPI.WriteToMemory(Address, newBytes);
        }
    }
}
