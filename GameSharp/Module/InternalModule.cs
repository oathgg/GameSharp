using GameSharp.Extensions;
using GameSharp.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace GameSharp.Module
{
    public class InternalModule : ModuleBase
    {
        public PeNet.PeFile PeHeader { get; }

        public InternalModule(ProcessModule processModule) : base(processModule)
        {
            PeHeader = GeneratePeHeader();
        }

        private PeNet.PeFile GeneratePeHeader()
        {
            PeNet.PeFile header = new PeNet.PeFile(ProcessModule.BaseAddress.Read<byte[]>(ProcessModule.ModuleMemorySize));

            return header;
        }

        /// <summary>
        ///     Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="moduleName">The module name (not case-sensitive).</param>
        /// <param name="functionName">The function or variable name, or the function's ordinal value.</param>
        /// <returns>The address of the exported function.</returns>
        public override IntPtr GetProcAddress(string functionName)
        {
            IntPtr ret = Kernel32.GetProcAddress(ProcessModule.BaseAddress, functionName);

            if (ret == IntPtr.Zero)
                throw new Win32Exception($"Couldn't get the function address with name {functionName}.");

            return ret;
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
        public IntPtr FindCodeCaveInModule(uint size)
        {
            byte[] moduleBytes = ProcessModule.BaseAddress.Read<byte[]>(ProcessModule.ModuleMemorySize);

            for (uint i = 0x1000; i < moduleBytes.Length; i++)
            {
                if (moduleBytes[i] != 0x0)
                    continue;

                // If the codecave has already been taken (might still have bytes that are 0'd then we skip the size of the other codecave.
                CodeCavesTaken.TryGetValue((uint)ProcessModule.BaseAddress + i, out uint sizeTaken);
                if (sizeTaken > 0)
                {
                    i += sizeTaken;
                    continue;
                }

                for (uint j = 0; j <= size; j++)
                {
                    if (moduleBytes[i + j] == 0x0)
                    {
                        if (j == size)
                        {
                            CodeCavesTaken.Add((uint)ProcessModule.BaseAddress + i, size);

                            return new IntPtr((uint)ProcessModule.BaseAddress + i);
                        }
                    }
                    // If we can't find a codecave big enough we will stop looping through the bytes
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
