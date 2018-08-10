using CsInjection.Core.Helpers;
using System;
using System.Diagnostics;

namespace CsInjection.Core.Utilities
{
    public static class ExceptionHandler
    {
        public static void Initialize()
        {
            // We call our friendly ExceptionHandler method whenever an unhandled exception occurs in the CLR.
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            ExceptionHelper.BeautifyException(eventArgs.ExceptionObject);
            if (Debugger.IsAttached)
                Debugger.Break();
        }
    }
}