using GameSharp.Extensions;
using GameSharp.Interoperability;
using GameSharp.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Tests
{
    [TestClass]
    public class BytePatcherTests
    {
        [TestMethod]
        public void EnableDisableTest()
        {
            byte[] byteArray = Utils.GenerateByteArray(1024);

            // Allocate memory.
            InternalIntPtr ptrAllocatedMemory = new InternalIntPtr(Marshal.AllocHGlobal(byteArray.Length));

            byte[] originalBytes = ptrAllocatedMemory.Read<byte[]>(3);
            byte[] newBytes = new byte[] { 1, 2, 3 };

            // Create the bytepatcher object.
            Patch bp = new Patch(ptrAllocatedMemory, newBytes);

            // Change bytes by enabling the patch.
            bp.Enable();

            // Validate if the byte has been changed.
            byte[] retrievedBytes = ptrAllocatedMemory.Read<byte[]>(3);
            for (int i = 0; i < originalBytes.Length; i++)
                Assert.AreEqual(retrievedBytes[i], newBytes[i]);

            // Disable the patch.
            bp.Disable();

            // Validate if the old byte is there.
            retrievedBytes = ptrAllocatedMemory.Read<byte[]>(3);
            for (int i = 0; i < originalBytes.Length; i++)
                Assert.AreEqual(retrievedBytes[i], originalBytes[i]);
        }
    }
}
