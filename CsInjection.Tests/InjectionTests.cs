using CsInjection.ManualMapInjection.Injection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace CsInjection.Tests
{
    [TestClass]
    public class InjectionTests
    {
        [TestMethod]
        public void ManualMapInjectionTest()
        {
            Process targetProcess = Process.Start("Sandbox.App.exe");
            ManualMapInjector injector = new ManualMapInjector(targetProcess);
            FileInfo fileInfo = new FileInfo(@"CsInjection.Cpp.Bootstrap.dll");
            injector.Inject(fileInfo.FullName);
        }
    }
}
