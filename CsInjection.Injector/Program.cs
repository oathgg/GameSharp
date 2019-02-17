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
            InjectDll("Application-steam-x64", "Banished.Injectable.dll");
            //InjectDll("wow", "wow.injectable.dll");
            //InjectDll("ScyllaTest_x64", "ScyllaTest_x64.dll");
        }

        static void InjectDll(string processName, string dllName)
        {
            string dllPath = Path.Combine(Environment.CurrentDirectory, dllName);
            Process process = Process.GetProcessesByName(processName).FirstOrDefault();

            IInjection injector = new RemoteThreadInjection(process);

            injector.InjectAndExecute(dllPath, "Main");

            injector.AttachToProcess();
        }
    }
}
