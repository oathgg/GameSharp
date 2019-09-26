using System.Runtime.InteropServices;

namespace GameSharp.Core.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct M128A
    {
        public ulong High;
        public long Low;

        public override string ToString()
        {
            return string.Format("High:{0}, Low:{1}", High, Low);
        }
    }
}
