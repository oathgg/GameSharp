using CsInjection.Core.Utilities;
using RGiesecke.DllExport;
using System;
using System.Windows.Forms;

namespace WoWSharp
{
    public class Entrypoint
    {
        [DllExport]
        public static void Main()
        {
            ExceptionHandler.Initialize();

            Logger.Write("Injected");

            //throw new NullReferenceException();
        }
    }
}
