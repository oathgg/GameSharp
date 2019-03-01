using GameSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace GameSharp.Utilities
{
    public class Assembly
    {
        [SuppressUnmanagedCodeSecurity] // disable security checks for better performance
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] // cdecl - let caller (.NET CLR) clean the stack
        private delegate void AssemblyFunction();

        public static void Execute(byte[] asm)
        {
            // Create a codecave
            IntPtr allocatedMemory = Marshal.AllocHGlobal(asm.Length);

            // Write the ASM to the CodeCave
            allocatedMemory.Write(asm);

            // Generate a callable function with the PTR pointed to the codecave start address
            AssemblyFunction funcPtr = Marshal.GetDelegateForFunctionPointer<AssemblyFunction>(allocatedMemory);

            // Call the function
            funcPtr();

            // Free the codecave so we don't create a leak
            Marshal.FreeHGlobal(allocatedMemory);
        }
    }
}