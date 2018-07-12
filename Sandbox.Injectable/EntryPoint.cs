using CsInjection.Core.Helpers;
using System;

namespace Injectable
{
    /// <summary>
    ///     EntryPoint class needs to have a static Main method which returns an int and takes 1 string parameter.
    /// </summary>
    public class EntryPoint
    {
        public static int Main(string s)
        {
            InjectionHelper.Initialize();
            RunTests();
            Console.ReadKey();
            return 0;
        }

        private static void RunTests()
        {
            const string text = "Hello world from C# injectable class";
            GenerateConsoleLine(text);
            GenerateManagedException();
            HookOnAbout();
            CallFunction();
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
                ExceptionHelper.BeautifyException(ex);
            }
        }

        private static void GenerateConsoleLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
