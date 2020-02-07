namespace GameSharp.Core.Native.Offsets
{
    public class PebOffsets64 : IPebOffsets
    {
        public int BeingDebugged { get; } = 0x2;
        public int NtGlobalFlag { get; } = 0xBC;
    }
}
