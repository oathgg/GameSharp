using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Core.Native.PInvoke;
using GameSharp.Internal.Memory;
using PeNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace GameSharp.Internal.Module
{
    public class MemoryModule : ModuleBase
    {
        public PeFile PeHeader { get; }

        public override IMemoryAddress MemoryAddress { get; }

        public MemoryModule(ProcessModule processModule) : base(processModule)
        {
            MemoryAddress = new MemoryAddress(NativeProcessModule.BaseAddress);
            PeHeader = GeneratePeHeader();
        }

        private PeFile GeneratePeHeader()
        {
            PeFile header = new PeFile(MemoryAddress.Read(0x1000));

            return header;
        }

        /// <summary>
        ///     Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="moduleName">The module name (not case-sensitive).</param>
        /// <param name="functionName">The function or variable name, or the function's ordinal value.</param>
        /// <returns>The address of the exported function.</returns>
        public override IMemoryAddress GetProcAddress(string name)
        {
            MemoryAddress ret = new MemoryAddress(Kernel32.GetProcAddress(NativeProcessModule.BaseAddress, name));

            if (ret == null)
            {
                throw new Win32Exception($"Couldn't get the function address with name {name}.");
            }

            return ret;
        }

        /// <summary>
        ///     Keeps track of all code caves currently in use, even if there are no injected bytes.
        /// </summary>
        private static readonly Dictionary<ulong, uint> CodeCavesTaken = new Dictionary<ulong, uint>();

        /// <summary>
        ///     Get .text region from Module
        ///     Scan for bytes which are in range 0x00 - 0x10
        ///     Loop once byte has been found until size has been reached
        ///     Return pointer to the address
        /// </summary>
        /// <param name="module"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public MemoryAddress FindCodeCaveInModule(uint size)
        {
            uint baseOfCode = PeHeader.ImageNtHeaders.OptionalHeader.BaseOfCode;
            uint sizeOfCode = PeHeader.ImageNtHeaders.OptionalHeader.SizeOfCode;
            ulong codeSection = (ulong)NativeProcessModule.BaseAddress + baseOfCode;

            byte[] moduleBytes = MemoryAddress.Read((int)sizeOfCode, (int)baseOfCode);

            for (uint i = 0; i < moduleBytes.Length; i++)
            {
                if (moduleBytes[i] != 0x0)
                {
                    continue;
                }

                // If the codecave has already been taken, might still have bytes that are 0'd then we skip the size of the other codecave.
                CodeCavesTaken.TryGetValue(codeSection + i, out uint sizeTaken);
                if (sizeTaken > 0)
                {
                    i += sizeTaken;
                    continue;
                }

                for (uint j = 0; j <= size; j++)
                {
                    byte curByte = moduleBytes[i + j];
                    if (curByte == 0x0)
                    {
                        if (j == size)
                        {
                            ulong address = codeSection + i;

                            CodeCavesTaken.Add(address, size);

                            return new MemoryAddress((IntPtr)address);
                        }
                    }
                    // If we can't find a codecave big enough we will stop looping through the bytes but increment the (i) var
                    //  so we don't loop through bytes we already scanned.
                    else
                    {
                        i += j;
                        break;
                    }
                }
            }

            throw new NullReferenceException("Unable to find a codecave.");
        }
    }
}
