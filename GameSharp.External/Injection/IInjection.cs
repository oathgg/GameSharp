using GameSharp.Core;
using GameSharp.External.Injection;

namespace GameSharp.External.Injection
{
    public interface IInjection
    {
        IProcess Process { get; }

        Injectable InjectableAssembly { get; set; }

        void InjectAndExecute(Injectable assembly, bool attach);
    }
}
