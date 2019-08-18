using GameSharp.Shared.Interfaces;
using System;

namespace GameSharp.Internal.Memory
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
    }
}
