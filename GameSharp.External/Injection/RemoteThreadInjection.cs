using GameSharp.Core.Native.PInvoke;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace GameSharp.External.Injection
{
    public class RemoteThreadInjection : InjectionBase
    {
        public RemoteThreadInjection(GameSharpProcess process) : base(process)
        {
        }

        protected override void PreExecution(Injectable assembly)
        {
            Process.SuspendThreads(true);
            Process.LoadLibrary(assembly.PathToAssemblyFile);
        }

        protected override void Execute(Injectable assembly)
        {
            IntPtr dllBase = Kernel32.LoadLibraryExW(assembly.PathToAssemblyFile, IntPtr.Zero, Core.Native.Enums.LoadLibraryFlags.DontResolveDllReferences);
            if (dllBase == IntPtr.Zero)
            {
                throw new Win32Exception($"Couldn't load DLL, system returned error code: {Marshal.GetLastWin32Error()}");
            }

            IntPtr entryPointAddress = Kernel32.GetProcAddress(dllBase, assembly.Entrypoint);
            if (entryPointAddress == IntPtr.Zero)
            {
                throw new Win32Exception($"Couldn't find the entry point, system returned error code: {Marshal.GetLastWin32Error()}");
            }

            IntPtr thread = Kernel32.CreateRemoteThread(Process.NativeHandle, IntPtr.Zero, 0, entryPointAddress, IntPtr.Zero, 0, IntPtr.Zero);
            if (thread == IntPtr.Zero)
            {
                throw new Win32Exception($"Couldn't load DLL, system returned error code: {Marshal.GetLastWin32Error().ToString()}");
            }
        }

        protected override void PostExecution(Injectable assembly)
        {
            Process.SuspendThreads(false);
        }
    }
}
