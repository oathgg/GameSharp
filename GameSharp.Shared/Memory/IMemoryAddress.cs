using System;

namespace GameSharp.Core.Memory
{
    public interface IMemoryAddress
    {
        IntPtr BaseAddress { get; }

        T Read<T>(int offset = 0) where T : struct;
        T Read<T>(int size, int offset = 0);
        void Write(byte[] bytes);
    }
}
