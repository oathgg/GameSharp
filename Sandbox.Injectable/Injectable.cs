using CsInjection.Core.Native;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Sandbox.Injectable
{
    public class Injectable
    {
        public static int Main(string s)
        {
            Kernel32.AllocConsole();

            RunTests();

            Console.ReadKey();

            return 0;
        }

        private static void RunTests()
        {
            const string text = "Hello world from C# injectable class";
            GenerateConsoleLine(text);
            GenerateMessageBox(text);
            GenerateManagedException();
            HookOnAbout();
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
                MessageBox.Show(ex.Message);
            }
        }

        private static void GenerateMessageBox(string text)
        {
            MessageBox.Show(text);
        }

        private static void GenerateConsoleLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
