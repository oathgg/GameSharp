using System;
using CsInjection.Core.Helpers;

namespace CsInjection.LeagueOfLegends.Helpers
{
    public static class Offsets
    {
        // Patch 8.10.229.7328 offsets :: Base 0.
        private static IntPtr ClientModuleBaseAddress = ProcessHelper.GetMainModuleBaseAddress;
        public static IntPtr PrintChat = ClientModuleBaseAddress + 0x0056CD30;
        public static IntPtr ChatClient = ClientModuleBaseAddress + 0x02E36EB8;
        public static IntPtr DrawCirclePatch = ClientModuleBaseAddress + 0x005FBC87;

        // Events
        public static IntPtr OnAfk = ClientModuleBaseAddress + 0x005D6BC0;
        public static IntPtr OnCreateObject = ClientModuleBaseAddress + 0x2E4520;
        public static IntPtr OnDeleteObject = ClientModuleBaseAddress + 0x2D8BB0;
    }
}
