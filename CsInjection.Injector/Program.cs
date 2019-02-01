using CsInjection.Core.Injection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CsInjection.Injector
{
    class Program
    {
        static void Main(string[] args)
        {
            InjectDll("wow");
            //InjectDll("ScyllaTest_x64");
        }

        static void InjectDll(string processName)
        {
            string dllPath = Path.Combine(Environment.CurrentDirectory, $"{processName}.Injectable.dll");
            Process process = Process.GetProcessesByName(processName).FirstOrDefault();

            IInjection injector = new RemoteThreadInjection(process);

            injector.InjectAndExecute(dllPath, "Main");

            //injector.AttachToProcess();
        }
    }
}
