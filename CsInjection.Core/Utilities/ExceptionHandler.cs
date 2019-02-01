using System;
using System.Diagnostics;

namespace CsInjection.Core.Utilities
{
    public static class ExceptionHandler
    {
        public static void Initialize()
        {
            // We call our friendly ExceptionHandler method whenever an 
            //  unhandled exception occurs in the CLR which hasn't been handled properly.
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            BeautifyException(eventArgs.ExceptionObject);

            if (Debugger.IsAttached)
                Debugger.Break();
        }

        /// <summary>
        ///     Simple wrapper.
        /// </summary>
        /// <param name="exception"></param>
        private static void BeautifyException(object exception)
        {
            // Safe cast it as an exception
            if (exception is Exception)
                BeautifyException(exception as Exception);
        }

        /// <summary>
        ///     Elobuddy exception logger.
        /// </summary>
        /// <param name="ex"></param>
        private static void BeautifyException(Exception exception)
        {
            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("");

            if (exception != null)
            {
                do
                {
                    Console.WriteLine("Type: {0}", exception.GetType().FullName);
                    Console.WriteLine("Message: {0}", exception.Message);
                    if (!String.IsNullOrEmpty(exception.StackTrace))
                    {
                        Console.WriteLine("Stracktrace:");
                        Console.WriteLine(exception.StackTrace);
                    }
                    exception = exception.InnerException;
                } while (exception != null);
            }

            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("");
        }
    }
}