// Part of the credits go to Lolp1 for giving the idea how to finish.
// https://github.com/lolp1/Process.NET/blob/master/src/Process.NET/Applied/Detours/Detour.cs
//

using GameSharp.Core.Memory;
using GameSharp.Core.Services;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Module;
using System;
using System.Runtime.InteropServices;

namespace GameSharp.Internal.Memory
{
    public abstract class Hook
    {
        /// <summary>
        ///     This var is not used within the detour itself. It is only here
        ///     to keep a reference, to avoid the GC from collecting the delegate instance!
        /// </summary>
        private readonly Delegate HookDelegate;

        /// <summary>
        ///     This variable contains the jump from the module where the hookable function resides into our module.
        ///     Bypass for an anti-cheat which is validating the return address of a function to reside in it's own module.
        /// </summary>
        private MemoryPatch MemoryPatch { get; set; }

        private InternalMemoryPointer HookPtr { get; }

        private MemoryPatch HookPatch { get; set; }

        private InternalMemoryPointer TargetFuncPtr { get; }

        private Delegate TargetDelegate { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Hook" /> class.
        ///     
        ///     A hook can be used to create a detour from an original function into your own function.
        ///     You can then proceed to call the original function by using the method <see cref="CallOriginal" />.
        /// </summary>
        /// <param name="target">The target delegate we want to detour.</param>
        /// <param name="hook">The hook delegate where want it to go.</param>
        public Hook(bool useAntiCheatHook = true)
        {
            try
            {
                TargetDelegate = GetHookDelegate();
                HookDelegate = GetDetourDelegate();

                TargetFuncPtr = TargetDelegate.ToFunctionPtr();
                HookPtr = HookDelegate.ToFunctionPtr();

                byte[] function = HookPtr.CreateFunctionCall();
                MemoryPointer codeCavePtr = useAntiCheatHook ? CreateAntiCheatCodeCave(function) : AllocateMemory(function);
                FillMemoryPointer(codeCavePtr, function);
            }
            catch (Exception ex)
            {
                LoggingService.Error($"Hook {ToString()}, could not be initialized: {ex.Message}");
            }
        }

        private MemoryPointer CreateAntiCheatCodeCave(byte[] function)
        {
            LoggingService.Info($"Searching anti-cheat code cave for hook {TargetDelegate}");

            InternalModulePointer module = TargetFuncPtr.GetMyModule();

            if (module == null)
            {
                throw new NullReferenceException("Cannot find a module which belongs to the specified pointer.");
            }

            MemoryPointer codeCavePtr = module.FindCodeCaveInModule((uint)function.Length);

            LoggingService.Info($"Found codecave at 0x{codeCavePtr.ToString()}");

            return codeCavePtr;
        }

        private MemoryPointer AllocateMemory(byte[] function)
        {
            LoggingService.Info($"Allocating memory for hook {TargetDelegate}");

            MemoryPointer allocatedMemory = GameSharpProcess.Instance.AllocateManagedMemory(function.Length);

            LoggingService.Info($"Allocated memory at {allocatedMemory}");

            return allocatedMemory;
        }

        private void FillMemoryPointer(MemoryPointer memoryPointer, byte[] function)
        {
            MemoryPatch = new MemoryPatch(memoryPointer, function);

            byte[] retToMemoryPtr = MemoryPatch.PatchAddress.CreateFunctionCall();

            HookPatch = new MemoryPatch(TargetFuncPtr, retToMemoryPtr);
        }

        public void Disable()
        {
            MemoryPatch.Disable();
            HookPatch.Disable();
        }

        public void Enable()
        {
            MemoryPatch.Enable();
            HookPatch.Enable();
        }

        public void CallOriginal(params object[] args)
        {
            Disable();
            TargetDelegate.DynamicInvoke(args);
            Enable();
        }

        /// <summary>
        ///     Source code found on
        ///     https://github.com/lolp1/Process.NET/blob/master/src/Process.NET/Applied/Detours/Detour.cs#L215
        ///     
        ///     Calls the original function, and returns a return value.
        /// </summary>
        /// <param name="args">
        ///     The arguments to pass. You MUST pass 'null' if it is a 'void' argument list.
        /// </param>
        /// <returns>An object containing the original functions return value.</returns>
        public T CallOriginal<T>(params object[] args)
        {
            Disable();

            object ret = TargetDelegate.DynamicInvoke(args);

            Enable();

            return (T)ret;
        }

        /// <summary>
        ///     This should return an UnmanagedFunctionPointer delegate.
        ///
        ///     e.g. Marshal.GetDelegateForFunctionPointer<DELEGATE>(ADDRESS);
        /// </summary>
        /// <returns></returns>
        public abstract Delegate GetHookDelegate();

        /// <summary>
        ///     This should return the delegate to the hook with the function where it needs to go.
        ///
        ///     e.g. return new OnAfkDelegate(DetourMethod);
        /// </summary>
        public abstract Delegate GetDetourDelegate();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Defaults to FastCall as this is the x64 architecture standard routine</returns>
        protected virtual CallingConvention GetCallingConvention()
        {
            return CallingConvention.FastCall;
        }
    }
}