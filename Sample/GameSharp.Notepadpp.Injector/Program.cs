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
            Process notepadpp;
            notepadpp = Process.GetProcessesByName("notepad++").FirstOrDefault();

            if (notepadpp == null)
            {
                // The process we are injecting into.
                notepadpp = Process.Start("notepad++");
                notepadpp.WaitForInputIdle();
            }

            GameSharpProcess gameSharp = new GameSharpProcess(notepadpp);

            if (gameSharp == null)
            {
                throw new Exception("Process not found.");
            }

            // A simple RemoteThreadInjector.
            IInjection injector = new RemoteThreadInjection(gameSharp);

            // Inject the DLL and executes the entrypoint.
            string pathToDll = Path.Combine(Environment.CurrentDirectory, "GameSharp.Notepadpp.dll");
            injector.InjectAndExecute(new Injectable(pathToDll, "Main"), attach: true, launchConsole: true);
        }
    }
}
