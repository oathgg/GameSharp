using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsInjection.Core.Helpers;
using CsInjection.ManualMapInjection.Injection;
using System.Diagnostics;
using System.IO;

namespace CsInjection.Tests
{
    [TestClass]
    public class LibraryTests
    {
        [TestMethod]
        public void ConvertTest()
        {
            ConvertHelper.FromByteArray<int>(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }, out var iResult);
            Assert.AreEqual(-1, iResult);

            ConvertHelper.FromByteArray<string>(new byte[] { 0x53, 0x61, 0x6e, 0x64, 0x65, 0x72 }, out var sResult);
            Assert.AreEqual("Sander", sResult);

            ConvertHelper.FromByteArray<byte[]>(new byte[] { 0x53, 0x61, 0x6E, 0x64, 0x65, 0x72 }, out var bResult);
            Assert.AreEqual((byte) 0x6E, bResult[2]);

            ConvertHelper.FromByteArray<bool>(new byte[] { 0x01 }, out var boolResult);
            Assert.IsTrue(boolResult);
        }

        [TestMethod]
        public void CrashTest()
        {
            Process targetProcess = Process.Start(@"C:\Program Files (x86)\Notepad++\notepad++.exe");
            ManualMapInjector injector = new ManualMapInjector(targetProcess);
            FileInfo fileInfo = new FileInfo(@"CsInjection.Cpp.Bootstrap.dll");
            injector.Inject(fileInfo.FullName);
        }
    }
}
