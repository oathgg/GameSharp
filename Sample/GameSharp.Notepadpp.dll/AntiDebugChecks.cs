using GameSharp.Core.Memory;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.PInvoke;
using GameSharp.Core.Services;
using GameSharp.Internal;
using GameSharp.Notepadpp.FunctionWrapper;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace GameSharp.Notepadpp
{
    public class AntiDebugChecks
    {
        static GameSharpProcess Process { get; } = GameSharpProcess.Instance;
        readonly IMemoryAddress NtQueryResult = Process.AllocateManagedMemory(IntPtr.Size);
        readonly IMemoryAddress SizeRead = Process.AllocateManagedMemory(IntPtr.Size);

        public static void CheckForDebugger()
        {
            AntiDebugChecks antiDebug = new AntiDebugChecks();

            while (true)
            {
                Console.Clear();

                //antiDebug.IsDebuggerPresent();
                antiDebug.InjectedIsProcessDebugPort();
                antiDebug.IsProcessDebugPort();
                antiDebug.WinApiIsProcessDebugPort();
                //antiDebug.IsProcessDebugObjectHandle();
                //IsProcessDebugFlag();

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// This debug flag will still trigger even if you use something like ScyllaHide to hide your presence in the X64 Debugger.
        /// </summary>
        private void InjectedIsProcessDebugPort()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugPort;
            uint result = new InjectedNtQueryInformationProcess().Call<uint>(Process.Handle, flag, NtQueryResult.Address, (uint)IntPtr.Size, SizeRead.Address);

            if (result == 0)
            {
                bool beingDebugged = (long)NtQueryResult.Read<IntPtr>() != 0;
                if (beingDebugged)
                {
                    LoggingService.Info($"InjectedIsProcessDebugPort() => Debugger found.");
                }
            }
            else
            {
                LoggingService.Error(
                    $"Couldn't query NtQueryInformationProcess, Error code: {Marshal.GetLastWin32Error().ToString("X")}, " +
                    $"Return value of NtQueryInformationProcess function is 0x{result.ToString("X")}.");
            }
        }

        private void IsProcessDebugFlag()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugFlags;
            if (Functions.NtQueryInformationProcess<ulong>(Process, flag) != 0)
            {
                LoggingService.Info($"{flag.ToString()} => Debugger found.");
            }           
        }

        private void IsProcessDebugObjectHandle()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugObjectHandle;
            if (Functions.NtQueryInformationProcess<IntPtr>(Process, flag) != IntPtr.Zero)
            {
                LoggingService.Info($"{flag.ToString()} => Debugger found.");
            }
        }

        private void IsProcessDebugPort()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugPort;
            if (Functions.NtQueryInformationProcess<IntPtr>(Process, flag) != IntPtr.Zero)
            {
                LoggingService.Info($"{flag.ToString()} => Debugger found.");
            }
        }

        private void WinApiIsProcessDebugPort()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugPort;
            uint result = Ntdll.NtQueryInformationProcess(Process.Handle, flag, NtQueryResult.Address, (uint)IntPtr.Size, out _);

            if (result == 0)
            {
                bool beingDebugged = (long)NtQueryResult.Read<IntPtr>() != 0;
                if (beingDebugged)
                {
                    LoggingService.Info($"WinApi{flag.ToString()} => Debugger found.");
                }
            }
            else
            {
                LoggingService.Error(
                    $"Couldn't query NtQueryInformationProcess, Error code: {Marshal.GetLastWin32Error().ToString("X")}, " +
                    $"Return value of NtQueryInformationProcess function is 0x{result.ToString("X")}.");
            }
        }

        private void IsDebuggerPresent()
        {
            if (Functions.IsDebuggerPresent.Call<bool>(null))
            {
                LoggingService.Info("IsDebuggerPresent() => We're being debugged!");
            }
        }
    }
}
