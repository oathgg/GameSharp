using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Core.Memory
{
    // http://blog.rewolf.pl/blog/wp-content/uploads/2013/03/PEB_Evolution.pdf
    public interface IMemoryPeb
    {
        bool BeingDebugged { get; set; }
        long NtGlobalFlag { get; set; }
    }
}
