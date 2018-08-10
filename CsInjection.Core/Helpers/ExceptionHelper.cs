using System;

namespace CsInjection.Core.Helpers
{
    public static class ExceptionHelper
    {
        /// <summary>
        ///     Simple wrapper.
        /// </summary>
        /// <param name="exception"></param>
        public static void BeautifyException(object exception)
        {
            BeautifyException(exception as Exception);
        }

        /// <summary>
        ///     Elobuddy exception logger.
        /// </summary>
        /// <param name="ex"></param>
        public static void BeautifyException(Exception exception)
        {
            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("");
            Console.WriteLine(exception.Message);

            if (exception != null)
            {
                do
                {
                    Console.WriteLine("");
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