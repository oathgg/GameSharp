using CsInjection.ManualMapInjection.Injection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CsInjection.Tests
{
    [TestClass]
    public class InjectionTest
    {
        [TestMethod]
        public void ManualMapInjection(string[] args)
        {
            Process targetProcess = Process.Start("Cpp.Sandbox.Exe");
            ManualMapInjector injector = new ManualMapInjector(targetProcess);
            FileInfo fileInfo = new FileInfo(@"CsInjection.Cpp.Bootstrap.dll");
            injector.Inject(fileInfo.FullName);
        }
    }
}
