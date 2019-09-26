using System;

namespace GameSharp.Core.Extensions
{
    public static class ObjectExtension
    {
        public static T CastTo<T>(this object @object)
        {
            TypeCode typeCodeFrom = GetTypeCode(@object);
            TypeCode typeCodeTo = GetTypeCode<T>();
            T result = default;

            if (typeCodeFrom == typeCodeTo)
            {
                // hard casting it as the object types are the same anyway
                result = (T)@object;
            }
            else
            {
                switch (typeCodeFrom)
                {
                    case TypeCode.Byte:
                        byte[] castedObject = @object as byte[];
                        result = castedObject.CastTo<T>();
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return result;
        }

        private static TypeCode GetTypeCode(object @object)
        {
            Type realType = @object.GetType();

            TypeCode typeCode = Type.GetTypeCode(realType);

            // As we don't understand IntPtr in unmanaged code we do a check and set it to the appropriate type we DO understand.
            if (realType == typeof(IntPtr))
            {
                typeCode = IntPtr.Size == 4 ? TypeCode.Int32 : TypeCode.Int64;
            }

            return typeCode;
        }

        private static TypeCode GetTypeCode<T>()
        {
            Type realType = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(realType);

            // As we don't understand IntPtr in unmanaged code we do a check and set it to the appropriate type we DO understand.
            if (typeof(T) == typeof(IntPtr))
            {
                typeCode = IntPtr.Size == 4 ? TypeCode.Int32 : TypeCode.Int64;
            }

            return typeCode;
        }
    }
}
