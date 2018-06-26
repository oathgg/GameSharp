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
        static void Main(string[] args)
        {
            string dir = "D:\\Repos\\CsInjection\\Output\\Sandbox";
            //Process targetProcess = Process.Start(Path.Combine(dir, "Sandbox.App.exe"));
            Process targetProcess = Process.GetProcessesByName("Sandbox.App")[0];
            ManualMapInjector injector = new ManualMapInjector(targetProcess);
            FileInfo fileInfo = new FileInfo(Path.Combine(dir, "Sandbox.Bootstrap.dll"));
            injector.Inject(fileInfo.FullName);
        }
    }
}
