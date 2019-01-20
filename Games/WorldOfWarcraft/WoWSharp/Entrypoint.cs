using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WoWSharp
{
    public class Entrypoint
    {
        [DllExport]
        public static void Main()
        {
            MessageBox.Show("Hello World");
            Console.WriteLine("Hello world");
        }
    }
}
