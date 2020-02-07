using GameSharp.Internal.Enums;
using GameSharp.Core.Memory;
using GameSharp.Internal.Extensions;
using GameSharp.Internal.Memory;

namespace GameSharp.Internal.Functions
{
    public class Direct3DDevice9
    {
        public static MemoryPointer GetAddress(Direct3DDevice9FunctionOrdinals ordinal)
        {
            GameSharpProcess process = GameSharpProcess.Instance;
            IMemoryPointer d3d9dllAddress = process.Modules["d3d9.dll"].MemoryPointer;
            MemoryPointer ptr = d3d9dllAddress.GetVtableAddress((int) ordinal);

            return ptr;
        }
    }
}
