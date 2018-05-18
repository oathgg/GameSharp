using System;
using System.Runtime.InteropServices;

namespace CsInjection.Library.Helpers
{
    public class Functions
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, SetLastError = true)]
        private delegate IntPtr PrintChatDelegate(IntPtr pChatClient, string message, int type);
        public static void PrintChat(string message)
        {
            PrintChatDelegate printChat = Marshal.GetDelegateForFunctionPointer<PrintChatDelegate>(Offsets.PrintChat);
            printChat(Offsets.ChatClient, message, 0);
        }
    }
}
