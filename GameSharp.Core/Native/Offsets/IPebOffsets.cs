namespace GameSharp.Core.Native.Offsets
{
    /// <summary>
    /// The structure of the Process Environment Block of a 32-bit process.
    /// </summary>
    /// <remarks>
    /// Tested on Windows 7 x64, 2013-03-10
    /// Source: http://blog.rewolf.pl/blog/?p=573#.UTyBo1fJL6p
    /// </remarks>
    public interface IPebOffsets
    {
        int BeingDebugged { get; }
        int NtGlobalFlag { get; }
    }
}
