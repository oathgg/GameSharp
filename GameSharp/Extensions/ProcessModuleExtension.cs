using GameSharp.Native;
using GameSharp.Memory.Internal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;

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
        public static IntPtr FindPattern(this ProcessModule module, string pattern, int offset = 0) => new PatternScanner(module).FindPattern(pattern, offset);

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
            IntPtr ret = Kernel32.GetProcAddress(module.BaseAddress, functionName);

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

        /// <summary>
        ///     Keeps track of all code caves currently in use, even if there are no injected bytes.
        /// </summary>
        private static Dictionary<uint, uint> CodeCavesTaken = new Dictionary<uint, uint>();

        /// <summary>
        ///     Get .text region from Module
        ///     Scan for bytes which are in range 0x00 - 0x10
        ///     Loop once byte has been found until size has been reached
        ///     Return pointer to the address
        /// </summary>
        /// <param name="module"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IntPtr FindCodeCaveInModule(this ProcessModule module, uint size)
        {
            byte[] moduleBytes = module.BaseAddress.Read<byte[]>(module.ModuleMemorySize);

            for (uint i = 0x1000; i < moduleBytes.Length; i++)
            {
                if (moduleBytes[i] != 0x0)
                    continue;

                // If the codecave has already been taken (might still have bytes that are 0'd then we skip the size of the other codecave.
                CodeCavesTaken.TryGetValue((uint)module.BaseAddress + i, out uint sizeTaken);
                if (sizeTaken > 0)
                {
                    i += sizeTaken;
                    continue;
                }

                for (uint j = 0; j <= size; j++)
                {
                    if (moduleBytes[i + j] == 0x0)
                    {
                        // We found a codecave if we match the size of the bytes we wish to inject.
                        if (j == size)
                        {
                            CodeCavesTaken.Add((uint)module.BaseAddress + i, size);

                            return new IntPtr((uint)module.BaseAddress + i);
                        }
                    }
                    // If we can't find a codecave we will stop looping through the bytes
                    else
                    {
                        i += j;
                        break;
                    }
                }
            }

            return IntPtr.Zero;
        }
    }
}
