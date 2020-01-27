using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.External.Helpers
{
    public static class LoadLibraryHelper
    {
        public static  byte[] LoadLibraryPayload(string pathToDll)
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
