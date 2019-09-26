using GameSharp.Core;
using GameSharp.Core.Extensions;
using GameSharp.Core.Memory;
using GameSharp.Core.Native.PInvoke;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.External.Memory
{
    public class MemoryAddress : IMemoryAddress
    {
        public IntPtr Address { get; }

        public IProcess Process { get; }

        public MemoryAddress(GameSharpProcess process, IntPtr address)
        {
            Address = address;
            Process = process as IProcess;
        }

        public T Read<T>(int offset = 0) where T : struct
        {
            byte[] result = new byte[Marshal.SizeOf<T>()];

            Kernel32.ReadProcessMemory(Process.NativeProcess.Handle, Address + offset, result, result.Length, out IntPtr _);

            return result.CastTo<T>(); // [0] would be faster, but First() is safer. E.g. of buffer[0] ?? default(T)
        }

        public byte[] Read(int size, int offset = 0)
        {
            byte[] result = new byte[size];

            Kernel32.ReadProcessMemory(Process.NativeProcess.Handle, Address + offset, result, result.Length, out IntPtr _);

            return result;
        }

        public void Write(byte[] bytes)
        {
            Kernel32.WriteProcessMemory(Process.NativeProcess.Handle, Address, bytes, bytes.Length, out IntPtr _);
        }
    }
}
