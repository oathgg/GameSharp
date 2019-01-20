using CsInjection.Core.Utilities;
using RGiesecke.DllExport;
using System;
using System.Windows.Forms;

namespace WoWSharp
{
    /// <summary>
    ///     DLL needs to be the same platform as the game (e.g. x64 or x86).
    ///     If you're using other DLLs besides the windows platform default ones then you'll
    ///         need to transfer those to the directory of the executable.
    /// </summary>
    public class Entrypoint
    {

        [DllExport]
        public static void Main()
        {
            Log.Write("Injected.");
        }
    }
}
