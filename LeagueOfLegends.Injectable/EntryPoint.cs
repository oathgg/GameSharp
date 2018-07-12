using System;
using CsInjection.Core.Utilities;
using CsInjection.Core.Helpers;
using Injectable.Helpers;
using Injectable.Hooks;
using Injectable.Functions;
using System.Runtime.InteropServices;

namespace Injectable
{
    public class EntryPoint
    {
        public static int Main(string s)
        {
            InjectionHelper.Initialize();

            SamplePrintChat();

            SampleDrawCirclePatch();

            SampleFunctionDetour();

            return 0;
        }

        private static void SampleFunctionDetour()
        {
            Console.WriteLine("Hooking on game update");
            HookOnUpdate onUpdate = new HookOnUpdate();
            onUpdate.InstallHook();
        }

        private static void SamplePrintChat()
        {
            Console.WriteLine($"Printing something to chat in game.");
            Chat.Print("Injected.");
        }

        private static void SampleDrawCirclePatch()
        {
            Console.WriteLine($"Patching byte at address 0x{Offsets.DrawCirclePatch.ToInt64()}.");
            BytePatcher patch = new BytePatcher(Offsets.DrawCirclePatch);
            patch.Patch(new byte[] { 0xEB });
        }
    }
}
