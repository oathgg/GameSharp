namespace GameSharp.Injection
{
    public class InjectableAssembly
    {
        public string PathToAssemblyFile { get; }
        public string Entrypoint { get; }

        public InjectableAssembly(string pathToDll) => new InjectableAssembly(pathToDll, "");

        public InjectableAssembly(string pathToDll, string entrypoint)
        {
            PathToAssemblyFile = pathToDll;
            Entrypoint = entrypoint;
        }
    }
}
