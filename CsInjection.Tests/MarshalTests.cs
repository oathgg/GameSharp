using CsInjection.Core.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;

namespace CsInjection.Tests
{
    [TestClass]
    public class MarshalTests
    {
        [TestMethod]
        public void WriteBytesTest()
        {
            // Allocate unmanaged memory. 
            byte[] byteArray = new byte[] { 0x53, 0x61, 0x6e, 0x64, 0x53, 0x61, 0x6e, 0x64, 0x65, 0x72 };
            IntPtr unmanagedArrayPtr = Marshal.AllocHGlobal(byteArray.Length);

            // Call our function
            Kernel32.WriteProcessMemory(unmanagedArrayPtr, byteArray);

            // Read the bytes from the memory address
            for (int i = 0; i < byteArray.Length; i++)
            {
                Assert.AreEqual(byteArray[i], Marshal.ReadByte(unmanagedArrayPtr, i));
            }

            // Release the unmanaged memory.
            Marshal.FreeHGlobal(unmanagedArrayPtr);
        }
    }
}
