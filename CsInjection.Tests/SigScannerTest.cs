using CsInjection.Core.Models;
using CsInjection.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace CsInjection.Tests
{
    [TestClass]
    public class SigScannerTest
    {
        [TestMethod]
        public void PatternCheckTest()
        {
            //byte[] byteArray = new byte[] 
            //{
            //    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            //    // HELLO WRRLD
            //    0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x77, 0x72, 0x72, 0x6C, 0x64,
            //    // HELLO WORLD
            //    0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x77, 0x6F, 0x72, 0x6C, 0x64,
            //    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            //};

            //Process proc = Process.GetCurrentProcess();
            //SigScanner sigScanner = new SigScanner(proc.MainModule);
            //sigScanner.ModuleBytes = byteArray;

            //MemoryAddress result = sigScanner.FindPattern("48 65 6c 6c 6f ? 77 6f 72 6c 64");
            //Assert.IsNotNull(result);
        }
    }
}
