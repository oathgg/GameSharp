using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsInjection.Interfaces
{
    interface IInjection
    {
        void Inject(string pathToDll, string entryPoint);
    }
}
