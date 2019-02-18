using System;
using GameSharp.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSharp.Tests
{
    [TestClass]
    public class ByteArrayExtensionTests
    {
        [TestMethod]
        public void CastTest()
        {
            int iResult = (new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }).Cast<int>();
            Assert.AreEqual(-1, iResult);

            string sResult = (new byte[] { 0x53, 0x61, 0x6e, 0x64, 0x65, 0x72 }).Cast<string>();
            Assert.AreEqual("Sander", sResult);

            byte[] bResult = (new byte[] { 0x53, 0x61, 0x6E, 0x64, 0x65, 0x72 }).Cast<byte[]>();
            Assert.AreEqual(0x6E, bResult[2]);

            bool boolResult = (new byte[] { 0x01 }).Cast<bool>();
            Assert.IsTrue(boolResult);
        }
    }
}
