using GameSharp.External;
using GameSharp.External.Injection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GameSharp.Notepadpp.Injector
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // The process we are injecting into.
            GameSharpProcess process = new GameSharpProcess(Process.GetProcessesByName("notepad++").FirstOrDefault());

            if (process == null)
            {
                throw new Exception("Process not found.");
            }

            // A simple RemoteThreadInjector.
            IInjection injector = new RemoteThreadInjection(process);

            // Inject the DLL and executes the entrypoint.
            string pathToDll = Path.Combine(Environment.CurrentDirectory, "GameSharp.Notepadpp.dll");
            injector.InjectAndExecute(new Injectable(pathToDll, "Main"), attach: true);
        }
    }
}
