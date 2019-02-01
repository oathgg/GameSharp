using CsInjection.Core.Utilities;
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllTest
{
    public class Entrypoint
    {
        [DllExport]
        public static void Main()
        {
            ExceptionHandler.Initialize();
            Logger.Write("Injected");
        }
    }
}
