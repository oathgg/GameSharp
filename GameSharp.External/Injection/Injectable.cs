using System;

namespace GameSharp.External.Injection
{
    public class Injectable
    {
        public string PathToAssemblyFile { get; }
        public string Entrypoint { get; }

        public Injectable(string pathToDll) => new Injectable(pathToDll, "");

        public Injectable(string pathToDll, string entrypoint)
        {
            if (string.IsNullOrEmpty(pathToDll))
                throw new ArgumentNullException("pathToDll");

            if (string.IsNullOrEmpty(entrypoint))
                throw new ArgumentNullException("entrypoint");

            PathToAssemblyFile = pathToDll;
            Entrypoint = entrypoint;
        }
    }
}
