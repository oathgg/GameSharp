using System;

namespace DauntlessBinaryLoader
{
    internal class Program
    {
        private const string PackageJson = @"https://cdn.playdauntless.com/Dauntless/Production/PatcherCode/package.json";
        public static int MyProperty { get; } = new Random().Next(0, 100);
        public static int MyProperty1 => new Random().Next(0, 100);

        private static void Main(string[] args)
        {
            ConfigurationStorage.Get();
        }
    }
}