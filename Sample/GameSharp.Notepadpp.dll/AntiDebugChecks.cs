using GameSharp.Core.Native.Enums;
using GameSharp.Core.Services;
using GameSharp.Internal;
using System;
using System.Threading;

namespace GameSharp.Notepadpp
{
    public class AntiDebugChecks
    {
        private static GameSharpProcess Process { get; } = GameSharpProcess.Instance;

        public static void CheckForDebugger()
        {
            AntiDebugChecks antiDebug = new AntiDebugChecks();

            while (true)
            {
                Console.Clear();

                antiDebug.IsDebuggerPresent();
                antiDebug.IsProcessDebugPort();
                antiDebug.IsProcessDebugObjectHandle();
                antiDebug.IsProcessDebugFlags();

                antiDebug.InjectedIsProcessDebugPort();
                antiDebug.WinApiIsProcessDebugPort();

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// This debug flag will still trigger even if you use something like ScyllaHide to hide your presence in the X64 Debugger.
        /// The reason is because ScyllaHide doesn't flip the NtGlobalFlag but instead nops part of the NtQueryInformationProcess method.
        /// </summary>
        private void InjectedIsProcessDebugPort()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugPort;
            if (Functions.InjectedNtQueryInformationProcess<IntPtr>(Process, flag) != IntPtr.Zero)
            {
                LoggingService.Info($"{System.Reflection.MethodBase.GetCurrentMethod().Name} => Debugger found.");
            }
        }

        private void IsProcessDebugFlags()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugFlags;
            if (Functions.NtQueryInformationProcess<IntPtr>(Process, flag) != IntPtr.Zero)
            {
                LoggingService.Info($"{System.Reflection.MethodBase.GetCurrentMethod().Name} => Debugger found.");
            }
        }

        private void IsProcessDebugObjectHandle()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugObjectHandle;
            if (Functions.NtQueryInformationProcess<IntPtr>(Process, flag) != IntPtr.Zero)
            {
                LoggingService.Info($"{System.Reflection.MethodBase.GetCurrentMethod().Name} => Debugger found.");
            }
        }

        private void IsProcessDebugPort()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugPort;
            if (Functions.NtQueryInformationProcess<IntPtr>(Process, flag) != IntPtr.Zero)
            {
                LoggingService.Info($"{System.Reflection.MethodBase.GetCurrentMethod().Name} => Debugger found.");
            }
        }

        private void WinApiIsProcessDebugPort()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugPort;
            if (Functions.WinApiNtQueryInformationProcess<IntPtr>(Process, flag) != IntPtr.Zero)
            {
                LoggingService.Info($"{System.Reflection.MethodBase.GetCurrentMethod().Name} => Debugger found.");
            }
        }

        private void IsDebuggerPresent()
        {
            if (Functions.IsDebuggerPresent.Call())
            {
                LoggingService.Info("IsDebuggerPresent() => Debugger found.");
            }
        }
    }
}
