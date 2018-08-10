using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CsInjection.Core.Helpers
{
    public class ThreadContextHelper
    {
        public enum ContextFlags : uint
        {
            CONTEXT_i386 = 0x10000,
            CONTEXT_i486 = 0x10000,
            CONTEXT_CONTROL = CONTEXT_i386 | 0x01,
            CONTEXT_INTEGER = CONTEXT_i386 | 0x02,
            CONTEXT_SEGMENTS = CONTEXT_i386 | 0x04,
            CONTEXT_FLOATING_POINT = CONTEXT_i386 | 0x08,
            CONTEXT_DEBUG_REGISTERS = CONTEXT_i386 | 0x10,
            CONTEXT_EXTENDED_REGISTERS = CONTEXT_i386 | 0x20,
            CONTEXT_FULL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS,
            CONTEXT_ALL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS | CONTEXT_FLOATING_POINT | CONTEXT_DEBUG_REGISTERS | CONTEXT_EXTENDED_REGISTERS
        }

        [Flags]
        public enum ThreadAccessFlags : int
        {
            TERMINATE = 0x0001,
            SUSPEND_RESUME = 0x0002,
            GET_CONTEXT = 0x0008,
            SET_CONTEXT = 0x0010,
            SET_INFORMATION = 0x0020,
            QUERY_INFORMATION = 0x0040,
            SET_THREAD_TOKEN = 0x0080,
            IMPERSONATE = 0x0100,
            DIRECT_IMPERSONATION = 0x0200
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FloatingSaveArea
        {
            public uint ControlWord;
            public uint StatusWord;
            public uint TagWord;
            public uint ErrorOffset;
            public uint ErrorSelector;
            public uint DataOffset;
            public uint DataSelector;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            public byte[] RegisterArea;

            public uint Cr0NpxState;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Context
        {
            public uint ContextFlags; //set this to an appropriate value

                                      // Retrieved by CONTEXT_DEBUG_REGISTERS
            public uint Dr0;

            public uint Dr1;
            public uint Dr2;
            public uint Dr3;
            public uint Dr6;
            public uint Dr7;

            // Retrieved by CONTEXT_FLOATING_POINT
            public FloatingSaveArea FloatSave;

            // Retrieved by CONTEXT_SEGMENTS
            public uint SegGs;

            public uint SegFs;
            public uint SegEs;
            public uint SegDs;

            // Retrieved by CONTEXT_INTEGER
            public uint Edi;

            public uint Esi;
            public uint Ebx;
            public uint Edx;
            public uint Ecx;
            public uint Eax;

            // Retrieved by CONTEXT_CONTROL
            public uint Ebp;

            public uint Eip;
            public uint SegCs;
            public uint EFlags;
            public uint Esp;
            public uint SegSs;

            // Retrieved by CONTEXT_EXTENDED_REGISTERS
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] ExtendedRegisters;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenThread(ThreadAccessFlags dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetThreadContext(IntPtr hThread, ref Context lpContext);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetThreadContext(IntPtr hThread, ref Context lpContext);

        public static Context GetThreadContext()
        {
            var curThread = Process.GetCurrentProcess().Threads[0]; // Get current thread

            Context Context = new Context();
            Context.ContextFlags = (uint)ContextFlags.CONTEXT_CONTROL;
            IntPtr hThread = OpenThread(ThreadAccessFlags.GET_CONTEXT, false, (uint)curThread.Id);

            if (hThread == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            SuspendThread(hThread);

            if (!GetThreadContext(hThread, ref Context))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!CloseHandle(hThread))
                throw new Win32Exception("Cannot close thread handle.");

            ResumeThread(hThread);

            return Context;
        }
    }
}