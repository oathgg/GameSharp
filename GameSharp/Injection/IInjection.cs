namespace GameSharp.Injection
{
    public interface IInjection
    {
        void InjectAndExecute(string pathToDll, string entryPoint, bool attach);
    }
}
