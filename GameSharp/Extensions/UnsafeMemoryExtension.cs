using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Extensions
{
    /// <summary>
    /// https://github.com/lolp1/Process.NET/blob/fbe6fd3f865dc0e2e2f62bb5dfc41a423952aebc/src/Process.NET/Extensions/UnsafeMemoryExtensions.cs
    /// </summary>
    public static class UnsafeMemoryExtension
    {
        /// <summary>
        /// Gets an address from a vtable index. Since it uses index * IntPtr, it should work for both x64 and x32. 
        /// </summary>
        /// <param name="intPtr">The int PTR.</param>
        /// <param name="functionIndex">Index of the function.</param>
        /// <returns>IntPtr.</returns>
        public static IntPtr GetVtableIntPtr(this IntPtr intPtr, int functionIndex)
        {
            IntPtr vftable = intPtr.Read<IntPtr>();
            return (vftable + functionIndex * IntPtr.Size).Read<IntPtr>();
        }

        /// <summary>
        ///     Converts an unmanaged delegate to a function pointer.
        /// </summary>
        public static IntPtr ToFunctionPtr(this Delegate d)
        {
            return Marshal.GetFunctionPointerForDelegate(d);
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
    }
}
