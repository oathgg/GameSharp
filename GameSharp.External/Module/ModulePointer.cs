using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Core.Native.PInvoke;
using GameSharp.External.Memory;
using System;
using System.Diagnostics;

namespace GameSharp.External.Module
{
    public class ModulePointer : ModulePointerBase
    {
        public override IMemoryPointer MemoryPointer { get; }

        public GameSharpProcess GameSharpProcess { get; }

        public ModulePointer(GameSharpProcess process, ProcessModule processModule) : base(processModule)
        {
            GameSharpProcess = process;
            MemoryPointer = new MemoryPointer(GameSharpProcess, processModule.BaseAddress);
        }

        public override IMemoryPointer GetProcAddress(string name)
        {
            MemoryPointer address = new MemoryPointer(GameSharpProcess, Kernel32.GetProcAddress(ProcessModule.BaseAddress, name));
            if (address == null)
            {
                throw new NullReferenceException($"Couldn't find function {name} in module {ProcessModule.ModuleName}");
            }
            return address;
        }
    }
}
