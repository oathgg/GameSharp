using GameSharp.Extensions;
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
            Logger.Info("I have been injected!");

            new UnsafeCallMessageBox().Run();
            HookMessageBox();
            //PatchMessageBoxW();
            new SafeCallMessageBoxW().Call<int>(IntPtr.Zero, "This is a sample of how to Call a function", "Title of the Messagebox", (uint)0);
        }

        private static void HookMessageBox()
        {
            new HookMessageBoxW().Enable();

            Logger.Info("MessageBoxW Hooked!");
        }

        private static void PatchMessageBoxW()
        {
            ProcessModule module = Process.GetCurrentProcess().GetProcessModule("USER32.DLL");
            Patch patch = new Patch(module.BaseAddress + 0x78290, new byte[] { 0xC3 });
            patch.Enable();
            Logger.Info("MessageBoxW Patched!");
        }
    }
}
