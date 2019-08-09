using System;
using GameSharp.Memory.External;
using GameSharp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSharp.Tests
{
    [TestClass]
    public class SigScannerTests
    {
        [TestMethod]
        public void FindByteAddressTest()
        {
            byte[] byteArray = Utils.GenerateByteArray(1000000);
            byte[] pattern = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x77, 0x6F, 0x72, 0x6C, 0x64 };

            int rndMemLocation = new Random().Next(byteArray.Length);
            for (int i = 0; i < pattern.Length; i++)
            {
                byteArray[rndMemLocation + i] = pattern[i];
            }

            PatternScanner sigScanner = new PatternScanner(byteArray);
            IntPtr result = sigScanner.FindPattern("48 65 6c 6c 6f ? 77 6f 72 6c 64");
            Assert.AreEqual(rndMemLocation, result.ToInt32());
        }
    }
}
