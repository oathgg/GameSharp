using CsInjection.Core.Helpers;
using CsInjection.Core.Utilities;
using Injectable.Functions;
using Injectable.Helpers;
using Injectable.Hooks;
using RGiesecke.DllExport;
using System;

namespace Injectable
{
    public class EntryPoint
    {
        [DllExport("Initialize")]
        public static void Main()
        {
            InjectionHelper.Initialize();

            SamplePrintChat();

            SampleDrawCirclePatch();

            SampleFunctionDetour();
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