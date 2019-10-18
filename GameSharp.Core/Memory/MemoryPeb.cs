using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.Offsets;
using System;

namespace GameSharp.Core.Memory
{
    public class MemoryPeb
    {
        public bool IsValid => PebBasePointer.IsValid;
        private readonly IMemoryPointer PebBasePointer;
        private readonly IPebOffsets PebOffsets;

        public MemoryPeb(IProcess process)
        {
            PebBasePointer = process.GetPebAddress();

            if (process.Is64Bit)
            {
                PebOffsets = new PebOffsets64();
            }
            else
            {
                PebOffsets = new PebOffsets32();
            }
        }

        public bool BeingDebugged
        {
            get => PebBasePointer.Read<bool>(PebOffsets.BeingDebugged);
            set => PebBasePointer.Write(value, PebOffsets.BeingDebugged);
        }
        
        public long NtGlobalFlag
        {
            get => PebBasePointer.Read<long>(PebOffsets.NtGlobalFlag);
            set => PebBasePointer.Write(value, PebOffsets.NtGlobalFlag);
        }
    }
}
