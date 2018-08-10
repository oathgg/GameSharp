using System;

namespace CsInjection.Core.Helpers
{
    public static class ConvertHelper
    {
        public static T FromByteArray<T>(byte[] data)
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
                    val = System.Text.Encoding.UTF8.GetString(data);
                    break;
            }

            return (T)val;
        }
    }
}