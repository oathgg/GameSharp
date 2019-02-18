using GameSharp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Tests
{
    public static class Utils
    {
        public static byte[] GenerateByteArray(int size = 10)
        {
            Random rnd = new Random();
            byte[] array = new byte[size];
            rnd.NextBytes(array);
            return array;
        }
    }
}
