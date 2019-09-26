using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PeNet
{
    /// <summary>
    ///     This class parses the Certificate Revocation Lists
    ///     of a signing certificate. It provides access to all
    ///     CRL URLs in the certificate.
    /// </summary>
    public class CrlUrlList
    {
        /// <summary>
        ///     Create a new CrlUrlList object.
        /// </summary>
        /// <param name="rawData">A byte array containing a X509 certificate</param>
        public CrlUrlList(byte[] rawData)
        {
            Urls = new List<string>();

            if (rawData == null)
            {
                return;
            }

            ParseCrls(rawData);
        }

        /// <summary>
        ///     Create a new CrlUrlList object.
        /// </summary>
        /// <param name="cert">A X509 certificate object.</param>
        public CrlUrlList(X509Certificate2 cert)
        {
            Urls = new List<string>();
            if (cert == null)
            {
                return;
            }

            foreach (X509Extension ext in cert.Extensions)
            {
                if (ext.Oid.Value == "2.5.29.31")
                {
                    ParseCrls(ext.RawData);
                }
            }
        }

        /// <summary>
        ///     List with all CRL URLs.
        /// </summary>
        public List<string> Urls { get; }

        private void ParseCrls(byte[] rawData)
        {
            int rawLength = rawData.Length;
            for (int i = 0; i < rawLength - 5; i++)
            {
                // Find a HTTP(s) string.
                if ((rawData[i] == 'h'
                     && rawData[i + 1] == 't'
                     && rawData[i + 2] == 't'
                     && rawData[i + 3] == 'p'
                     && rawData[i + 4] == ':')
                    || (rawData[i] == 'l'
                        && rawData[i + 1] == 'd'
                        && rawData[i + 2] == 'a'
                        && rawData[i + 3] == 'p'
                        && rawData[i + 4] == ':'))
                {
                    List<byte> bytes = new List<byte>();
                    for (int j = i; j < rawLength; j++)
                    {
                        if ((rawData[j - 4] == '.'
                             && rawData[j - 3] == 'c'
                             && rawData[j - 2] == 'r'
                             && rawData[j - 1] == 'l')
                            || (rawData[j] == 'b'
                                && rawData[j + 1] == 'a'
                                && rawData[j + 2] == 's'
                                && rawData[j + 3] == 'e'
                                ))
                        {
                            i = j;
                            break;
                        }


                        if (rawData[j] < 0x20 || rawData[j] > 0x7E)
                        {
                            i = j;
                            break;
                        }

                        bytes.Add(rawData[j]);
                    }
                    string uri = Encoding.ASCII.GetString(bytes.ToArray());

                    if (IsValidUri(uri) && uri.StartsWith("http://") && uri.EndsWith(".crl"))
                    {
                        Urls.Add(uri);
                    }

                    if (uri.StartsWith("ldap:", StringComparison.InvariantCulture))
                    {
                        uri = "ldap://" + uri.Split('/')[2];
                        Urls.Add(uri);
                    }
                }
            }
        }

        private bool IsValidUri(string uri)
        {
            return Uri.TryCreate(uri, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp
                       || uriResult.Scheme == Uri.UriSchemeHttps);
        }


        /// <summary>
        ///     Create a string representation of all CRL in
        ///     the list.
        /// </summary>
        /// <returns>CRL URLs.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CRL URLs:");
            foreach (string url in Urls)
            {
                sb.AppendFormat("\t{0}\n", url);
            }

            return sb.ToString();
        }
    }
}