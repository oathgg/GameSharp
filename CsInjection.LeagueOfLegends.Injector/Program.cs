using CsInjection.Core.Extensions;
using CsInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LeagueOfLegends.Injector
{
    internal class Program
    {
        private const string exe = "League of Legends";
        private const string dllEntryPoint = "Initialize";
        private static readonly string dll = Path.Combine(Environment.CurrentDirectory, "LeagueOfLegends.Injectable.dll");

        private static void Main(string[] args)
        {
            Process targetProcess = Process.GetProcessesByName(exe).FirstOrDefault();
            RemoteThreadInjection injector = new RemoteThreadInjection(targetProcess);

            if (Debugger.IsAttached)
                targetProcess.Attach();

            injector.Inject(dll, dllEntryPoint);
        }
    }
}