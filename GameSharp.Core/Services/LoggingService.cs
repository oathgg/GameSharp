using System;
using System.Diagnostics;

namespace GameSharp.Core.Services
{
    public static class LoggingService
    {
        public static void Info(object obj)
        {
            Write(obj.ToString(), ConsoleColor.White);
        }

        public static void Warning(string message)
        {
            Write(message, ConsoleColor.Yellow);
        }

        public static void Error(string message)
        {
            Write(message, ConsoleColor.Red);
        }

        public static void Verbose(string message)
        {
            Write(message, ConsoleColor.Cyan);
        }

        public static void Debug(string message)
        {
            if (!Debugger.IsAttached)
            {
                return;
            }

            Write(message, ConsoleColor.Cyan);
            Write("Press any key to continue", ConsoleColor.Cyan);
            Console.ReadKey();
        }

        private static void Write(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[GameSharp] :: {message}");
        }
    }
}
