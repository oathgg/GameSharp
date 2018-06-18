using CsInjection.Core.Native;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Sandbox.Injectable
{
    public class Injectable
    {
        public static int Main(string s)
        {
            Kernel32.AllocConsole();

            Console.WriteLine("Hello world from C# injectable class");
            MessageBox.Show("Hello world from C# injectable class");

            Console.ReadKey();

            return 0;
        }
    }
}
