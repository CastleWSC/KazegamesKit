using System;
using System.Collections.Generic;

namespace KazegamesKit
{
    public static class TypeExtension 
    {

        public static bool IsSameOrSubClass(this Type a, Type b)
        {
            if(a != null && b != null)
            {
                return a.IsSubclassOf(b) || a == b;
            }

            return false;
        }

        public static bool IsSameOrSubClassOrImplementInterface(this Type a, Type b)
        {
            if(a != null && b != null)
            {
                bool result = b.IsAssignableFrom(a) || (new List<Type>(a.GetInterfaces())).Contains(b);
                if (!result)
                    result = IsSameOrSubClass(a, b);

                return result;
            }

            return false;
        }
    }
}
