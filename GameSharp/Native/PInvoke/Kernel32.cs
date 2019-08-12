using GameSharp.Native.Enums;
using GameSharp.Native.Structs;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Native
{
    public static class Kernel32
    {
        internal const string kernel32 = "kernel32.dll";

        [DllImport(kernel32, SetLastError = true)]
        internal static extern bool VirtualProtect(IntPtr lpAddress, int dwSize, MemoryProtection flNewProtect, out MemoryProtection lpflOldProtect);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr baseAddress, string procName);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPWStr)]string lpFileName);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern IntPtr LoadLibraryExW([MarshalAs(UnmanagedType.LPWStr)]string lpFileName, IntPtr hReservedNull, LoadLibraryFlags dwFlags);

        [DllImport(kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FreeLibrary(IntPtr hModule);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr hObject);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern int ResumeThread(IntPtr hThread);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern int SuspendThread(IntPtr hThread);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern bool GetThreadContext(IntPtr hThread, ref ThreadContext32 lpContext);

        [DllImport(kernel32, SetLastError = true)]
        internal static extern bool VirtualQueryEx(IntPtr processHandle, IntPtr baseAddress, out MemoryBasicInformation buffer, int length);
    }
}