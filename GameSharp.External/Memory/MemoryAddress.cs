using GameSharp.Core.Memory;
using System;

namespace GameSharp.External.Memory
{
    public class MemoryAddress : IMemoryAddress
    {
        public IntPtr BaseAddress { get; }

        public MemoryAddress(IntPtr address)
        {
            BaseAddress = address;
        }

        public T Read<T>(int offset = 0) where T : struct
        {
            throw new NotImplementedException();
        }

        public T Read<T>(int size, int offset = 0)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
