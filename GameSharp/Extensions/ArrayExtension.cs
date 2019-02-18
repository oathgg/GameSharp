using System;
using System.Text;

namespace GameSharp.Extensions
{
    public static class ArrayExtension
    {
        public static string CastString(this byte[] data, Encoding encoding)
        {
            return encoding.GetString(data);
        }

        /// <summary>
        ///     Casts the data into the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Cast<T>(this byte[] data)
        {
            object val = default(T);
            Type realType = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(realType);

            // Additional type checking
            if (typeof(T) == typeof(IntPtr))
                typeCode = TypeCode.Int32;
            if (typeof(T) == typeof(byte[]))
                typeCode = TypeCode.Byte;

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
                    throw new NotImplementedException();
            }

            return (T)val;
        }
    }
}