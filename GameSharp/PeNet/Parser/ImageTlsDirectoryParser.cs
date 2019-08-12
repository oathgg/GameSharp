using PeNet.Structures;
using PeNet.Utilities;
using System.Collections.Generic;

namespace PeNet.Parser
{
    internal class ImageTlsDirectoryParser : SafeParser<IMAGE_TLS_DIRECTORY>
    {
        private readonly bool _is64Bit;
        private readonly IMAGE_SECTION_HEADER[] _sectionsHeaders;

        internal ImageTlsDirectoryParser(
            byte[] buff,
            uint offset,
            bool is64Bit,
            IMAGE_SECTION_HEADER[] sectionHeaders
            )
            : base(buff, offset)
        {
            _is64Bit = is64Bit;
            _sectionsHeaders = sectionHeaders;
        }

        protected override IMAGE_TLS_DIRECTORY ParseTarget()
        {
            IMAGE_TLS_DIRECTORY tlsDir = new IMAGE_TLS_DIRECTORY(_buff, _offset, _is64Bit);
            tlsDir.TlsCallbacks = ParseTlsCallbacks(tlsDir.AddressOfCallBacks);
            return tlsDir;
        }

        private IMAGE_TLS_CALLBACK[] ParseTlsCallbacks(ulong addressOfCallBacks)
        {
            List<IMAGE_TLS_CALLBACK> callbacks = new List<IMAGE_TLS_CALLBACK>();
            uint rawAddressOfCallbacks = (uint)addressOfCallBacks.VAtoFileMapping(_sectionsHeaders);

            uint count = 0;
            while (true)
            {
                if (_is64Bit)
                {
                    IMAGE_TLS_CALLBACK cb = new IMAGE_TLS_CALLBACK(_buff, rawAddressOfCallbacks + count * 8, _is64Bit);
                    if (cb.Callback == 0)
                        break;

                    callbacks.Add(cb);
                    count++;
                }
                else
                {
                    IMAGE_TLS_CALLBACK cb = new IMAGE_TLS_CALLBACK(_buff, rawAddressOfCallbacks + count * 4, _is64Bit);
                    if (cb.Callback == 0)
                        break;

                    callbacks.Add(cb);
                    count++;
                }
            }

            return callbacks.ToArray();
        }
    }
}