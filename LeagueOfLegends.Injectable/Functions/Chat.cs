using Injectable.Helpers;
using System;
using System.Runtime.InteropServices;

namespace Injectable.Functions
{
    public class Chat
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, SetLastError = true)]
        private delegate IntPtr PrintChatDelegate(IntPtr pChatClient, string message, int type);

        public static void Print(string message)
        {
            Marshal.GetDelegateForFunctionPointer<PrintChatDelegate>(Offsets.PrintChat).DynamicInvoke
                (Offsets.ChatClient, message, 0);
        }
    }
}