using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsInjection.Core.Utilities;

namespace Banished.Injectable
{
    public class Entrypoint
    {
        [DllExport]
        public static void Main()
        {
            ExceptionHandler.Initialize();
            Logger.Info("Injected");
        }
    }
}
