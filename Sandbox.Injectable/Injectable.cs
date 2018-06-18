using CsInjection.Core.Native;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Sandbox.Injectable
{
    public class Injectable
    {
        public void Main()
        {
            Kernel32.AllocConsole();

            Console.WriteLine("Message");

            MessageBox.Show("Hello world from injectable class");

            Console.ReadKey();
        }
    }
}
