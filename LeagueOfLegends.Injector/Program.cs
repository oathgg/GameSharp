using CsInjection.Core.Helpers;
using CsInjection.Injection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LeagueOfLegends.Injector
{
    class Program
    {
        static readonly string dir = Environment.CurrentDirectory;
        static readonly string exe = "League of Legends";
        static readonly string dll = Path.Combine(dir, "CsInjection.Bootstrapper.dll");

        static void Main(string[] args)
        {
            Process targetProcess = Process.GetProcessesByName(exe).FirstOrDefault();
            ManualMapInjection injector = new ManualMapInjection(targetProcess);

            if (Debugger.IsAttached)
                targetProcess.Attach();

            injector.Inject(dll);
        }
    }
}
