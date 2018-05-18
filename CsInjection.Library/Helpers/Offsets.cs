using System;
using System.Diagnostics;

namespace CsInjection.Library.Helpers
{
    public static class Offsets
    {
        // Patch 8.10.229.7328 offsets :: Base 0.
        private static IntPtr ClientModuleBaseAddress = ProcessHelper.GetMainModuleBaseAddress;
        public static IntPtr PrintChat = ClientModuleBaseAddress + 0x0056CD30;
        public static IntPtr ChatClient = ClientModuleBaseAddress + 0x02E36EB8;
        public static IntPtr DrawCirclePatch = ClientModuleBaseAddress + 0x005FBC87;
    }
}
