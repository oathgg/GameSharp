using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace GameSharp.Extensions
{
    public static class ProcessExtension
    {
        public static void Attach(this Process process)
        {
            EnvDTE.DTE dte;
            try
            {
                dte = (EnvDTE.DTE)Marshal.GetActiveObject("VisualStudio.DTE.16.0");
            }
            catch (COMException)
            {
                Debug.WriteLine("Visual studio v2019 not found.");
                return;
            }

            int tryCount = 5;
            do
            {
                process.WaitForInputIdle();

                try
                {
                    EnvDTE.Processes processes = dte.Debugger.LocalProcesses;
                    foreach (EnvDTE80.Process2 proc in processes.Cast<EnvDTE80.Process2>().Where(proc => proc.Name.IndexOf(process.ProcessName) != -1))
                    {
                        EnvDTE80.Engine debugEngine = proc.Transport.Engines.Item("Managed (v4.6, v4.5, v4.0)");
                        proc.Attach2(debugEngine);
                        break;
                    }
                    break;
                }
                catch { }

                Thread.Sleep(1000);
            } while (tryCount-- > 0);
        }

        internal static ProcessModule GetProcessModule(this Process process, string moduleName)
        {
            int retryCount = 5;
            ProcessModule module = null;
            do
            {
                // We do a refresh in case something has changed in the process, for example a DLL has been injected.
                process.Refresh();

                module = process.Modules.Cast<ProcessModule>().SingleOrDefault(m => string.Equals(m.ModuleName, moduleName, StringComparison.OrdinalIgnoreCase));

                if (module != null)
                    break;

                Thread.Sleep(1000);
            } while (retryCount-- > 0);

            return module;
        }
    }
}