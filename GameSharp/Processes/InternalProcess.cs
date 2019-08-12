using GameSharp.Extensions;
using GameSharp.Memory.Internal;
using GameSharp.Memory.Module;
using GameSharp.Native;
using GameSharp.Native.Enums;
using GameSharp.Native.Structs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace GameSharp.Processes
{
    public sealed class InternalProcess : IProcess
    {
        private static InternalProcess _instance;

        public static InternalProcess Instance => _instance ?? (_instance = new InternalProcess());

        public Process Process { get; } = Process.GetCurrentProcess();

        public Module LoadLibrary(string libraryPath, bool resolveReferences = true)
        {
            if (!File.Exists(libraryPath))
                throw new FileNotFoundException(libraryPath);

            bool failed = resolveReferences
                ? Kernel32.LoadLibrary(libraryPath) == IntPtr.Zero
                : Kernel32.LoadLibraryExW(libraryPath, IntPtr.Zero, LoadLibraryFlags.DontResolveDllReferences) == IntPtr.Zero;

            if (failed)
                throw new Win32Exception($"Couldn't load the library {libraryPath}.");

            return GetModule(Path.GetFileName(libraryPath));
        }

        public Module GetModule(string moduleName) => Process.GetProcessModule(moduleName);

        public T CallFunction<T>(SafeFunction safeFunction, params object[] parameters) => safeFunction.Call<T>(parameters);

        public ThreadContext32 GetThreadContext()
        {
            ThreadContext32 context = new ThreadContext32
            {
                ContextFlags = (uint)Context.CONTEXT_CONTROL
            };

            uint threadId = 0;
            foreach (ProcessThread t in Process.Threads)
            {
                ThreadState state = t.ThreadState;

                if (state == ThreadState.Wait)
                    threadId = (uint)t.Id;
            }

            IntPtr hThread = Kernel32.OpenThread(ThreadAccess.GET_CONTEXT, false, threadId);

            if (hThread == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            Kernel32.SuspendThread(hThread);

            if (!Kernel32.GetThreadContext(hThread, ref context))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (!Kernel32.CloseHandle(hThread))
                throw new Win32Exception("Cannot close thread handle.");

            Kernel32.ResumeThread(hThread);

            return context;
        }
    }
}
