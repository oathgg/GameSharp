using GameSharp.Extensions;
using GameSharp.Memory;
using GameSharp.Memory.Internal;
using GameSharp.Native;
using GameSharp.Native.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using static GameSharp.Native.Structs;

namespace GameSharp.Processes
{
    public sealed class InternalProcess : IProcess
    {
        private static InternalProcess _instance;

        public static InternalProcess Instance => _instance ?? (_instance = new InternalProcess());

        public Process Process { get; } = Process.GetCurrentProcess();

        public ProcessModule LoadLibrary(string libraryPath, bool resolveReferences = true)
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

        public ProcessModule GetModule(string moduleName)
        {
            ProcessModule internalModule = Process.GetProcessModule(moduleName);

            return internalModule;
        }
        public T CallFunction<T>(SafeFunction safeFunction, params object[] parameters) => safeFunction.Call<T>(parameters);

        public ThreadContext32 GetThreadContext()
        {
            ThreadContext32 context = new ThreadContext32
            {
                ContextFlags = (uint) Context.CONTEXT_CONTROL
            };

            uint threadId = 0;
            foreach (ProcessThread t in Process.Threads)
            {
                var state = t.ThreadState;

                if (state == System.Diagnostics.ThreadState.Wait)
                    threadId = (uint) t.Id;
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
