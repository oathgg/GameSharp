using GameSharp.Core;
using GameSharp.Core.Extensions;
using GameSharp.Core.Memory;
using GameSharp.Core.Native.PInvoke;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.External.Memory
{
    public class MemoryPointer : IMemoryPointer
    {
        public IntPtr Address { get; }

        public IProcess GameSharpProcess { get; }

        public MemoryPointer(GameSharpProcess process, IntPtr address)
        {
            Address = address;
            GameSharpProcess = process as IProcess;
        }

        public T Read<T>(int offset = 0) where T : struct
        {
            byte[] result = new byte[Marshal.SizeOf<T>()];

            Kernel32.ReadProcessMemory(GameSharpProcess.Native.Handle, Address + offset, result, result.Length, out IntPtr _);

            return result.CastTo<T>(); // [0] would be faster, but First() is safer. E.g. of buffer[0] ?? default(T)
        }

        public byte[] Read(int size, int offset = 0)
        {
            byte[] result = new byte[size];

            Kernel32.ReadProcessMemory(GameSharpProcess.Native.Handle, Address + offset, result, result.Length, out IntPtr _);

            return result;
        }

        public void Write(byte[] bytes, int offset = 0)
        {
            Kernel32.WriteProcessMemory(GameSharpProcess.Native.Handle, Address + offset, bytes, bytes.Length, out IntPtr _);
        }

        public void Write(bool value, int offset = 0)
        {
            byte[] bArray = BitConverter.GetBytes(value);
            Write(bArray, offset);
        }

        public void Write(byte value, int offset = 0)
        {
            byte[] bArray = BitConverter.GetBytes(value);
            Write(bArray, offset);
        }

        public void Write(long value, int offset = 0)
        {
            byte[] bArray = BitConverter.GetBytes(value);
            Write(bArray, offset);
        }

        public void Write(IntPtr value, int offset = 0)
        {
            byte[] bArray;

            if (GameSharpProcess.Is64Bit)
            {
                bArray = BitConverter.GetBytes(value.ToInt64());
            }
            else
            {
                bArray = BitConverter.GetBytes(value.ToInt32());
            }

            Write(bArray, offset);
        }

        public void Write(IntPtr[] value, int offset = 0)
        {
            for (int i = 0; i < value.Length; i++)
            {
                Write(value[i], IntPtr.Size * i);
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
