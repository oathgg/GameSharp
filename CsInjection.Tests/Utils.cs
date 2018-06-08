using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsInjection.Tests
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
