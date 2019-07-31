using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Utilities
{
    public static class Logger
    {
        public static void Info(string message) => Write(message, ConsoleColor.White);
        public static void Warning(string message) => Write(message, ConsoleColor.Yellow);
        public static void Error(string message) => Write(message, ConsoleColor.Red);
        public static void Verbose(string message) => Write(message, ConsoleColor.Cyan);

        private static void Write(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[GameSharp] :: {message}");
        }
    }
}
