﻿using GameSharp.Core.Memory;
using GameSharp.Core.Native.Enums;
using GameSharp.Core.Services;
using GameSharp.Internal;
using GameSharp.Notepadpp.FunctionWrapper;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Notepadpp
{
    public static class Functions
    {
        public static IsDebuggerPresent IsDebuggerPresent = new IsDebuggerPresent();
        public static MessageBoxW MessageBoxW = new MessageBoxW();
        public static InjectedNtQueryInformationProcess InjectedNtQueryInformationProcess = new InjectedNtQueryInformationProcess();

        private static readonly NtQueryInformationProcess NtQueryInformationProcessWrapper = new NtQueryInformationProcess();
        public static T NtQueryInformationProcess<T>(GameSharpProcess process, ProcessInformationClass pic) where T : struct
        {
            T returnResult = default;

            IMemoryAddress returnPtr = process.AllocateManagedMemory(Marshal.SizeOf<T>());
            IMemoryAddress readBytes = process.AllocateManagedMemory(Marshal.SizeOf<T>());

            uint ntResult = NtQueryInformationProcessWrapper.Call<uint>(process.Handle, pic, returnPtr.Address, (uint) Marshal.SizeOf<T>(), readBytes.Address);

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
    }
}
