using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.External.Injection
{
    public class Injectable
    {
        public string PathToAssemblyFile { get; }
        public string Entrypoint { get; }

        public Injectable(string pathToDll) => new Injectable(pathToDll, "");

        public Injectable(string pathToDll, string entrypoint)
        {
            PathToAssemblyFile = pathToDll;
            Entrypoint = entrypoint;
        }
    }
}
