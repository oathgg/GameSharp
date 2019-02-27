using System;
using System.Runtime.InteropServices;
using GameSharp.Extensions;
using GameSharp.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSharp.Tests
{
    [TestClass]
    public class DetourTests
    {
        [TestMethod]
        public void CreateTrampolineTest()
        {
            IntPtr originalMemoryPtr = Marshal.AllocHGlobal(1024);

            byte[] simpleFuncBytes = SimpleFunction();
            Marshal.Copy(simpleFuncBytes, 0, originalMemoryPtr, simpleFuncBytes.Length);

            byte[] trampFuncBytes = TrampolineFunction();
            Detour trampoline = new Detour(originalMemoryPtr, trampFuncBytes);
            trampoline.Enable();

            byte[] currentBytes = new byte[simpleFuncBytes.Length];
            Marshal.Copy(originalMemoryPtr, currentBytes, 0, currentBytes.Length);
        }

        public byte[] SimpleFunction()
        {
            return new byte[]
            {
                0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, // NOPs
                0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, // NOPs
                0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, // NOPs
                0xC3 // RET
            };
        }

        public byte[] TrampolineFunction()
        {
            return new byte[]
            {
                0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, // INT 3
                0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, // INT 3
            };
        }
    }
}
