using CsInjection.ManualMapInjection.Injection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Injector
{
    class Program
    {
        static readonly string dir = Environment.CurrentDirectory;
        static readonly string exe = Path.Combine(dir, "Sandbox.App.exe");
        static readonly string dll = Path.Combine(dir, "Sandbox.Bootstrap.dll");
        static void Main(string[] args)
        {
            Process targetProcess = Process.Start(exe);
            ManualMapInjector injector = new ManualMapInjector(targetProcess);
            injector.Inject(dll);
            
            //targetProcess.Kill();
        }
    }
}
