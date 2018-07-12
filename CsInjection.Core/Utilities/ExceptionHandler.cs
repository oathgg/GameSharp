using CsInjection.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsInjection.Core.Utilities
{
    public static class ExceptionHandler
    {
        public static void Initialize()
        {
            // Whenever an unhandled exception occurs we call our friendly exception handler method.
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            ExceptionHelper.BeautifyException(eventArgs.ExceptionObject);
            Console.ReadKey();
        }
    }
}
