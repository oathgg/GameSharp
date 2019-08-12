using GameSharp.Memory;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Extensions
{
    /// <summary>
    /// https://github.com/lolp1/Process.NET/blob/fbe6fd3f865dc0e2e2f62bb5dfc41a423952aebc/src/Process.NET/Extensions/UnsafeMemoryExtensions.cs
    /// </summary>
    public static class UnsafeMemoryExtension
    {
        public static UnmanagedMemory ToFunctionPtr(this Delegate d)
        {
            return new UnmanagedMemory(Marshal.GetFunctionPointerForDelegate(d));
        }

        public static T ToDelegate<T>(this IntPtr addr) where T : class
        {
            if (typeof(T).GetCustomAttributes(typeof(UnmanagedFunctionPointerAttribute), true).Length == 0)
                throw new InvalidOperationException("This operation can only convert to delegates adorned with the UnmanagedFunctionPointerAttribute");

            return Marshal.GetDelegateForFunctionPointer(addr, typeof(T)) as T;
        }
    }
}
