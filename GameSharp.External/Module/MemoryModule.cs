using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.Core.Native.PInvoke;
using GameSharp.External.Memory;
using System;
using System.Diagnostics;

namespace GameSharp.External.Module
{
    public class MemoryModule : ModuleBase
    {
        public override IMemoryAddress MemoryAddress { get; }

        public GameSharpProcess GameSharpProcess { get; }

        public MemoryModule(GameSharpProcess process, ProcessModule processModule) : base(processModule)
        {
            GameSharpProcess = process;

            MemoryAddress = new MemoryAddress(GameSharpProcess, processModule.BaseAddress);
        }

        public override IMemoryAddress GetProcAddress(string name)
        {
            MemoryAddress address = new MemoryAddress(GameSharpProcess, Kernel32.GetProcAddress(NativeProcessModule.BaseAddress, name));
            if (address == null)
            {
                throw new NullReferenceException($"Couldn't find function {name} in module {NativeProcessModule.ModuleName}");
            }
            return address;
        }
    }
}
