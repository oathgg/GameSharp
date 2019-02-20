using System;
using System.Runtime.InteropServices;
using GameSharp.Extensions;
using GameSharp.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSharp.Tests
{
    [TestClass]
    public class TrampolineTests
    {
        [TestMethod]
        public void CreateTrampolineTest()
        {
            IntPtr originalMemoryPtr = Marshal.AllocHGlobal(1024);

            byte[] simpleFuncBytes = SimpleFunction();
            Marshal.Copy(simpleFuncBytes, 0, originalMemoryPtr, simpleFuncBytes.Length);

            byte[] trampFuncBytes = TrampolineFunction();
            Trampoline trampoline = new Trampoline(originalMemoryPtr, trampFuncBytes);
            trampoline.Enable();

            byte[] currentBytes = new byte[simpleFuncBytes.Length];
            Marshal.Copy(originalMemoryPtr, currentBytes, 0, currentBytes.Length);
        }

        [TestMethod]
        public void CreateJumpTest()
        {
            IntPtr originalLoc = Marshal.AllocHGlobal(1024);
            IntPtr trampLoc = Marshal.AllocHGlobal(1024);

            Trampoline trampoline = new Trampoline(originalLoc, TrampolineFunction());

            // Distance from the originalLoc to the trampLoc.
            byte[] jumpToTramp = trampoline.CreateJump(originalLoc, trampLoc);
            IntPtr ptrToTramp = Marshal.AllocHGlobal(jumpToTramp.Length);
            Marshal.Copy(jumpToTramp, 1, ptrToTramp, jumpToTramp.Length - 1);
            var resultToTramp = ptrToTramp.Read<int>();
            Assert.AreEqual((int)trampLoc - (int)originalLoc, resultToTramp);

            // Distance from the trampLoc to the original loc plus the extra 5 bytes.
            byte[] jumpToOrig = trampoline.CreateJump(trampLoc, originalLoc + 5);
            IntPtr ptrToOrig = Marshal.AllocHGlobal(jumpToOrig.Length);
            Marshal.Copy(jumpToOrig, 1, ptrToOrig, jumpToOrig.Length - 1);
            var resultToOrig = ptrToOrig.Read<int>();
            Assert.AreEqual((int)originalLoc - (int)trampLoc + 5, resultToOrig);
        }

        public byte[] SimpleFunction()
        {
            return new byte[]
            {
                0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, // NOPs
                0xC3 // RET
            };
        }

        public byte[] TrampolineFunction()
        {
            return new byte[]
            {
                0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC // INT 3
            };
        }
    }
}
