using PeNet.Structures;
using System.Collections.Generic;
using System.Linq;

namespace PeNet.Parser
{
    internal class ImageBaseRelocationsParser : SafeParser<IMAGE_BASE_RELOCATION[]>
    {
        private readonly uint _directorySize;

        public ImageBaseRelocationsParser(
            byte[] buff,
            uint offset,
            uint directorySize
            )
            : base(buff, offset)
        {
            _directorySize = directorySize;
        }

        protected override IMAGE_BASE_RELOCATION[] ParseTarget()
        {
            if (_offset == 0)
            {
                return null;
            }

            List<IMAGE_BASE_RELOCATION> imageBaseRelocations = new List<IMAGE_BASE_RELOCATION>();
            uint currentBlock = _offset;


            while (true)
            {
                if (currentBlock >= _offset + _directorySize - 8)
                {
                    break;
                }

                imageBaseRelocations.Add(new IMAGE_BASE_RELOCATION(_buff, currentBlock, _directorySize));
                currentBlock += imageBaseRelocations.Last().SizeOfBlock;
            }

            return imageBaseRelocations.ToArray();
        }
    }
}