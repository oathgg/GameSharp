# GameSharp

> TIP: The solution only works with VS 2019.


*For the record; there are a many libraries which are better than mine available for free on GitHub.
I'm just creating this codebase for myself to get a better understanding of architectures.*

This library changes a native (unmanaged) game application into a managed application.

### How to

Create a 'Class Library' project and reference the nuget package UnmanagedExports.
https://www.nuget.org/packages/UnmanagedExports/1.2.7

Make sure you select an architecture which is the same as the process you're injecting in to.

> TIP: UnmanagedExports does not support 'AnyCPU'. You will have to set the build architecture to the same architecture as the process you're tying to inject into.

Create an Entrypoint for your DLL.

```csharp
using GameSharp.Utilities;
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Notepadpp.dll
{
    public class Entrypoint
    {
        [DllExport]
        public static void Main()
        {
            Logger.Info("I have been injected!");

            new CallMessageBoxW().Run();
            HookMessageBox();
            //PatchMessageBoxW();
        }

        private static void HookMessageBox()
        {
            new HookMessageBoxW().Enable();

            Logger.Info("MessageBoxW Hooked!");
        }

        private static void PatchMessageBoxW()
        {
            ProcessModule module = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(x => x.ModuleName.ToUpper() == "USER32.DLL").FirstOrDefault();
            Patch patch = new Patch(module.BaseAddress + 0x78290, new byte[] { 0xC3 });
            patch.Enable();
            Logger.Info("MessageBoxW Patched!");
        }
    }
}
```

You can then inject the managed DLL into a native process with your favorite Injector or use the library I provide.
After injecting the DLL you can execute the entry point.

```csharp
using GameSharp.Injection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GameSharp.Notepadpp.Injector
{
    class Program
    {
        static void Main(string[] args)
        {
            // The process we are injecting into.
            Process process = Process.GetProcessesByName("notepad++").FirstOrDefault();

            if (process == null)
            {
                throw new Exception("Process not found.");
            }

            // A simple RemoteThreadInjector.
            IInjection injector = new RemoteThreadInjection(process);

            // Inject the DLL and executes the entrypoint.
            string pathToDll = Path.Combine(Environment.CurrentDirectory, "GameSharp.Notepadpp.dll.dll");
            injector.InjectAndExecute(pathToDll, "Main", attach: true);
        }
    }
}
```

### Add your own injection method

You always want to extend your injection method from the `GameSharp.Injection.InjectionBase` class.
You can add your own injection methods by overriding the `Inject` and `Execute` method.

### Anti-Cheat

Currently there are a lot of detection vectors which are still present and most likely you'll have to figure out how their anti-cheat is working to get the most out of this project.
However, I do try to keep some of the anti-cheat in mind, for example:
> The code creates a code cave inside the remote process before hooking a function.
> This bypass is for an anti-cheat which is validating the return address of a function to reside in it's own module.

### What the sample includes

- DLL injection of a managed DLL in an unmanaged application through the famous RemoteThread injection method.
- Randomizing the PE header.
- Attaching a managed debugger to the unmanaged remote process.
- Execution of the entry point of the injected DLL in the remote process.
- Execution of the MessageBoxW function with my own arguments in an Unsafe and 'Safe' way.
- Hooking the MessageBoxW function in a 'Safe' way.
- (Currently commented) Disabling the MessageBoxW function.
