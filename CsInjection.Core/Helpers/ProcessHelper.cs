using EnvDTE;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace CsInjection.Core.Helpers
{
    public static class ProcessHelper
    {
        public static ProcessModule GetMainModule = System.Diagnostics.Process.GetCurrentProcess().MainModule;
        public static IntPtr GetMainModuleBaseAddress = GetMainModule.BaseAddress;

        public static void Attach(this System.Diagnostics.Process process)
        {
            // Reference Visual Studio core
            DTE dte;
            try
            {
                dte = (DTE)Marshal.GetActiveObject("VisualStudio.DTE.15.0");
            }
            catch (COMException)
            {
                Debug.WriteLine(String.Format(@"Visual studio not found."));
                return;
            }

            // Try loop - Visual Studio may not respond the first time.
            int tryCount = 5;
            while (tryCount-- > 0)
            {
                try
                {
                    Processes processes = dte.Debugger.LocalProcesses;
                    foreach (EnvDTE80.Process2 proc in processes.Cast<EnvDTE80.Process2>().Where(proc => proc.Name.IndexOf(process.ProcessName) != -1))
                    {
                        EnvDTE100.Debugger5 dbg5 = (EnvDTE100.Debugger5)dte.Debugger;
                        EnvDTE80.Transport trans = dbg5.Transports.Item("Default");
                        EnvDTE80.Engine dbgeng = trans.Engines.Item("Managed (v4.6, v4.5, v4.0)");
                        var proc2 = (EnvDTE80.Process2)dbg5.GetProcesses(trans, ".").Item(process.ProcessName);
                        proc2.Attach2(dbgeng);
                        Debug.WriteLine(String.Format("Attached to process {0} successfully.", process.ProcessName));
                        break;
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
    }
}
