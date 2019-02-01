using CsInjection.Injection.Injection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectorTest
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
        }
    }
}
