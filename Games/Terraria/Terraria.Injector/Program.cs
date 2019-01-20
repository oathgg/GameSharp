using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CsInjection.Injection.Injection;

namespace Terraria.Injector
{
    internal class Program
    {
        private const string exe = "ProcessHacker";
        private const string dllEntryPoint = "Main";
        private static readonly string dll = Path.Combine(Environment.CurrentDirectory, "Terraria.Injectable.dll");

        private static void Main(string[] args)
        {
            Process targetProcess = Process.GetProcessesByName(exe).FirstOrDefault();
            IInjection injector = new RemoteThreadInjection(targetProcess);

            injector.InjectAndExecute(dll, dllEntryPoint);
        }
    }
}