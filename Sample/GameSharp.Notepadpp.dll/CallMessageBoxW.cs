using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Notepadpp.dll
{
    public class CallMessageBoxW
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int HookMessageBoxWDelegate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]string text, [MarshalAs(UnmanagedType.LPWStr)]string caption, uint type);

        public void Run()
        {
            ProcessModule module = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(x => x.ModuleName.ToUpper() == "USER32.DLL").FirstOrDefault();

            HookMessageBoxWDelegate function = Marshal.GetDelegateForFunctionPointer<HookMessageBoxWDelegate>(module.BaseAddress + 0x78290);

            function.DynamicInvoke(IntPtr.Zero, "This is a sample of how to Call a function", "Title of the Messagebox", (uint) 0);
        }
    }
}
