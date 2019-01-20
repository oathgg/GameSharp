using CsInjection.Core.Utilities;
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
            ExceptionHandler.Initialize();

            //Log.Write("Hello world");

            LayeredException();
        }

        private static void LayeredException()
        {
            throw new Exception("Managed exception");
        }
    }
}
