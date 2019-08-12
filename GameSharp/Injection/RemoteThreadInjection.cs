using GameSharp.Native;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using GameSharp.Extensions;
using System.Runtime.InteropServices;
using System.ComponentModel;
using GameSharp.Processes;
using GameSharp.Memory;

namespace GameSharp.Injection
{
    public class RemoteThreadInjection : InjectionBase
    {
        private ExternalProcess RemoteProcess { get; set; }

        public RemoteThreadInjection(System.Diagnostics.Process process) : base(process)
        {
            RemoteProcess = new ExternalProcess(process);
        }

        protected override void Inject(string pathToDll) => RemoteProcess.LoadLibrary(pathToDll);

        protected override void Execute(string pathToDll, string entryPoint)
        {
            InternalProcess myProcess = InternalProcess.Instance;
            ProcessModule module = myProcess.LoadLibrary(pathToDll, false);
            IntPtr entryPointAddress = module.GetProcAddress(entryPoint);

            if (entryPointAddress == IntPtr.Zero)
                throw new Win32Exception($"Couldn't find the entry point, system returned error code: {Marshal.GetLastWin32Error()}");

            // Invoke the entry point in the remote process
            Kernel32.CreateRemoteThread((IntPtr)RemoteProcess.Process.Handle, IntPtr.Zero, 0, entryPointAddress, IntPtr.Zero, 0, IntPtr.Zero);
        }
    }
}
