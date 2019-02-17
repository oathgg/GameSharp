using System;
using System.Text;
using GameSharp.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSharp.Tests
{
    [TestClass]
    public class SigScannerTests
    {
        [TestMethod]
        public void FindByteAddressTest()
        {
            int maxBytes = 1024;

            // Create a byte array we wish to scan.
            byte[] bytesToScan = new byte[maxBytes];

            // Fill it with random bytes.
            new Random().NextBytes(bytesToScan);

            // Hello World.
            byte[] helloWorldByte = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64 };

            // Pass the Byte string into a random spot of the byte array object.
            int rndSpot = new Random().Next(0, maxBytes);
            for (int i = 0; i < helloWorldByte.Length; i++)
                bytesToScan[rndSpot + i] = helloWorldByte[i];

            // Scan if we can find the pattern.
            SigScanner sc = new SigScanner(bytesToScan);
            IntPtr intPtr = sc.FindPattern("48 65 ? ? 6F 20 57 6F 72 ? 64");

            // Needs to be the same Entry point.
            Assert.AreEqual(rndSpot, (int)intPtr);
        }
    }
}
