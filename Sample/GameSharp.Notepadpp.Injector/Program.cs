using GameSharp.Injection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Notepadpp.Injector
{
    class Program
    {
        static void Main(string[] args)
        {
            // The process we are injecting into.
            Process process = Process.GetProcessesByName("notepad++").FirstOrDefault();

            // A simple RemoteThreadInjector.
            IInjection injector = new RemoteThreadInjection(process);

            // Inject the DLL and executes the entrypoint.
            string pathToDll = Path.Combine(Environment.CurrentDirectory, "GameSharp.Notepadpp.dll.dll");
            injector.InjectAndExecute(pathToDll, "Main", attach: true);
        }
    }
}
