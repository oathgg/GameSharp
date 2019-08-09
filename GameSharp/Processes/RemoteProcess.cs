using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Processes
{
    public class RemoteProcess : IProcess
    {
        public Process Process { get; }

        public RemoteProcess(Process remoteProcess)
        {
            Process = remoteProcess;
        }

        public ProcessModule LoadLibrary(string libraryPath, bool resolveReferences = true)
        {
            throw new NotImplementedException();
        }

        public ProcessModule GetProcessModule(string moduleName)
        {
            throw new NotImplementedException();
        }
    }
}
