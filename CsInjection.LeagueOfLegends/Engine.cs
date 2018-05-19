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
            // Creates a new console window
            Kernel32.AllocConsole();

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
        }

        private static void SampleDrawCirclePatch()
        {
            Console.WriteLine($"Patching byte at address 0x{Offsets.DrawCirclePatch.ToInt64()}.");
            BytePatcher patch = new BytePatcher(Offsets.DrawCirclePatch);
            patch.Patch(new byte[] { 0xEB });
        }
    }
}
