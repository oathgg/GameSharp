using GameSharp.Utilities;
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Notepadpp.dll
{
    public class Entrypoint
    {
        [DllExport]
        public static void Main()
        {
            Console.WriteLine("I have been injected!");

            PatchRtlFindClearBits();
        }

        private static void PatchRtlFindClearBits()
        {
            ProcessModule module = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(x => x.ModuleName.ToUpper() == "NTDLL.DLL").FirstOrDefault();

            Patch patch = new Patch(module.BaseAddress + 0x1010, new byte[] { 0xC3 });

            patch.Enable();

            Console.WriteLine("RtlFindClearBits Patched!");
        }
    }
}
