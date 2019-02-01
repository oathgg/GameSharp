using CsInjection.Core.Injection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Injector
{
    class Program
    {
        private const string exe = "wow";
        private const string dllEntryPoint = "Main";
        private static readonly string dll = Path.Combine(Environment.CurrentDirectory, "WoWSharp.dll");

        private static void Main(string[] args)
        {
            Process targetProcess = Process.GetProcessesByName(exe).FirstOrDefault();
            IInjection injector = new RemoteThreadInjection(targetProcess);

            injector.InjectAndExecute(dll, dllEntryPoint);
        }
    }
}
