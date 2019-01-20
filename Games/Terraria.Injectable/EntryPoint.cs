using CsInjection.Core.Utilities;
using CsInjection.Injection.Helpers;
using RGiesecke.DllExport;
using System;

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
            //InjectionHelper.Initialize();

            Console.WriteLine("Hello world");
            Console.Read();
        }
    }
}
