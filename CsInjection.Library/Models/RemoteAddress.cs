using System;
using System.Diagnostics;
using CsInjection.Library.Helpers;

namespace CsInjection.Library.Models
{
    public class MemoryAddress
    {
        public IntPtr Address { get; private set; }
        public MemoryAddress(IntPtr address)
        {
            Address = address;
        }

        public T Read<T>(int offset, int size)
        {
            return Native.ReadFromMemory<T>(Address + offset, size);
        }

        public void Write(byte[] newBytes)
        {
            Native.WriteToMemory(Address, newBytes);
        }
    }
}
