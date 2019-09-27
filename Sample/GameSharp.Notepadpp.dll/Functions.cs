using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Notepadpp
{
    public static class Functions
    {
        public static IsDebuggerPresent IsDebuggerPresent = new IsDebuggerPresent();
        public static SafeCallMessageBoxW SafeMessageBoxFunction = new SafeCallMessageBoxW();
        public static NtQueryInformationProcess NtQueryInformationProcess = new NtQueryInformationProcess();
    }
}
