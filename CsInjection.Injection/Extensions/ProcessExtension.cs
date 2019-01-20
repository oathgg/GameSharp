using EnvDTE;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace CsInjection.Injection.Extensions
{
    public static class ProcessExtension
    {
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
                        // Get the debug engine we want to use.
                        EnvDTE80.Engine debugEngine = proc.Transport.Engines.Item("Managed (v4.6, v4.5, v4.0)");
                        proc.Attach2(debugEngine);
                        break;
                    }
                    break;
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
    }
}