# CsInjection
C# Injection, inject a managed dll into a native application.

Create a Class Library project and reference the nuget package UnmanagedExports.
https://www.nuget.org/packages/UnmanagedExports/1.2.7

Make sure you select an architecture which is the same as the process you're injecting to.
UnmanagedExports does not support 'AnyCPU'.

Create a new Entrypoint for your DLL.

```csharp
using RGiesecke.DllExport;
using System;

namespace Banished.Injectable
{
    public class Entrypoint
    {
        [DllExport]
        public static void Main()
        {
            Console.WriteLine("Hello world");
        }
    }
}
```

You can then inject the managed DLL into a native process with your favorite Injector or use the library I provide.
After injecting the DLL you can execute the entry point.

```csharp
using CsInjection.Core.Injection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CsInjection.Injector
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathToDll = Path.Combine(Environment.CurrentDirectory, "Banished.Injectable.dll");
            InjectDll("Application-steam-x64", pathToDll, "Main");
        }

        static void InjectDll(string processName, string pathToDll, string dllEntryPoint)
        {
            // The process we are injecting into.
            Process process = Process.GetProcessesByName(processName).FirstOrDefault();

            // A simple RemoteThreadInjector.
            IInjection injector = new RemoteThreadInjection(process);

            // Inject the DLL and executes the entrypoint.
            injector.InjectAndExecute(pathToDll, dllEntryPoint);

            // Attaches to the current process.
            injector.AttachToProcess();
        }
    }
}
```