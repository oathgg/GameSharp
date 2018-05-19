using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsInjection.Core.Hooks
{
    public interface IHook
    {
        /// <summary>
        ///     Will be used to install the <see cref="IHook"/> and enable it.
        /// </summary>
        void InstallHook();

        /// <summary>
        ///     Gets the name of the <see cref="IHook"/>
        /// </summary>
        string GetName();

        /// <summary>
        ///     This should return an UnmanagedFunctionPointer delegate.
        ///     
        ///     e.g. Marshal.GetDelegateForFunctionPointer<DELEGATE>(ADDRESS);
        /// </summary>
        /// <returns></returns>
        Delegate GetToHookDelegate();

        /// <summary>
        ///     This should return the delegate to the hook with the function where it needs to go.
        ///     
        ///     e.g. return new OnAfkDelegate(DetourMethod);
        ///     In this function you can call the <see cref="Detour.CallOriginal(object[])"/> of the <see cref="Detour"/> class
        ///     to also call the original function of the program.
        /// </summary>
        Delegate GetToDetourDelegate();
    }
}
