using GameSharp.Core;

namespace GameSharp.External.Injection
{
    public interface IInjection
    {
        IProcess Process { get; }

        Injectable InjectableAssembly { get; set; }

        void InjectAndExecute(Injectable assembly, bool attach);
    }
}
