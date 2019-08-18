using GameSharp.Core.Memory;
using GameSharp.Core.Module;
using GameSharp.External.Memory;
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

    }
}
