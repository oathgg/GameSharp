﻿using GameSharp.Native;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using GameSharp.Extensions;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace GameSharp.Injection
{
    public class RemoteThreadInjection : InjectionBase
    {
        public RemoteThreadInjection(Process process) : base(process)
        {
        }

        protected override void Inject(string pathToDll)
        {
            if (string.IsNullOrWhiteSpace(pathToDll) || !File.Exists(pathToDll))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            byte[] pathBytes = Encoding.Unicode.GetBytes(pathToDll);

            // Allocates memory the size of the path to our dll in bytes in the remote process.
            IntPtr allocatedMemory = Kernel32.VirtualAllocEx(_process.Handle, IntPtr.Zero, (uint)pathBytes.Length,
                Enums.AllocationType.Reserve | Enums.AllocationType.Commit, Enums.MemoryProtection.ExecuteReadWrite);

            // Write the path to our dll in the newly allocated memory section of the process.
            if (Kernel32.WriteProcessMemory(_process.Handle, allocatedMemory, pathBytes, pathBytes.Length, out IntPtr a))
            {
                // Gets the base address of the Kernel32.Dll file
                IntPtr kernel32Module = Kernel32.GetModuleHandle("kernel32.dll");

                // Gets the address of the exported function 'LoadLibraryA' from the kernel32.dll file
                IntPtr loadLibraryAddress = Kernel32.GetProcAddress(kernel32Module, "LoadLibraryW");

                // Creates a remote thread in the process that will call the function loadlibrary which takes a memory pointer which contains the path to our dll.
                Kernel32.CreateRemoteThread(_process.Handle, IntPtr.Zero, 0, loadLibraryAddress, allocatedMemory, 0, IntPtr.Zero);
            }
        }

        protected override void Execute(string pathToDll, string entryPoint)
        {
            Process myProcess = Process.GetCurrentProcess();
            ProcessModule module = myProcess.LoadLibrary(pathToDll, false);
            IntPtr entryPointAddress = module.GetProcAddress(entryPoint);

            if (entryPointAddress == IntPtr.Zero)
                throw new Win32Exception($"Couldn't find the entry point, system returned error code: {Marshal.GetLastWin32Error()}");

            // Invoke the entry point in the remote process
            Kernel32.CreateRemoteThread(_process.Handle, IntPtr.Zero, 0, entryPointAddress, IntPtr.Zero, 0, IntPtr.Zero);
        }
    }
}
