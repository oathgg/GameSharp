using System;
using GameSharp.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSharp.Tests
{
    [TestClass]
    public class ArrayExtensionTests
    {
        [TestMethod]
        public void CastTest()
        {
            int iResult = (new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }).Cast<int>();
            Assert.AreEqual(-1, iResult);

            string sResult = (new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64 }).Cast<string>();
            Assert.AreEqual("Hello World", sResult);

            byte[] bResult = (new byte[] { 0x53, 0x61, 0x6E, 0x64, 0x65, 0x72 }).Cast<byte[]>();
            Assert.AreEqual(0x6E, bResult[2]);

            bool boolResult = (new byte[] { 0x01 }).Cast<bool>();
            Assert.IsTrue(boolResult);
        }
    }
}
