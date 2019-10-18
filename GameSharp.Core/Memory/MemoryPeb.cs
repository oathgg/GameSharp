using GameSharp.Core.Native.Enums;
using System;

namespace GameSharp.Core.Memory
{
    public class MemoryPeb
    {
        private readonly IMemoryPointer PebBasePointer;

        public MemoryPeb(IProcess process)
        {
            PebBasePointer = process.GetPebAddress();
        }

        public bool BeingDebugged
        {
            get => PebBasePointer.Read<bool>((int)PebOffsets32.BeingDebugged);
            set => PebBasePointer.Write(value, (int)PebOffsets32.BeingDebugged);
        }
        
        public long NtGlobalFlag
        {
            get => PebBasePointer.Read<long>((int)PebOffsets32.NtGlobalFlag);
            set => PebBasePointer.Write(value, (int)PebOffsets32.NtGlobalFlag);
        }
    }
}
