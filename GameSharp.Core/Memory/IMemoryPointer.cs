using System;

namespace GameSharp.Core.Memory
{
    public interface IMemoryPointer : IDisposable
    {
        IProcess GameSharpProcess { get; }
        IntPtr Address { get; }
        T Read<T>(int offset = 0) where T : struct;
        bool IsValid { get; }
        byte[] Read(int size, int offset = 0);
        void Write(bool value, int offset = 0);
        void Write(byte value, int offset = 0);
        void Write(long value, int offset = 0);
        void Write(IntPtr value, int offset = 0);
        void Write(IntPtr[] value, int offset = 0);
        void Write(byte[] value, int offset = 0);
    }
}
