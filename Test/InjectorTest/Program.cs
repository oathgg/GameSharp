using CsInjection.Core.Injection;
using System;
using System.Diagnostics;
using System.IO;

namespace ScyllaTest_x64.Injectable
{
    class Program
    {
        private const string dllEntryPoint = "Main";
        private static readonly string dll = Path.Combine(Environment.CurrentDirectory, "DllTest.dll");

        private static void Main(string[] args)
        {
            Process targetProcess = Process.GetProcessesByName("ScyllaTest_x64")[0];
            IInjection injector = new RemoteThreadInjection(targetProcess);

            injector.InjectAndExecute(dll, dllEntryPoint);
            injector.AttachToProcess();
        }
    }
}
