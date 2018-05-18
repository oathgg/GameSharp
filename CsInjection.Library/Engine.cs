using System;
using System.ComponentModel;
using CsInjection.Library.Helpers;

namespace CsInjection.Library
{
    public class Engine
    {
        public static int Initialize(string s)
        {
            Native.AllocConsole();

            Console.WriteLine("Printing text to Console.");

            TestPrintChat();

            TestDrawCirclePatch();

            return 0;
        }

        private static void TestPrintChat()
        {
            Console.WriteLine($"Printing something to chat in game.");
            Functions.PrintChat("Injected.");
            Console.WriteLine($"Finished printing something to chat in game.");
        }

        private static void TestDrawCirclePatch()
        {
            Console.WriteLine($"Patching byte at address 0x{Offsets.DrawCirclePatch.ToInt64()}.");
            BytePatch patch = new BytePatch(Offsets.DrawCirclePatch);
            patch.Patch(new byte[] { 0xEB });
            Console.WriteLine($"Successfully patched!");
        }
    }
}
