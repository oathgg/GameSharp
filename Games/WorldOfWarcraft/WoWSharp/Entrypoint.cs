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
            MessageBox.Show("Hello World");
            Console.WriteLine("Hello world");

            // Fails if you don't have the DLL in the WOW folder installed.
            Log.Write("Hello world");
        }
    }
}
