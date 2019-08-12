using PeNet.Structures;
using PeNet.Utilities;

namespace PeNet.Parser
{
    internal class ExportedFunctionsParser : SafeParser<ExportFunction[]>
    {
        private readonly IMAGE_EXPORT_DIRECTORY _exportDirectory;
        private readonly IMAGE_SECTION_HEADER[] _sectionHeaders;

        internal ExportedFunctionsParser(
            byte[] buff,
            IMAGE_EXPORT_DIRECTORY exportDirectory,
            IMAGE_SECTION_HEADER[] sectionHeaders
            )
            : base(buff, 0)
        {
            _exportDirectory = exportDirectory;
            _sectionHeaders = sectionHeaders;
        }

        protected override ExportFunction[] ParseTarget()
        {
            if (_exportDirectory == null || _exportDirectory.AddressOfFunctions == 0)
                return null;

            ExportFunction[] expFuncs = new ExportFunction[_exportDirectory.NumberOfFunctions];

            uint funcOffsetPointer = _exportDirectory.AddressOfFunctions.RVAtoFileMapping(_sectionHeaders);
            uint ordOffset = _exportDirectory.AddressOfNameOrdinals.RVAtoFileMapping(_sectionHeaders);
            uint nameOffsetPointer = _exportDirectory.AddressOfNames.RVAtoFileMapping(_sectionHeaders);

            //Get addresses
            for (uint i = 0; i < expFuncs.Length; i++)
            {
                uint ordinal = i + _exportDirectory.Base;
                uint address = _buff.BytesToUInt32(funcOffsetPointer + sizeof(uint) * i);

                expFuncs[i] = new ExportFunction(null, address, (ushort)ordinal);
            }

            //Associate names
            for (uint i = 0; i < _exportDirectory.NumberOfNames; i++)
            {
                uint namePtr = _buff.BytesToUInt32(nameOffsetPointer + sizeof(uint) * i);
                uint nameAdr = namePtr.RVAtoFileMapping(_sectionHeaders);
                string name = _buff.GetCString(nameAdr);
                uint ordinalIndex = _buff.GetOrdinal(ordOffset + sizeof(ushort) * i);

                expFuncs[ordinalIndex] = new ExportFunction(name, expFuncs[ordinalIndex].Address,
                    expFuncs[ordinalIndex].Ordinal);
            }

            return expFuncs;
        }
    }
}