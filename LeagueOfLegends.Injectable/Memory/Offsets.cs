using System;
using CsInjection.Core.Helpers;

namespace Injectable.Helpers
{
    public static class Offsets
    {
        // Patch 8.10.229.7328 offsets :: Base 0.
        private static IntPtr ClientModuleBaseAddress = ProcessHelper.GetMainModuleBaseAddress;

        // Functions
        public static IntPtr PrintChat = ClientModuleBaseAddress + 0x56CD30;
        public static IntPtr IssueOrder = ClientModuleBaseAddress + 0x1C9EA0;

        // Objects
        public static IntPtr ChatClient = ClientModuleBaseAddress + 0x2E36EB8;

        // Patches
        public static IntPtr DrawCirclePatch = ClientModuleBaseAddress + 0x5FBC87;

        // Events
        public static IntPtr OnAfk = ClientModuleBaseAddress + 0x5D6BC0;
        public static IntPtr OnCreateObject = ClientModuleBaseAddress + 0x2E4520;
        public static IntPtr OnDeleteObject = ClientModuleBaseAddress + 0x2D8BB0;

        // Can be found by looking for string 'GAMESTATE_GAMELOOP Begin\n' 
        // and then go to the very last jump table where everything comes together at the bottom of the class
        public static IntPtr OnUpdate = ClientModuleBaseAddress + 0x5BD820;
    }
}
