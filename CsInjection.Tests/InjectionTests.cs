using CsInjection.ManualMapInjection.Injection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CsInjection.Tests
{
    [TestClass]
    public class InjectionTests
    {
        [TestMethod]
        public void ManualMapInjectionTest()
        {
            string dir = "D:\\Repos\\CsInjection\\Output\\Sandbox";
            Process targetProcess = Process.Start(Path.Combine(dir, "Sandbox.App.exe"));
            ManualMapInjector injector = new ManualMapInjector(targetProcess);
            FileInfo fileInfo = new FileInfo(Path.Combine(dir, "Sandbox.Bootstrap.dll"));

            MessageBox.Show("Starting injection.");

            injector.Inject(fileInfo.FullName);

            MessageBox.Show("Injection completed.");

            targetProcess.Kill();
        }
    }
}
