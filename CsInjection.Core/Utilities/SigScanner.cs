using System;
using System.Collections.Generic;
using System.Diagnostics;
using CsInjection.Core.Models;
using CsInjection.Core.Native;

namespace CsInjection.Core.Utilities
{
    public class SigScanner
    {
        /// <summary>
        ///     Contains all the bytes of the specified module.
        /// </summary>
        private byte[] ModuleBytes { get; set; }

        /// <summary>
        ///     The <see cref="ProcessModule"/> that has been selected to scan for patterns.
        /// </summary>
        private ProcessModule _selectedModule { get; }

        /// <summary>
        ///     The base address of the module.
        /// </summary>
        private MemoryAddress _moduleBase { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SigScanner" /> class.
        /// </summary>
        /// <param name="module"><see cref="ProcessModule"/> which we are going to scan.</param>
        public SigScanner(ProcessModule module)
        {
            _selectedModule = module;
            _moduleBase = new MemoryAddress(module.BaseAddress);
            ModuleBytes = _moduleBase.Read<byte[]>(module.ModuleMemorySize);
        }

        /// <summary>
        ///     Find a pattern that matches the string.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public MemoryAddress FindPattern(string pattern, int offset = 0)
        {
            byte[] arrPattern = ParsePatternString(pattern);

            for (int index = 0; index < ModuleBytes.Length; index++)
            {
                if (ModuleBytes[index] != arrPattern[0])
                    continue;

                if (PatternCheck(index, arrPattern, out index))
                {
                    return new MemoryAddress(_moduleBase.Address + index + int.Parse(offset.ToString("X"), System.Globalization.NumberStyles.HexNumber));
                }
            }

            return null;
        }

        /// <summary>
        ///     Parses the pattern and changes the values to byte array understandable logic.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private byte[] ParsePatternString(string pattern)
        {
            List<byte> patternbytes = new List<byte>();

            foreach (var curByte in pattern.Split(' '))
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
        private bool PatternCheck(int index, byte[] pattern, out int newIndex)
        {
            newIndex = index;

            for (int i = 0; i < pattern.Length; i++)
            {
                // Skip this byte in case its a variable.
                if (pattern[i] == 0x0)
                    continue;

                if (pattern[i] != ModuleBytes[index + i])
                {
                    // Increase the index with the i we stopped at so we don't repeat scanning those bytes again.
                    newIndex += i;
                    return false;
                }
            }

            return true;
        }
    }
}
