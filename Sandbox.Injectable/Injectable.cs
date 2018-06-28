using CsInjection.Core.Native;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Sandbox.Injectable
{
    public class Injectable
    {
        public static int EntryPoint(string s)
        {
            Kernel32.AllocConsole();

            RunTests();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            return 0;
        }

        private static void RunTests()
        {
            const string text = "Hello world from C# injectable class";
            GenerateConsoleLine(text);
            GenerateManagedException();
            HookOnAbout();
            //CallFunction();
        }

        private static void CallFunction()
        {
            CallAbout.Run();
        }

        private static void HookOnAbout()
        {
            Console.WriteLine("Hooking on about");
            HookOnAbout onAbout = new HookOnAbout();
            onAbout.InstallHook();
        }

        private static void GenerateManagedException()
        {
            try
            {
                throw new Exception("Fake error has been thrown.");
            }
            catch (Exception ex)
            {
                GenerateConsoleLine(ex.Message);
            }
        }

        private static void GenerateConsoleLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
