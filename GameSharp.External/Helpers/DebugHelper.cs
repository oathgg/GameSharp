using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace GameSharp.External.Helpers
{
    internal class DebugHelper
    {
        private readonly List<MemoryPatch> MemoryPatches = new List<MemoryPatch>();
        private readonly GameSharpProcess Process;

        private DebugHelper(GameSharpProcess process)
        {
            Process = process;
        }

        public static void SafeAttach(GameSharpProcess process)
        {
            DebugHelper debugHelper = new DebugHelper(process);

            debugHelper.ValidateDbgBreakPoint();
            debugHelper.AttachManagedDebugger();
            debugHelper.HideDebugger();
            debugHelper.DisposeOfPatches();
        }

        /// <summary>
        /// Anti-anti-debugging methods are placed in here
        /// </summary>
        private void HideDebugger()
        {
            MemoryPeb peb = Process.PEB;

            if (peb.IsValid)
            {
                peb.BeingDebugged = false;
                peb.NtGlobalFlag = 0;
            }
        }

        private void ValidateDbgBreakPoint()
        {
            ModulePointer ntdll = Process.Modules["ntdll.dll"];

            IMemoryPointer dbgBreakPointPtr = ntdll.GetProcAddress("DbgBreakPoint");

            byte dbgBreakPointByte = dbgBreakPointPtr.Read<byte>();

            if (dbgBreakPointByte == 0xC3)
            {
                MemoryPatches.Add(new MemoryPatch(dbgBreakPointPtr, new byte[] { 0xCC }));
            }
        }

        private void AttachManagedDebugger()
        {
            EnvDTE.DTE dte;
            try
            {
                dte = (EnvDTE.DTE)Marshal.GetActiveObject("VisualStudio.DTE.16.0");
            }
            catch (COMException)
            {
                throw new Exception("Visual studio v2019 not found.");
            }

            int tryCount = 5;

            // using a do while because the object is sometimes null....
            do
            {
                Process.Native.WaitForInputIdle();

                try
                {
                    EnvDTE.Processes processes = dte.Debugger.LocalProcesses;
                    foreach (EnvDTE80.Process2 proc in processes.Cast<EnvDTE80.Process2>().Where(proc => proc.Name.IndexOf(Process.Native.ProcessName) != -1))
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

        /// <summary>
        /// Using our own memorypatch disposer otherwise we might dispose patches we don't want to if we call the base.
        /// </summary>
        private void DisposeOfPatches()
        {
            foreach (MemoryPatch p in MemoryPatches)
            {
                p.Dispose();
            }
        }
    }
}
