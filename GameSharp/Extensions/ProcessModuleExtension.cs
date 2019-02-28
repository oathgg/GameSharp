using GameSharp.Native;
using GameSharp.Utilities;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameSharp.Extensions
{
    public static class ProcessModuleExtensions
    {
        /// <summary>
        ///     Wrapper for the PatternScanner.FindPattern utility.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static IntPtr FindPattern(this ProcessModule module, string pattern, int offset = 0)
        {
            return new PatternScanner(module).FindPattern(pattern, offset);
        }

        /// <summary>
        ///     Wrapper for the PatternScanner.FindPattern but after finding the pattern it will read the value for you.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="module"></param>
        /// <param name="pattern"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static T FindPattern<T>(this ProcessModule module, string pattern, int offset = 0)
        {
            IntPtr ptr = module.FindPattern(pattern, offset);
            return ptr.Read<T>(Marshal.SizeOf<T>());
        }

        /// <summary>
        ///     Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="moduleName">The module name (not case-sensitive).</param>
        /// <param name="functionName">The function or variable name, or the function's ordinal value.</param>
        /// <returns>The address of the exported function.</returns>
        public static IntPtr GetProcAddress(this ProcessModule module, string functionName)
        {
            // Get the function address
            var ret = Kernel32.GetProcAddress(module.BaseAddress, functionName);

            // Check whether the function was found
            if (ret != IntPtr.Zero)
                return ret;

            // Else the function was not found, throws an exception
            throw new Win32Exception($"Couldn't get the function address of {functionName}.");
        }

        /// <summary>
        ///     Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        /// </summary>
        /// <param name="libraryName">The name of the library to free (not case-sensitive).</param>
        public static void FreeLibrary(this ProcessModule module)
        {
            // Free the library
            if (!Kernel32.FreeLibrary(module.BaseAddress))
                throw new Win32Exception($"Couldn't free the library {module.ModuleName}.");
        }
    }
}
