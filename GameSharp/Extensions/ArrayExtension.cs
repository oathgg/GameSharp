using System;
using System.Text;

namespace GameSharp.Extensions
{
    public static class ArrayExtension
    {
        public static string CastToString(this byte[] data, Encoding encoding)
        {
            return encoding.GetString(data);
        }

        /// <summary>
        ///     Converts the input byte(s) into the castable object by using the BitConverter class.
        ///     For a string you'll need to use the <c>CastToString</c> method.
        /// </summary>
        /// <typeparam name="T">Object type you want the byte(s) to be converted to, eg. int, boolean</typeparam>
        /// <param name="data">Byte(s) you want to convert</param>
        /// <returns></returns>
        public static T CastTo<T>(this byte[] data)
        {
            object val = default(T);

            Type realType = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(realType);

            // As we don't understand IntPtr in unmanaged code we do a check and set it to the appropriate type we DO understand.
            if (typeof(T) == typeof(IntPtr))
            {
                typeCode = IntPtr.Size == 4 ? TypeCode.Int32 : TypeCode.Int64;
            }

            // When we want to cast it to type byte array then we can just return the array.
            if (typeof(T) == typeof(byte[]))
            {
                typeCode = TypeCode.Byte;
            }

            // Most of these types can be converted to an int, see the BP of microsoft:
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-byte-array-to-an-int
            switch (typeCode)
            {
                case TypeCode.Int32:
                    val = BitConverter.ToInt32(data, 0);
                    break;

                case TypeCode.Byte:
                    val = data;
                    break;

                case TypeCode.Boolean:
                    val = data[0] > 0;
                    break;

                case TypeCode.Char:
                    val = BitConverter.ToChar(data, 0);
                    break;

                case TypeCode.Int64:
                    val = BitConverter.ToInt64(data, 0);
                    break;

                case TypeCode.Single:
                    val = BitConverter.ToSingle(data, 0);
                    break;

                case TypeCode.Double:
                    val = BitConverter.ToDouble(data, 0);
                    break;

                case TypeCode.Decimal:
                    val = Convert.ToDecimal(data);
                    break;

                case TypeCode.String:
                    throw new NotSupportedException("Please use the CastToString(Encoding) function.");
            }

            return (T)val;
        }
    }
}