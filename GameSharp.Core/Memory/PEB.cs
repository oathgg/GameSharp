using GameSharp.Core.Native.Offsets;

namespace GameSharp.Core.Memory
{
    public class PEB
    {
        public bool IsValid => _pebPointer.IsValid;
        private readonly IMemoryPointer _pebPointer;
        private readonly IPebOffsets _pebOffsets;

        public PEB(IProcess process)
        {
            _pebPointer = process.GetPebAddress();

            if (process.Is64Bit)
            {
                _pebOffsets = new PebOffsets64();
            }
            else
            {
                _pebOffsets = new PebOffsets32();
            }
        }

        public bool BeingDebugged
        {
            get => _pebPointer.Read<bool>(_pebOffsets.BeingDebugged);
            set => _pebPointer.Write(value, _pebOffsets.BeingDebugged);
        }

        public long NtGlobalFlag
        {
            get => _pebPointer.Read<long>(_pebOffsets.NtGlobalFlag);
            set => _pebPointer.Write(value, _pebOffsets.NtGlobalFlag);
        }
    }
}
