using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CsInjection.Core.Helpers
{
    public static class ConvertHelper
    {
        public static bool FromByteArray<T>(byte[] data, out T result)
        {
            object val = default(T);
            result = (T) val;

            TypeCode typeCode = Type.GetTypeCode(typeof(T));

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
                    val = data.First() > 0;
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
                    val = System.Text.Encoding.UTF8.GetString(data);
                    break;
                default:
                    return false;
            }

            result = (T) val;
            return true;
        }
    }
}
