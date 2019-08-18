using PeNet.Structures;
using PeNet.Utilities;
using System.Collections.Generic;

namespace PeNet.Parser
{
    internal class ImportedFunctionsParser : SafeParser<ImportFunction[]>
    {
        private readonly IMAGE_IMPORT_DESCRIPTOR[] _importDescriptors;
        private readonly bool _is64Bit;
        private readonly IMAGE_SECTION_HEADER[] _sectionHeaders;

        internal ImportedFunctionsParser(
            byte[] buff,
            IMAGE_IMPORT_DESCRIPTOR[] importDescriptors,
            IMAGE_SECTION_HEADER[] sectionHeaders,
            bool is64Bit) :
                base(buff, 0)
        {
            _importDescriptors = importDescriptors;
            _sectionHeaders = sectionHeaders;
            _is64Bit = is64Bit;
        }

        protected override ImportFunction[] ParseTarget()
        {
            if (_importDescriptors == null)
            {
                return null;
            }

            List<ImportFunction> impFuncs = new List<ImportFunction>();
            uint sizeOfThunk = (uint)(_is64Bit ? 0x8 : 0x4); // Size of IMAGE_THUNK_DATA
            ulong ordinalBit = _is64Bit ? 0x8000000000000000 : 0x80000000;
            ulong ordinalMask = (ulong)(_is64Bit ? 0x7FFFFFFFFFFFFFFF : 0x7FFFFFFF);

            foreach (IMAGE_IMPORT_DESCRIPTOR idesc in _importDescriptors)
            {
                uint dllAdr = idesc.Name.RVAtoFileMapping(_sectionHeaders);
                string dll = _buff.GetCString(dllAdr);
                if (IsModuleNameTooLong(dll))
                {
                    continue;
                }

                uint tmpAdr = idesc.OriginalFirstThunk != 0 ? idesc.OriginalFirstThunk : idesc.FirstThunk;
                if (tmpAdr == 0)
                {
                    continue;
                }

                uint thunkAdr = tmpAdr.RVAtoFileMapping(_sectionHeaders);
                uint round = 0;
                while (true)
                {
                    IMAGE_THUNK_DATA t = new IMAGE_THUNK_DATA(_buff, thunkAdr + round * sizeOfThunk, _is64Bit);

                    if (t.AddressOfData == 0)
                    {
                        break;
                    }

                    // Check if import by name or by ordinal.
                    // If it is an import by ordinal, the most significant bit of "Ordinal" is "1" and the ordinal can
                    // be extracted from the least significant bits.
                    // Else it is an import by name and the link to the IMAGE_IMPORT_BY_NAME has to be followed

                    if ((t.Ordinal & ordinalBit) == ordinalBit) // Import by ordinal
                    {
                        impFuncs.Add(new ImportFunction(null, dll, (ushort)(t.Ordinal & ordinalMask)));
                    }
                    else // Import by name
                    {
                        IMAGE_IMPORT_BY_NAME ibn = new IMAGE_IMPORT_BY_NAME(_buff,
                            ((uint)t.AddressOfData).RVAtoFileMapping(_sectionHeaders));
                        impFuncs.Add(new ImportFunction(ibn.Name, dll, ibn.Hint));
                    }

                    round++;
                }
            }


            return impFuncs.ToArray();
        }

        private bool IsModuleNameTooLong(string dllName)
        {
            return dllName.Length > 256;
        }
    }
}