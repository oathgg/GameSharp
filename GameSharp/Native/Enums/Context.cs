﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Native.Enums
{
    [Flags]
    public enum Context : uint
    {
        CONTEXT_i386 = 0x10000,
        CONTEXT_i486 = 0x10000,
        CONTEXT_CONTROL = CONTEXT_i386 | 0x01,
        CONTEXT_INTEGER = CONTEXT_i386 | 0x02,
        CONTEXT_SEGMENTS = CONTEXT_i386 | 0x04,
        CONTEXT_FLOATING_POINT = CONTEXT_i386 | 0x08,
        CONTEXT_DEBUG_REGISTERS = CONTEXT_i386 | 0x10,
        CONTEXT_EXTENDED_REGISTERS = CONTEXT_i386 | 0x20,
        CONTEXT_FULL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS,
        CONTEXT_ALL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS | CONTEXT_FLOATING_POINT | CONTEXT_DEBUG_REGISTERS | CONTEXT_EXTENDED_REGISTERS
    }
}
