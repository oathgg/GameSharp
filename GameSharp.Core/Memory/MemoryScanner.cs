using GameSharp.Core.Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameSharp.Core.Memory
{
    public class MemoryScanner
    {
        /// <summary>
        ///     Contains all the bytes of the specified module.
        /// </summary>
        private byte[] Bytes { get; set; }

        /// <summary>
        ///     The base address of the module.
        /// </summary>
        private IMemoryPointer ModuleBase { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryScanner" /> class.
        /// </summary>
        /// <param name="module"><see cref="ProcessModule"/> which we are going to scan.</param>
        public MemoryScanner(IModulePointer module)
        {
            ModuleBase = module.MemoryAddress;
            Bytes = ModuleBase.Read(module.ModuleMemorySize);
        }

        public MemoryScanner(byte[] bytesToScan)
        {
            Bytes = bytesToScan;
        }

        private IntPtr FindByteAddress(byte[] array, int offset = 0)
        {
            for (int memByteOffset = 0; memByteOffset < Bytes.Length; memByteOffset++)
            {
                if (Bytes[memByteOffset] != array[0])
                {
                    continue;
                }

                if (PatternCheck(ref memByteOffset, array))
                {
                    return ModuleBase.Address
                        // offset in byte array
                        + memByteOffset
                        // offset given by user
                        + int.Parse(offset.ToString("X"), System.Globalization.NumberStyles.HexNumber);
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        ///     Find a pattern that matches the string.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public IntPtr FindPattern(string pattern, int offset = 0)
        {
            byte[] arrPattern = ParsePatternString(pattern.Trim());

            return FindByteAddress(arrPattern, offset);
        }

        /// <summary>
        ///     Parses the pattern and changes the values to byte array understandable logic.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private byte[] ParsePatternString(string pattern)
        {
            List<byte> patternbytes = new List<byte>();

            foreach (string curByte in pattern.Split(' '))
            {
                // when we have a ? it's a variable, otherwise convert it to a byte.
                patternbytes.Add(curByte == "?" ? (byte)0x0 : Convert.ToByte(curByte, 16));
            }

            return patternbytes.ToArray();
        }

        /// <summary>
        ///     Checks the values if they match with the provided pattern.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private bool PatternCheck(ref int index, byte[] pattern)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                // Skip this byte in case its a variable.
                if (pattern[i] == 0x0)
                {
                    continue;
                }

                if (pattern[i] != Bytes[index + i])
                {
                    // Increase the index with the i we stopped at so we don't repeat scanning those bytes again.
                    index += i;
                    return false;
                }
            }

            // Pattern has been found.
            return true;
        }
    }
}