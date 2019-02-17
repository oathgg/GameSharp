# CsInjection
C# Injection, inject a managed dll into a native application.

Create a Class Library project and reference the nuget package UnmanagedExports.
https://www.nuget.org/packages/UnmanagedExports/1.2.7

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

namespace Injector
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathToDll = Path.Combine(Environment.CurrentDirectory, "Banished.Injectable.dll");
            InjectDll("Application-steam-x64", "Banished.Injectable.dll", "Main");
        }

        static void InjectDll(string processName, string pathToDll, string dllEntryPoint)
        {
            Process process = Process.GetProcessesByName(processName).FirstOrDefault();

            IInjection injector = new RemoteThreadInjection(process);

            injector.InjectAndExecute(pathToDll, dllEntryPoint);

            injector.AttachToProcess();
        }
    }
}
```
