using System;

namespace GameSharp.Core.Memory
{
    public interface IMemoryAddress
    {
        IProcess Process { get; }
        IntPtr Address { get; }
        T Read<T>(int offset = 0) where T : struct;
        byte[] Read(int size, int offset = 0);
        void Write(byte[] bytes);
    }
}
