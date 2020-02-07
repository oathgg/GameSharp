using System;
using System.IO;
using System.Text;

namespace GameSharp.External.Helpers
{
    public static class LoadLibraryHelper
    {
        public static byte[] LoadLibraryPayload(string pathToDll)
        {
            if (string.IsNullOrWhiteSpace(pathToDll))
            {
                throw new NullReferenceException("pathToDll");
            }

            if (!File.Exists(pathToDll))
            {
                throw new FileNotFoundException(pathToDll);
            }

            byte[] pathBytes = Encoding.Unicode.GetBytes(pathToDll);

            return pathBytes;
        }
    }
}
