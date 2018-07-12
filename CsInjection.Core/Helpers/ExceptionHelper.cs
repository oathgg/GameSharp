using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsInjection.Core.Helpers
{
    public static class ExceptionHelper
    {
        /// <summary>
        ///     Elobuddy exception logger.
        /// </summary>
        /// <param name="ex"></param>
        public static void BeautifyException(object ex)
        {
            var exception = ex as Exception;

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
