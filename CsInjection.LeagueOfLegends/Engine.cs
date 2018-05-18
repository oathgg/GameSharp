using System;
using CsInjection.Core;
using CsInjection.LeagueOfLegends.Helpers;
using CsInjection.LeagueOfLegends.Hooks;

namespace CsInjection.LeagueOfLegends
{
    public class Engine
    {
        public static int Initialize(string s)
        {
            NativeAPI.AllocConsole();

            Console.WriteLine("Printing text to Console.");

            SamplePrintChat();

            SampleDrawCirclePatch();

            SampleFunctionDetour();

            return 0;
        }

        private static void SampleFunctionDetour()
        {
            Console.WriteLine("Hooking on AFK");
            HookOnAfk afkHook = new HookOnAfk();
            afkHook.InstallHook();
            Console.WriteLine("Finished hooking on AFK");
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
