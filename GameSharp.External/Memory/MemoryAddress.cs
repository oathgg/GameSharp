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

        public T Read<T>() where T : struct
        {
            throw new NotImplementedException();
        }

        public void Write()
        {
            throw new NotImplementedException();
        }

        public T Read<T>(int size)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
