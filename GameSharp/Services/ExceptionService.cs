using System;
using System.Diagnostics;

namespace GameSharp.Services
{
    public static class ExceptionService
    {
        public static void Initialize()
        {
            // Set the System.Diagnostics.Process.Exited event to be raised when the process terminates.
            Process.GetCurrentProcess().EnableRaisingEvents = true;
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            BeautifyException(eventArgs.ExceptionObject);

            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        private static void BeautifyException(object exception)
        {
            // Safe cast it as an exception otherwise we might cause a crash while crashing, lol.
            if (exception is Exception)
            {
                BeautifyException(exception as Exception);
            }
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