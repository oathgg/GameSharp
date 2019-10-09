using GameSharp.Core.Memory;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.PInvoke;
using GameSharp.Core.Services;
using GameSharp.Internal;
using RGiesecke.DllExport;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace GameSharp.Notepadpp
{
    public class Entrypoint
    {
        private static readonly GameSharpProcess Process = GameSharpProcess.Instance;

        [DllExport]
        public static void Main()
        {
            LoggingService.Info("I have been injected!");

            LoggingService.Info("Calling MessageBoxW!");
            if (Functions.SafeMessageBoxFunction.Call<int>(IntPtr.Zero, "Through a SafeFunctionCall method", "Caption", (uint)0) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            LoggingService.Info("Enabling hook on MessageBoxW!");
            HookMessageBoxW messageBoxHook = new HookMessageBoxW();
            messageBoxHook.Enable();

            TestForDebugger();
        }

        private static void TestForDebugger()
        {
            while (true)
            {
                IsDebuggerPresent();
                InjectedIsDebuggerPresent();
                IsProcessDebugPort();
                IsProcessDebugObjectHandle();
                //IsProcessDebugFlag();

                Thread.Sleep(1000);
            }
        }

        private static readonly IMemoryAddress NtQueryResult = Process.AllocateManagedMemory(IntPtr.Size);
        private static readonly IMemoryAddress sizeRead = Process.AllocateManagedMemory(IntPtr.Size);

        /// <summary>
        /// This debug flag will still trigger even if you use something like ScyllaHide to hide your presence in the X64 Debugger.
        /// </summary>
        private static void InjectedIsDebuggerPresent()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugPort;
            uint result = new InjectedNtQueryInformationProcess().Call<uint>(Process.Handle, flag, NtQueryResult.Address, (uint)IntPtr.Size, sizeRead.Address);

            if (result == 0)
            {
                bool beingDebugged = (long)NtQueryResult.Read<IntPtr>() != 0;
                if (beingDebugged)
                {
                    LoggingService.Info($"InjectedIsDebuggerPresent() => Debugger found.");
                }
            }
            else
            {
                LoggingService.Error(
                    $"Couldn't query NtQueryInformationProcess, Error code: {Marshal.GetLastWin32Error().ToString("X")}, " +
                    $"Return value of NtQueryInformationProcess function is 0x{result.ToString("X")}.");
            }
        }

        #region Default WinAPI func
        private static void IsProcessDebugFlag()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugFlags;
            uint result = Ntdll.NtQueryInformationProcess(Process.Handle, flag, NtQueryResult.Address, sizeof(ulong), out _);

            // https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-erref/596a1078-e883-4972-9bbc-49e60bebca55
            if (result == 0)
            {
                bool beingDebugged = NtQueryResult.Read<ulong>() != 0;
                if (beingDebugged)
                {
                    LoggingService.Info($"{flag.ToString()} => Debugger found.");
                }
            }
            else
            {
                LoggingService.Error(
                    $"Flag: {flag.ToString()}" +
                    $", Couldn't query NtQueryInformationProcess, Error code: {Marshal.GetLastWin32Error().ToString("X")}" +
                    $", Return value of NtQueryInformationProcess function is 0x{result.ToString("X")}");
            }
        }

        private static void IsProcessDebugObjectHandle()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugObjectHandle;
            uint result = Ntdll.NtQueryInformationProcess(Process.Handle, flag, NtQueryResult.Address, (uint)IntPtr.Size, out _);

            if (result == 0xc0000353)
            {
                // We're not being debugged, status: STATUS_PORT_NOT_SET
            }
            else if (result == 0)
            {
                bool beingDebugged = NtQueryResult.Read<ulong>() != 0;
                if (beingDebugged)
                {
                    LoggingService.Info($"{flag.ToString()} => Debugger found.");
                }
            }
            else
            {
                LoggingService.Error(
                    $"Couldn't query NtQueryInformationProcess, Error code: {Marshal.GetLastWin32Error().ToString("X")}, " +
                    $"Return value of NtQueryInformationProcess function is 0x{result.ToString("X")}.");
            }
        }

        private static void IsProcessDebugPort()
        {
            ProcessInformationClass flag = ProcessInformationClass.ProcessDebugPort;
            uint result = Ntdll.NtQueryInformationProcess(Process.Handle, flag, NtQueryResult.Address, (uint) IntPtr.Size, out _);

            if (result == 0)
            {
                bool beingDebugged = (long)NtQueryResult.Read<IntPtr>() != 0;
                if (beingDebugged)
                {
                    LoggingService.Info($"{flag.ToString()} => Debugger found.");
                }
            }
            else
            {
                LoggingService.Error(
                    $"Couldn't query NtQueryInformationProcess, Error code: {Marshal.GetLastWin32Error().ToString("X")}, " +
                    $"Return value of NtQueryInformationProcess function is 0x{result.ToString("X")}.");
            }
        }

        private static void IsDebuggerPresent()
        {
            if (Functions.IsDebuggerPresent.Call<bool>(null))
            {
                LoggingService.Info("IsDebuggerPresent() => We're being debugged!");
            }
        }
        #endregion
    }
}
