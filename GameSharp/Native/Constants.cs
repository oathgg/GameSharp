using System;

namespace GameSharp.Native
{
    public class Constants
    {
        public const string KERNEL32_DLL = "kernel32.dll";
        public const string NTDLL_DLL = "ntdll.dll";
        public const string LOAD_LIBRARY_PROC = "LoadLibraryA";
        public const string NT_CREATE_THREAD_EX = "NtCreateThreadEx";

        public const UInt32 INFINITE = 0xFFFFFFFF;
        public const UInt32 WAIT_ABANDONED = 0x00000080;
        public const UInt32 WAIT_OBJECT_0 = 0x00000000;
        public const UInt32 WAIT_TIMEOUT = 0x00000102;
    }
}
