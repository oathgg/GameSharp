using System;

namespace GameSharp.Core.Memory
{
    interface IMemoryPointer : IDisposable
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

    public abstract class MemoryPointer : IMemoryPointer
    {
        public virtual IProcess GameSharpProcess => throw new NotImplementedException();

        public virtual IntPtr Address => throw new NotImplementedException();

        public virtual bool IsValid => throw new NotImplementedException();

        public abstract void Dispose();

        public abstract T Read<T>(int offset = 0) where T : struct;

        public abstract byte[] Read(int size, int offset = 0);

        public abstract void Write(bool value, int offset = 0);

        public abstract void Write(byte value, int offset = 0);

        public abstract void Write(long value, int offset = 0);

        public abstract void Write(IntPtr value, int offset = 0);

        public abstract void Write(IntPtr[] value, int offset = 0);

        public abstract void Write(byte[] value, int offset = 0);

        public override string ToString()
        {
            return $"0x{Address.ToString("X")}";
        }
    }
}
