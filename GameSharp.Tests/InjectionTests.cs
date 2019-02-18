﻿using System;
using System.Diagnostics;
using System.Threading;
using GameSharp.Injection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSharp.Tests
{
    [TestClass]
    public class InjectionTests
    {
        [TestMethod]
        public void RemoteThreadInjectionTest()
        {
            Process process = Process.Start("notepad++");

            Thread.Sleep(1000);

            IInjection injection = new RemoteThreadInjection(process);
            injection.InjectAndExecute(Environment.CurrentDirectory + "\\..\\..\\Dll\\TestDll_x86.dll", "Main");

            Assert.IsNotNull(process.Id);

            process.Kill();
        }
    }
}