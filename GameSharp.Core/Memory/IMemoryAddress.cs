using System;

namespace GameSharp.Core.Memory
{
    public interface IMemoryAddress : IDisposable
    {
        IProcess Process { get; }
        IntPtr Address { get; }
        T Read<T>(int offset = 0) where T : struct;
        byte[] Read(int size, int offset = 0);
        void Write(bool value, int offset = 0);
        void Write(byte value, int offset = 0);
        void Write(long value, int offset = 0);
        void Write(IntPtr value, int offset = 0);
        void Write(IntPtr[] value, int offset = 0);
        void Write(byte[] value, int offset = 0);
    }
}
