using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsInjection.Injectors.Injection
{
    public interface IInjection
    {
        void InjectAndExecute(string pathToDll, string entryPoint);
    }
}
