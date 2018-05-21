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

            SamplePrintChat();

            SampleDrawCirclePatch();

            SampleFunctionDetour();

            Console.ReadKey();
            return 0;
        }

        private static void SampleFunctionDetour()
        {
            Console.WriteLine("Hooking on AFK");
            HookOnAfk afkHook = new HookOnAfk();
            afkHook.InstallHook();

            Console.WriteLine("Hooking on create object");
            HookOnCreateObject onCreateObject = new HookOnCreateObject();
            onCreateObject.InstallHook();

            Console.WriteLine("Hooking on delete object");
            HookOnDeleteObject onDeleteObject = new HookOnDeleteObject();
            onDeleteObject.InstallHook();
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
