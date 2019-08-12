using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSharp.Tests
{
    [TestClass]
    public class InjectionTests
    {
        //[TestMethod]
        //public void RemoteThreadInjectionTest()
        //{
        //    Process process = Process.Start("notepad");
        //    try
        //    {
        //        process.WaitForInputIdle();

        //        IInjection injection = new RemoteThreadInjection(process);
        //        injection.InjectAndExecute(Environment.CurrentDirectory + "\\..\\..\\Dll\\TestDll_x86.dll", "Main", false);

        //        Assert.IsNotNull(process.Id);
        //    }
        //    finally
        //    {
        //        if (process != null)
        //            process.Kill();
        //    }
        //}
    }
}
