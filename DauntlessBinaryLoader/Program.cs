using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DauntlessBinaryLoader
{
    class Program
    {
        const string PackageJson = @"https://cdn.playdauntless.com/Dauntless/Production/PatcherCode/package.json";
        public static int MyProperty { get; } = new Random().Next(0, 100);
        public static int MyProperty1 => new Random().Next(0, 100);

        static void Main(string[] args)
        {
            ConfigurationStorage.Get();
        }
       
    }
}
