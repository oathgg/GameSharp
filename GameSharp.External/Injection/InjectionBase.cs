using GameSharp.Core;
using System;
using System.Diagnostics;
using System.IO;

namespace GameSharp.External.Injection
{
    public abstract class InjectionBase : IInjection
    {
        public GameSharpProcess Process { get; }

        public InjectionBase(GameSharpProcess process)
        {
            Process = process ?? throw new NullReferenceException();
        }

        public void InjectAndExecute(Injectable assembly, bool attach)
        {
            UpdateFiles(assembly.PathToAssemblyFile);

            PreExecution(assembly);

            Process.RefreshModules();

            Process.AllocConsole();

            // In case we want to attach then we have to do so BEFORE we execute to give full debugging capabilities.
            if (attach && Debugger.IsAttached)
            {
                Process.AttachDebugger();
            }

            Execute(assembly);
        }

        private void UpdateFiles(string pathToDll)
        {
            string processPath = Path.GetDirectoryName(Process.MainModule.FileName);
            CopyFile(pathToDll, "GameSharp.Core.dll", processPath);
            CopyFile(pathToDll, "GameSharp.Internal.dll", processPath);
        }

        private void CopyFile(string pathToDll, string dllName, string destination)
        {
            string source = Path.Combine(Path.GetDirectoryName(pathToDll), dllName);
            string destinationFullName = Path.Combine(destination, dllName);

            File.Copy(source, destinationFullName, true);
        }

        /// <summary>
        ///     DLL needs to be the same platform as the target process (e.g. x64 or x86).
        /// </summary>
        /// <param name="pathToDll"></param>
        /// <param name="entryPoint"></param>
        protected abstract void PreExecution(Injectable assembly);

        /// <summary>
        ///     Execution after the DLL has been injected.
        /// </summary>
        /// <param name="pathToDll"></param>
        /// <param name="entryPoint"></param>
        protected abstract void Execute(Injectable assembly);
    }
}
