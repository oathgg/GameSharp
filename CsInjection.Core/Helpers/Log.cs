using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsInjection.Core.Helpers
{
    public static class Log
    {
        public static void Write(string message)
        {
            Console.WriteLine($"[CsInjection] - {message}");
        }
    }
}
