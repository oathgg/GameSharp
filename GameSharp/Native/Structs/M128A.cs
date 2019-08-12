using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Native.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct M128A
    {
        public ulong High;
        public long Low;

        public override string ToString()
        {
            return string.Format("High:{0}, Low:{1}", this.High, this.Low);
        }
    }
}
