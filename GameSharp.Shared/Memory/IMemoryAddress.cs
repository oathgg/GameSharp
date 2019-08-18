using System;

namespace GameSharp.Core.Memory
{
    public interface IMemoryAddress
    {
        IntPtr BaseAddress { get; }

        T Read<T>(int size);
        T Read<T>() where T : struct;
        void Write();
        void Write(byte[] bytes);
    }
}
