using System;
using System.Runtime.InteropServices;
using GameSharp.Native;
using GameSharp.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSharp.Tests
{
    [TestClass]
    public class MemoryTests
    {
        // Allocate unmanaged memory. 
        byte[] byteArray = new byte[] { 0x53, 0x61, 0x6e, 0x64, 0x53, 0x61, 0x6e, 0x64, 0x65, 0x72 };

        [TestMethod]
        public void ReadWriteTest()
        {
            IntPtr unmanagedArrayPtr = Marshal.AllocHGlobal(byteArray.Length);

            // Call our function
            Memory.Write(unmanagedArrayPtr, byteArray);

            // Read the bytes from the memory address
            byte[] result = Memory.Read<byte[]>(unmanagedArrayPtr, byteArray.Length);
            Assert.AreEqual(byteArray[5], result[5]);

            // Release the unmanaged memory.
            Marshal.FreeHGlobal(unmanagedArrayPtr);
        }

        [TestMethod]
        public void ReadStructTest()
        {
            IntPtr unmanagedArrayPtr = Marshal.AllocHGlobal(byteArray.Length);

            // Write our bytes to the newly allocated memory
            Memory.Write(unmanagedArrayPtr, byteArray);

            // Read the bytes from the memory address
            TestStruct result = Memory.Read<TestStruct>(unmanagedArrayPtr);
            Assert.AreEqual(byteArray[5], result.bArray[5]);

            // Release the unmanaged memory.
            Marshal.FreeHGlobal(unmanagedArrayPtr);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct TestStruct
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] bArray;
        }
    }
}
