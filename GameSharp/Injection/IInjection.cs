namespace GameSharp.Injection
{
    public interface IInjection
    {
        void InjectAndExecute(InjectableAssembly assembly, bool attach);
    }
}
