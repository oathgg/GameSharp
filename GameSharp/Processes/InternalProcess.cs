using GameSharp.Interoperability;
using GameSharp.Module;
using GameSharp.Native;
using GameSharp.Native.Enums;
using GameSharp.Native.Structs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace GameSharp.Processes
{
    public sealed class InternalProcess
    {
        private static InternalProcess _instance;

        public static InternalProcess Instance => _instance ?? (_instance = new InternalProcess());

        public Process Process { get; } = Process.GetCurrentProcess();

        public ProcessModuleCollection Modules => Process.Modules;

        public InternalModule LoadLibrary(string libraryPath, bool resolveReferences = true)
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

        public InternalModule GetModule(string moduleName)
        {
            int retryCount = 5;
            InternalModule module = null;
            do
            {
                // We do a refresh in case something has changed in the process, for example a DLL has been injected.
                Process.Refresh();

                module = new InternalModule(Process.Modules.Cast<ProcessModule>().SingleOrDefault(m => string.Equals(m.ModuleName, moduleName, StringComparison.OrdinalIgnoreCase)));

                if (module != null)
                    break;

                Thread.Sleep(1000);
            } while (retryCount-- > 0);

            return module;
        }

        public T CallFunction<T>(SafeFunction safeFunction, params object[] parameters) => safeFunction.Call<T>(parameters);

        public ThreadContext32 GetThreadContext()
        {
            ThreadContext32 context = new ThreadContext32
            {
                ContextFlags = (uint)Context.CONTEXT_CONTROL
            };

            IntPtr hThread = Kernel32.OpenThread(ThreadAccess.GET_CONTEXT, false, 0);

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
