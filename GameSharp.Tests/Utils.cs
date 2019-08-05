using System;

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
