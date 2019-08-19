using System;

namespace GameSharp.Core.Native.Enums
{
    [Flags]
    public enum LoadLibraryFlags : uint
    {
        DontResolveDllReferences = 0x00000001,
        LoadIgnoreCodeAuthzLevel = 0x00000010,
        LoadLibraryAsDatafile = 0x00000002,
        LoadLibraryAsDatafileExclusive = 0x00000040,
        LoadLibraryAsImageResource = 0x00000020,
        LoadWithAlteredSearchPath = 0x00000008
    }
}
