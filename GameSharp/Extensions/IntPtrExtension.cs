using GameSharp.Native;
using GameSharp.Utilities;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Extensions
{
    public static class IntPtrExtension
    {
        /// <summary>
        /// Gets an address from a vtable index. Since it uses index * IntPtr, it should work for both x64 and x32. 
        /// </summary>
        /// <param name="intPtr">The int PTR.</param>
        /// <param name="functionIndex">Index of the function.</param>
        /// <returns>IntPtr.</returns>
        public static IntPtr GetVtableIntPtr(this IntPtr intPtr, int functionIndex)
        {
            var vftable = intPtr.Read<IntPtr>();
            return (vftable + functionIndex * IntPtr.Size).Read<IntPtr>();
        }

        /// <summary>
        ///     Converts an unmanaged function pointer to the given delegate type.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="addr">Where address of the function.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        ///     This operation can only convert to delegates adorned with the
        ///     UnmanagedFunctionPointerAttribute
        /// </exception>
        /// TODO Edit XML Comment Template for ToDelegate`1
        public static T ToDelegate<T>(this IntPtr addr) where T : class
        {
            if (typeof(T).GetCustomAttributes(typeof(UnmanagedFunctionPointerAttribute), true).Length == 0)
                throw new InvalidOperationException(
                    "This operation can only convert to delegates adorned with the UnmanagedFunctionPointerAttribute");
            return Marshal.GetDelegateForFunctionPointer(addr, typeof(T)) as T;
        }

        public static T Read<T>(this IntPtr memoryAddress) where T : struct
        {
            return Marshal.PtrToStructure<T>(memoryAddress);
        }

        public static T Read<T>(this IntPtr addr, int size, int offset = 0)
        {
            byte[] destination = new byte[size];
            // Copy the memory to our own object
            Marshal.Copy(addr, destination, offset, destination.Length);
            return destination.Cast<T>();
        }

        public static void Write(this IntPtr addr, byte[] data)
        {
            // Update the memory section so we can write to it if not writeable.
            Kernel32.VirtualProtect(addr, data.Length, Enums.Protection.PAGE_EXECUTE_READWRITE, out Enums.Protection old);

            Marshal.Copy(data, 0, addr, data.Length);

            // Restore the page execution permissions.
            Kernel32.VirtualProtect(addr, data.Length, old, out Enums.Protection x);
        }

        public static void Write<T>(this IntPtr addr, T data)
        {
            Marshal.StructureToPtr(data, addr, false);
        }
    }
}
