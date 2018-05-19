using System;
using CsInjection.Core.Utilities;
using CsInjection.Core.Native;
using CsInjection.LeagueOfLegends.Helpers;
using CsInjection.LeagueOfLegends.Hooks;

namespace CsInjection.LeagueOfLegends
{
    public class Engine
    {
        public static int Initialize(string s)
        {
            Kernel32.AllocConsole();

            Console.WriteLine("Printing text to Console.");

            SamplePrintChat();

            SampleDrawCirclePatch();

            SampleFunctionDetour();

            Console.ReadKey();
            return 0;
        }

        private static void SampleFunctionDetour()
        {
            HookOnAfk afkHook = new HookOnAfk();
            afkHook.InstallHook();
        }

        private static void SamplePrintChat()
        {
            Console.WriteLine($"Printing something to chat in game.");
            Functions.PrintChat("Injected.");
            Console.WriteLine($"Finished printing something to chat in game.");
        }

        private static void SampleDrawCirclePatch()
        {
            Console.WriteLine($"Patching byte at address 0x{Offsets.DrawCirclePatch.ToInt64()}.");
            BytePatcher patch = new BytePatcher(Offsets.DrawCirclePatch);
            patch.Patch(new byte[] { 0xEB });
            Console.WriteLine($"Successfully patched!");
        }
    }
}
