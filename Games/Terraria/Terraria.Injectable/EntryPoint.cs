using CsInjection.Core.Utilities;
using RGiesecke.DllExport;
using System;
using System.Diagnostics;

namespace Terraria.Injectable
{
    public class EntryPoint
    {
        /// <summary>
        ///     You have to set your platform target to either x86, ia64 or x64. AnyCPU assemblies cannot export functions. 
        /// </summary>
        [DllExport]
        public static void Main()
        {
            for (int i = 0; i < 1000; i++)
                Logger.Write("Hello World");

            Debugger.Break();
        }
    }
}
