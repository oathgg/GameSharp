using System;
using System.Linq;

namespace CsInjection.Core.Helpers
{
    public static class ConvertHelper
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
                // byte[] is an object.
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
                    result = Convert.ToDecimal(data);
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
