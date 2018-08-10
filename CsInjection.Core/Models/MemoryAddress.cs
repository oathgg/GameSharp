using CsInjection.Core.Native;
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
            return Memory.ReadProcessMemory<T>(Address + offset, size);
        }

        public void Write(byte[] newBytes)
        {
            Memory.WriteProcessMemory(Address, newBytes);
        }
    }
}