using GameSharp.Notepadpp.FunctionWrapper;

namespace GameSharp.Notepadpp
{
    public static class Functions
    {
        public static IsDebuggerPresent IsDebuggerPresent = new IsDebuggerPresent();
        public static MessageBoxW SafeMessageBoxFunction = new MessageBoxW();
        public static NtQueryInformationProcess NtQueryInformationProcess = new NtQueryInformationProcess();
    }
}
