using GameSharp.Core.Memory;
using GameSharp.Core.Services;
using GameSharp.Internal;
using GameSharp.Internal.Memory;
using RGiesecke.DllExport;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace GameSharp.Notepadpp
{
    public class Entrypoint
    {
        static GameSharpProcess GameSharpProcess = GameSharpProcess.Instance;
        

        [DllExport]
        public static void Main()
        {
            LoggingService.Info("I have been injected!");

            LoggingService.Info("Calling MessageBoxW!");
            if (Functions.SafeMessageBoxFunction.Call<int>(IntPtr.Zero, "Through a SafeFunctionCall method", "Caption", (uint)0) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            //LoggingService.Info("Enabling hook on MessageBoxW!");
            //HookMessageBoxW messageBoxHook = new HookMessageBoxW();
            //messageBoxHook.Enable();

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
            if (Functions.IsDebuggerPresent.Call<bool>(null))
            {
                LoggingService.Info("IsDebuggerPresent() => We're being debugged!");
            }
        }

        // Works only for 64-bit... https://www.exploit-db.com/exploits/44463
        private static void NtQueryInformationProcess(int flag, string flagName)
        {
            using (IMemoryAddress result = GameSharpProcess.AllocateManagedMemory(IntPtr.Size))
            {
                if (Functions.NtQueryInformationProcess.Call<int>(GameSharpProcess.Handle, flag, result.Address, (uint)4, null) == 0)
                    LoggingService.Error($"Couldn't query NtQueryInformationProcess, Error code: {Marshal.GetLastWin32Error()}");

                LoggingService.Info($"{flagName} => Result {result.Read<int>().ToString("X")}");
            }
        }
    }
}
