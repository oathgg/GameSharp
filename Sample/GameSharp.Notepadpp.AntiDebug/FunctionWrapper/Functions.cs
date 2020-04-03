using GameSharp.Core.Memory;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Native.PInvoke;
using GameSharp.Core.Services;
using GameSharp.Internal;
using GameSharp.Notepadpp.FunctionWrapper;
using System.Runtime.InteropServices;

namespace GameSharp.Notepadpp
{
    public static class Functions
    {
        public static IsDebuggerPresent IsDebuggerPresent = new IsDebuggerPresent();

        private static readonly InjectedNtQueryInformationProcess InjectedNtQueryInformationProcessWrapper = new InjectedNtQueryInformationProcess();
        public static T InjectedNtQueryInformationProcess<T>(GameSharpProcess process, ProcessInformationClass pic) where T : struct
        {
            T returnResult = default;

            uint ntResult = InjectedNtQueryInformationProcessWrapper.Call(process.NativeHandle, pic, out IMemoryPointer returnPtr, Marshal.SizeOf<T>(), out IMemoryPointer _);

            // https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-erref/596a1078-e883-4972-9bbc-49e60bebca55
            if (ntResult == 0)
            {
                returnResult = returnPtr.Read<T>();
            }
            else
            {
                LoggingService.Error(
                    $"Flag: {pic.ToString()}" +
                    $", Couldn't query NtQueryInformationProcess, Error code: {Marshal.GetLastWin32Error().ToString("X")}" +
                    $", Return value of NtQueryInformationProcess function is 0x{ntResult.ToString("X")}");
            }

            return returnResult;
        }

        private static readonly NtQueryInformationProcess NtQueryInformationProcessWrapper = new NtQueryInformationProcess();
        /// <summary>
        /// Wrapper for the NtQueryInformationProcess delegate, this will make the code more readable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="process"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static T NtQueryInformationProcess<T>(GameSharpProcess process, ProcessInformationClass pic) where T : struct
        {
            T returnResult = default;

            uint ntResult = NtQueryInformationProcessWrapper.Call(process.NativeHandle, pic, out IMemoryPointer returnPtr, Marshal.SizeOf<T>(), out IMemoryPointer _);

            // https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-erref/596a1078-e883-4972-9bbc-49e60bebca55
            if (ntResult == 0)
            {
                returnResult = returnPtr.Read<T>();
            }
            //else
            //{
            //    LoggingService.Error(
            //        $"Flag: {pic.ToString()}" +
            //        $", Couldn't query NtQueryInformationProcess, Error code: {Marshal.GetLastWin32Error().ToString("X")}" +
            //        $", Return value of NtQueryInformationProcess function is 0x{ntResult.ToString("X")}");
            //}

            return returnResult;
        }

        /// <summary>
        /// Wrapper for the defautl WinApi NtQueryInformationProcess, makes the code more readable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="process"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static T WinApiNtQueryInformationProcess<T>(GameSharpProcess process, ProcessInformationClass pic) where T : struct
        {
            T returnResult = default;

            IMemoryPointer ntResult = process.AllocateManagedMemory(Marshal.SizeOf<T>());

            uint result = Ntdll.NtQueryInformationProcess(process.NativeHandle, pic, ntResult.Address, Marshal.SizeOf<T>(), out int _);

            if (result == 0)
            {
                returnResult = ntResult.Read<T>();
            }
            else
            {
                LoggingService.Error(
                    $"Couldn't query NtQueryInformationProcess, Error code: {Marshal.GetLastWin32Error().ToString("X")}, " +
                    $"Return value of NtQueryInformationProcess function is 0x{result.ToString("X")}.");
            }

            return returnResult;
        }
    }
}
