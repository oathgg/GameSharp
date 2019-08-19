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

        public MemoryModule(ProcessModule processModule) : base(processModule)
        {
            MemoryAddress = new MemoryAddress(processModule.BaseAddress);
        }

        public override IMemoryAddress GetProcAddress(string name)
        {
            MemoryAddress address = new MemoryAddress(Kernel32.GetProcAddress(ProcessModule.BaseAddress, name));
            if (address == null)
            {
                throw new NullReferenceException($"Couldn't find function {name} in module {ProcessModule.ModuleName}");
            }
            return address;
        }
    }
}
