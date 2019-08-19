using GameSharp.Core;

namespace GameSharp.External.Injection
{
    public interface IInjection
    {
        GameSharpProcess Process { get; }

        void InjectAndExecute(Injectable assembly, bool attach);
    }
}
