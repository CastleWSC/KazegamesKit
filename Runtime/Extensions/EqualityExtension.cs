using System;

namespace KazegamesKit
{
    public static class EqualityExtension
    {
        public static bool IsNullable(this object obj)
        {
            if (obj == null) return true;
            System.Type type = obj.GetType();
            if (!type.IsValueType) return true; // ref-type
            if (System.Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>

            return false; // value-type
        }

        public static bool IsNull(this object obj)
        {
            return ReferenceEquals(obj, null);
        }

        public static bool SafeEquals(this object obj, object other)
        {
            if (IsNull(obj) && IsNull(other))
            {
                return true;
            }
            else if (!IsNull(obj) && !IsNull(other))
            {
                return ReferenceEquals(obj, other);
            }

            return false;
        }
    }
}
