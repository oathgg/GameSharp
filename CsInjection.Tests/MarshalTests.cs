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
        public void ReadWriteBytesTest()
        {
            // Allocate unmanaged memory. 
            byte[] byteArray = new byte[] { 0x53, 0x61, 0x6e, 0x64, 0x53, 0x61, 0x6e, 0x64, 0x65, 0x72 };
            IntPtr unmanagedArrayPtr = Marshal.AllocHGlobal(byteArray.Length);

            // Call our function
            Memory.WriteProcessMemory(unmanagedArrayPtr, byteArray);

            // Read the bytes from the memory address
            byte[] result = Memory.ReadProcessMemory<byte[]>(unmanagedArrayPtr, byteArray.Length);
            Assert.AreEqual(byteArray[5], result[5]);

            // Release the unmanaged memory.
            Marshal.FreeHGlobal(unmanagedArrayPtr);
        }

        [TestMethod]
        public void ReadWriteBytesPressureTest()
        {
            Random rnd = new Random();
            int testCount = 10000;
            for(int i = 0; i < testCount; i++)
            {
                byte[] bArray = new byte[16];
                rnd.NextBytes(bArray);

                IntPtr unmanagedArrayPtr = Marshal.AllocHGlobal(bArray.Length);
                Memory.WriteProcessMemory(unmanagedArrayPtr, bArray);

                // Read the bytes from the memory address
                byte[] result = Memory.ReadProcessMemory<byte[]>(unmanagedArrayPtr, bArray.Length);
                Assert.AreEqual(bArray[5], result[5]);

                // Release the unmanaged memory.
                Marshal.FreeHGlobal(unmanagedArrayPtr);
            }
        }

        [TestMethod]
        public void GetStructFromMemoryPressureTest()
        {
            Random rnd = new Random();
            int testCount = 10000;
            for (int i = 0; i < testCount; i++)
            {
                byte[] bArray = new byte[16];
                rnd.NextBytes(bArray);

                IntPtr unmanagedArrayPtr = Marshal.AllocHGlobal(bArray.Length);
                Memory.WriteProcessMemory(unmanagedArrayPtr, bArray);

                // Read the bytes from the memory address
                TestStruct result = Memory.ReadProcessMemory<TestStruct>(unmanagedArrayPtr);
                Assert.AreEqual(bArray[5], result.bArray[5]);

                // Release the unmanaged memory.
                Marshal.FreeHGlobal(unmanagedArrayPtr);
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct TestStruct
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] bArray;
        }
    }
}
