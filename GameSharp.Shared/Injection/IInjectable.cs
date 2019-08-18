namespace GameSharp.Core.Injection
{
    public interface IInjectable
    {
        string PathToAssemblyFile { get; }
        string Entrypoint { get; }
    }
}
