using System;
using System.Runtime.InteropServices;
using System.Linq;

namespace CsInjection.Library.Helpers
{
    public static class Convert
    {
        public static T FromByteArray<T>(byte[] data)
        {
            object result = default(T);

            TypeCode typeCode = Type.GetTypeCode(typeof(T));
            switch (typeCode)
            {
                case TypeCode.Int32:
                    result = BitConverter.ToInt32(data, 0);
                    break;
                case TypeCode.Object:
                case TypeCode.Byte:
                    result = data;
                    break;
                case TypeCode.Boolean:
                    result = data.First() > 0;
                    break;
                case TypeCode.Char:
                    result = BitConverter.ToChar(data, 0);
                    break;
                case TypeCode.Int64:
                    result = BitConverter.ToInt64(data, 0);
                    break;
                case TypeCode.Single:
                    result = BitConverter.ToSingle(data, 0);
                    break;
                case TypeCode.Double:
                    result = BitConverter.ToDouble(data, 0);
                    break;
                case TypeCode.Decimal:
                    result = System.Convert.ToDecimal(data);
                    break;
                case TypeCode.String:
                    result = System.Text.Encoding.UTF8.GetString(data);
                    break;
                default:
                    Console.WriteLine($"Unparseable typecode: {typeCode}");
                    break;
            }

            return (T) result;
        }
    }
}
