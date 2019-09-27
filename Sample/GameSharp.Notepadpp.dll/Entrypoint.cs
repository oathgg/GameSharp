using GameSharp.Core.Memory;
using GameSharp.Core.Services;
using GameSharp.Internal;
using GameSharp.Internal.Memory;
using RGiesecke.DllExport;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace GameSharp.Notepadpp
{
    public class Entrypoint
    {
        [DllExport]
        public static void Main()
        {
            LoggingService.Info("I have been injected!");

            LoggingService.Info("Calling MessageBoxW!");
            SafeCallMessageBoxW safeMessageBoxFunction = new SafeCallMessageBoxW();
            if (safeMessageBoxFunction.Call<int>(IntPtr.Zero, "Through a SafeFunctionCall method", "Caption", (uint)0) == 0)
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
            while(true)
            {
                IsDebuggerPresent();
                NtQueryInformationProcess(0x7, "DebugPort");
                NtQueryInformationProcess(0x1E, "DebugObject");
                NtQueryInformationProcess(0x1F, "DebugFlags");
                Thread.Sleep(1000);
            }
        }

        private static void IsDebuggerPresent()
        {
            IsDebuggerPresent isDebuggerPresent = new IsDebuggerPresent();

            if (isDebuggerPresent.Call<bool>(null))
            {
                LoggingService.Info("IsDebuggerPresent() => We're being debugged!");
            }
        }

        private static void NtQueryInformationProcess(int flag, string flagName)
        {
            NtQueryInformationProcess ntQueryInformationProcess = new NtQueryInformationProcess();

            using (IMemoryAddress result = GameSharpProcess.Instance.AllocateManagedMemory(IntPtr.Size))
            {
                int queryState = ntQueryInformationProcess.Call<int>(GameSharpProcess.Instance.Handle, flag, result.Address, (uint)4, null);
                // STATUS_SUCCESS = 0, so if API call was successful queryState should contain 0.
                if (queryState == 0)
                {
                    if (!result.Read<bool>())
                    {
                        LoggingService.Info($"{flagName} => We're being debugged!");
                    }
                }
            }
        }
    }
}
