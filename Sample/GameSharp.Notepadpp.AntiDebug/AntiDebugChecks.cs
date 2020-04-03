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
                antiDebug.IsProcessDebugFlags();

                antiDebug.IsProcessDebugPort();
                antiDebug.IsProcessDebugObjectHandle();

                antiDebug.InjectedIsProcessDebugPort();
                antiDebug.WinApiIsProcessDebugPort();

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// This debug flag will still trigger even if you use something like ScyllaHide to hide your presence in the X64 Debugger.
        /// The reason is because ScyllaHide doesn't flip the NtGlobalFlag but instead nops part of the NtQueryInformationProcess method.
        /// And by creating our own NtQueryInformationProcess method we can do our validation through there where the opcodes are not nopped.
        /// </summary>
        private void InjectedIsProcessDebugPort()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugPort;
            bool check = Functions.InjectedNtQueryInformationProcess<IntPtr>(Process, flag) != IntPtr.Zero;
            LoggingService.Info($"{System.Reflection.MethodBase.GetCurrentMethod().Name}() => {check}.");
        }

        private void IsProcessDebugFlags()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugFlags;
            bool check = Functions.NtQueryInformationProcess<IntPtr>(Process, flag) != IntPtr.Zero;
            LoggingService.Info($"{System.Reflection.MethodBase.GetCurrentMethod().Name}() => {check}.");
        }

        private void IsProcessDebugObjectHandle()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugObjectHandle;
            bool check = Functions.NtQueryInformationProcess<IntPtr>(Process, flag) != IntPtr.Zero;
            LoggingService.Info($"{System.Reflection.MethodBase.GetCurrentMethod().Name}() => {check}.");
        }

        private void IsProcessDebugPort()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugPort;
            bool check = Functions.NtQueryInformationProcess<IntPtr>(Process, flag) != IntPtr.Zero;
            LoggingService.Info($"{System.Reflection.MethodBase.GetCurrentMethod().Name}() => {check}.");
        }

        private void WinApiIsProcessDebugPort()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugPort;
            bool check = Functions.WinApiNtQueryInformationProcess<IntPtr>(Process, flag) != IntPtr.Zero;
            LoggingService.Info($"{System.Reflection.MethodBase.GetCurrentMethod().Name}() => {check}.");
        }

        private void IsDebuggerPresent()
        {
            bool check = Functions.IsDebuggerPresent.Call();
            LoggingService.Info($"{System.Reflection.MethodBase.GetCurrentMethod().Name}() => {check}.");
        }
    }
}
