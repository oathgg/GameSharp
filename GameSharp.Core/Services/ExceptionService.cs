using System;
using System.Diagnostics;

namespace GameSharp.Core.Services
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

            LoggingService.Error("");
            LoggingService.Error("===================================================");
            LoggingService.Error("");

            if (exception != null)
            {
                do
                {
                    LoggingService.Error($"Type: {exception.GetType().FullName}");
                    LoggingService.Error($"Message: {exception.Message}");
                    if (!string.IsNullOrEmpty(exception.StackTrace))
                    {
                        LoggingService.Error("Stracktrace:");
                        LoggingService.Error(exception.StackTrace);
                    }
                    exception = exception.InnerException;
                } while (exception != null);
            }

            LoggingService.Error("");
            LoggingService.Error("===================================================");
            LoggingService.Error("");
        }
    }
}