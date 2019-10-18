namespace GameSharp.Core.Native.Enums
{
    /// <summary>
    /// The structure of the Process Environment Block of a 32-bit process.
    /// </summary>
    /// <remarks>
    /// Tested on Windows 7 x64, 2013-03-10
    /// Source: http://blog.rewolf.pl/blog/?p=573#.UTyBo1fJL6p
    /// </remarks>
    public enum PebOffsets64
    {
        BeingDebugged = 0x2,
        NtGlobalFlag = 0xBC,
    }
}
