using System;
using System.Runtime.InteropServices;
using CsInjection.Core;
using CsInjection.Core.Helpers;
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

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            return 0;
        }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall, SetLastError = true)]
        public delegate void OnAfkDelegate(IntPtr thisPtr);

        private static void OnAfkDetour(IntPtr thisPtr)
        {
            Console.WriteLine($"Event::OnAfk");
            detour.CallOriginal(thisPtr);
        }

        private static Detour detour;
        private static void SampleFunctionDetour()
        {
            Console.WriteLine("Hooking on AFK");

            detour = new Detour(Marshal.GetDelegateForFunctionPointer<OnAfkDelegate>(Offsets.OnAfk), 
                new OnAfkDelegate(OnAfkDetour));
            detour.Enable();

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
