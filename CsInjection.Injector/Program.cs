using CsInjection.Core.Extensions;
using CsInjection.Core.Injection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsInjection.Injector
{
    class Program
    {
        static void Main(string[] args)
        {
            //InjectDll("ScyllaTest_x64");
            InjectDll("ScyllaTest_x86");
        }

        static void InjectDll(string processName)
        {
            string dllPath = Path.Combine(Environment.CurrentDirectory, $"{processName}.Injectable.dll");
            Process process = Process.GetProcessesByName(processName).FirstOrDefault();

            IInjection injector = new RemoteThreadInjection(process);

            injector.InjectAndExecute(dllPath, "Main");
            injector.AttachToProcess();
        }
    }
}
