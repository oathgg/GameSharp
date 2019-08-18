using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Shared.Interfaces
{
    public interface IMemoryAddress
    {
        IntPtr BaseAddress { get; }

        T Read<T>(int size);
        T Read<T>() where T : struct;
        void Write();
    }
}
