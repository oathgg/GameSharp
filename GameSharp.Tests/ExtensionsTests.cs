using GameSharp.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GameSharp.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void CastTest()
        {
            int iResult = (new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }).CastTo<int>();
            Assert.AreEqual(-1, iResult);

            string sResult = (new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64 }).CastToString(Encoding.Unicode);
            Assert.AreEqual("Hello World", sResult);

            byte[] bResult = (new byte[] { 0x53, 0x61, 0x6E, 0x64, 0x65, 0x72 }).CastTo<byte[]>();
            Assert.AreEqual(0x6E, bResult[2]);

            bool boolResult = (new byte[] { 0x01 }).CastTo<bool>();
            Assert.IsTrue(boolResult);
        }

        [TestMethod]
        public void ReadWriteTest()
        {
            byte[] byteArray = Utils.GenerateByteArray(1024);

            IntPtr unmanagedArrayPtr = Marshal.AllocHGlobal(byteArray.Length);

            // Call our function
            unmanagedArrayPtr.Write(byteArray);

            // Read the bytes from the memory address
            byte[] result = unmanagedArrayPtr.Read<byte[]>(byteArray.Length);
            Assert.AreEqual(byteArray[5], result[5]);

            // Release the unmanaged memory.
            Marshal.FreeHGlobal(unmanagedArrayPtr);
        }

        [TestMethod]
        public void ReadStructTest()
        {
            byte[] byteArray = Utils.GenerateByteArray(Marshal.SizeOf<TestStruct>());

            IntPtr unmanagedArrayPtr = Marshal.AllocHGlobal(byteArray.Length);

            // Write our bytes to the newly allocated memory
            unmanagedArrayPtr.Write(byteArray);

            // Read the bytes from the memory address
            TestStruct result = unmanagedArrayPtr.Read<TestStruct>();
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
