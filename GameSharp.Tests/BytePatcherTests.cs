using System;
using System.Runtime.InteropServices;
using GameSharp.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSharp.Tests
{
    [TestClass]
    public class BytePatcherTests
    {
        [TestMethod]
        public void EnableDisable()
        {
            // Allocate memory.
            IntPtr ptrAllocatedMemory = Utils.GenerateRandomMemoryBlock();
            
            byte[] originalBytes = Memory.Read<byte[]>(ptrAllocatedMemory, 3);
            byte[] newBytes = new byte[] { 1, 2, 3 };

            // Create the bytepatcher object.
            BytePatcher bp = new BytePatcher(ptrAllocatedMemory, newBytes);

            // Change bytes by enabling the patch.
            bp.Enable();

            // Validate if the byte has been changed.
            for (int i = 0; i < originalBytes.Length; i++)
                Assert.AreEqual(Memory.Read<byte[]>(ptrAllocatedMemory, 3)[i], newBytes[i]);

            // Disable the patch.
            bp.Disable();

            // Validate if the old byte is there.
            for (int i = 0; i < originalBytes.Length; i++)
                Assert.AreEqual(Memory.Read<byte[]>(ptrAllocatedMemory, 3)[i], originalBytes[i]);
        }
    }
}
