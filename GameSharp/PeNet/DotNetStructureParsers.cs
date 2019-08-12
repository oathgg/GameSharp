using PeNet.Parser;
using PeNet.Structures;
using PeNet.Utilities;
using System.Linq;

namespace PeNet
{
    internal class DotNetStructureParsers
    {
        private readonly byte[] _buff;
        private readonly IMAGE_SECTION_HEADER[] _sectionHeaders;
        private readonly IMAGE_COR20_HEADER _imageCor20Header;
        private MetaDataHdrParser _metaDataHdrParser;
        private MetaDataStreamStringParser _metaDataStreamStringParser;
        private MetaDataStreamUSParser _metaDataStreamUSParser;
        private MetaDataStreamTablesHeaderParser _metaDataStreamTablesHeaderParser;
        private MetaDataStreamGUIDParser _metaDataStreamGuidParser;
        private MetaDataStreamBlobParser _metaDataStreamBlobParser;

        public METADATAHDR MetaDataHdr => _metaDataHdrParser?.GetParserTarget();
        public IMETADATASTREAM_STRING MetaDataStreamString => _metaDataStreamStringParser?.GetParserTarget();
        public IMETADATASTREAM_US MetaDataStreamUS => _metaDataStreamUSParser?.GetParserTarget();
        public IMETADATASTREAM_GUID MetaDataStreamGUID => _metaDataStreamGuidParser?.GetParserTarget();
        public byte[] MetaDataStreamBlob => _metaDataStreamBlobParser?.GetParserTarget();
        public METADATATABLESHDR MetaDataStreamTablesHeader => _metaDataStreamTablesHeaderParser?.GetParserTarget();

        public DotNetStructureParsers(
            byte[] buff,
            IMAGE_COR20_HEADER imageCor20Header,
            IMAGE_SECTION_HEADER[] sectionHeaders)
        {
            _buff = buff;
            _sectionHeaders = sectionHeaders;
            _imageCor20Header = imageCor20Header;

            InitAllParsers();
        }

        private void InitAllParsers()
        {
            _metaDataHdrParser = InitMetaDataParser();
            _metaDataStreamStringParser = InitMetaDataStreamStringParser();
            _metaDataStreamUSParser = InitMetaDataStreamUSParser();
            _metaDataStreamTablesHeaderParser = InitMetaDataStreamTablesHeaderParser();
            _metaDataStreamGuidParser = InitMetaDataStreamGUIDParser();
            _metaDataStreamBlobParser = InitMetaDataStreamBlobParser();
        }

        private MetaDataHdrParser InitMetaDataParser()
        {
            uint? rawAddress = _imageCor20Header?.MetaData.VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);
            return rawAddress == null ? null : new MetaDataHdrParser(_buff, rawAddress.Value);
        }

        private MetaDataStreamStringParser InitMetaDataStreamStringParser()
        {
            METADATASTREAMHDR metaDataStream = MetaDataHdr?.MetaDataStreamsHdrs?.FirstOrDefault(x => x.streamName == "#Strings");

            if (metaDataStream == null)
                return null;

            return new MetaDataStreamStringParser(_buff, MetaDataHdr.Offset + metaDataStream.offset, metaDataStream.size);
        }

        private MetaDataStreamUSParser InitMetaDataStreamUSParser()
        {
            METADATASTREAMHDR metaDataStream = MetaDataHdr?.MetaDataStreamsHdrs?.FirstOrDefault(x => x.streamName == "#US");

            if (metaDataStream == null)
                return null;

            return new MetaDataStreamUSParser(_buff, MetaDataHdr.Offset + metaDataStream.offset, metaDataStream.size);
        }

        private MetaDataStreamTablesHeaderParser InitMetaDataStreamTablesHeaderParser()
        {
            METADATASTREAMHDR metaDataStream = MetaDataHdr?.MetaDataStreamsHdrs?.FirstOrDefault(x => x.streamName == "#~");

            if (metaDataStream == null)
                return null;

            return new MetaDataStreamTablesHeaderParser(_buff, MetaDataHdr.Offset + metaDataStream.offset);
        }

        private MetaDataStreamGUIDParser InitMetaDataStreamGUIDParser()
        {
            METADATASTREAMHDR metaDataStream = MetaDataHdr?.MetaDataStreamsHdrs?.FirstOrDefault(x => x.streamName == "#GUID");

            if (metaDataStream == null)
                return null;

            return new MetaDataStreamGUIDParser(_buff, MetaDataHdr.Offset + metaDataStream.offset, metaDataStream.size);
        }

        private MetaDataStreamBlobParser InitMetaDataStreamBlobParser()
        {
            METADATASTREAMHDR metaDataStream = MetaDataHdr?.MetaDataStreamsHdrs?.FirstOrDefault(x => x.streamName == "#Blob");

            if (metaDataStream == null)
                return null;

            return new MetaDataStreamBlobParser(_buff, MetaDataHdr.Offset + metaDataStream.offset, metaDataStream.size);
        }
    }
}