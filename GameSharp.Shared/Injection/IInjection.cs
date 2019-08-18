namespace GameSharp.Core.Injection
{
    public interface IInjection
    {
        IInjectable InjectableAssembly { get; set; }

        void Inject(IInjectable assembly);
        void ExecuteEntryPoint();
    }
}
