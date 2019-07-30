# GameSharp

> TIP: The solution only works with VS 2019.


*For the record; there are a many libraries which are better than mine available for free on GitHub.
I'm just creating this codebase for myself to get a better understanding of architectures.*

This library changes a native (unmanaged) game application into a managed application.

### How to

See the Samples folder where you can see how I do things, I'm using notepad++ 32-bit for all my samples currently.
The library you are injecting needs an explicit architecture version, such as, if you're injecting into a 32 bit process then the DLL will have to be build with the x86 architecture.

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
