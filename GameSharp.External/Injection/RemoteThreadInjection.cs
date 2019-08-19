using GameSharp.Core.Native.PInvoke;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameSharp.External.Injection
{
    public class RemoteThreadInjection : InjectionBase
    {
        private GameSharpProcess ExternalProcess { get; }

        public RemoteThreadInjection(Process process) : base(process)
        {
            ExternalProcess = new GameSharpProcess(process);
        }

        protected override void Inject(string pathToDll) => ExternalProcess.LoadLibrary(pathToDll);

        protected override void Execute(string pathToDll, string entryPoint)
        {
            if (string.IsNullOrEmpty(pathToDll))
                throw new ArgumentNullException("pathToDll");

            if (string.IsNullOrEmpty(entryPoint))
                throw new ArgumentNullException("entryPoint");

            IntPtr dllBase = Kernel32.LoadLibraryExW(pathToDll, IntPtr.Zero, Core.Native.Enums.LoadLibraryFlags.DontResolveDllReferences);
            IntPtr entryPointAddress = Kernel32.GetProcAddress(dllBase, entryPoint);

            if (entryPointAddress == IntPtr.Zero)
            {
                throw new Win32Exception($"Couldn't find the entry point, system returned error code: {Marshal.GetLastWin32Error()}");
            }

            Kernel32.CreateRemoteThread(ExternalProcess.Process.Handle, IntPtr.Zero, 0, entryPointAddress, IntPtr.Zero, 0, IntPtr.Zero);
        }
    }
}
