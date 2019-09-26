using GameSharp.Core.Memory;
using System.Text;

namespace GameSharp.External.Injection
{
    public class ThreadHijackInjection : InjectionBase
    {
        public ThreadHijackInjection(GameSharpProcess process) : base(process)
        {
        }

        protected override void PreExecution(Injectable assembly)
        {
        }

        protected override void Execute(Injectable assembly)
        {
            IMemoryAddress allocatedMemory = Process.AllocateManagedMemory(assembly.PathToAssemblyFile.Length);

            allocatedMemory.Write(Encoding.Unicode.GetBytes(assembly.PathToAssemblyFile));
        }
    }
}
