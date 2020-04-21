using UnityEngine;

namespace KazegamesKit
{
    public class EnumTypeAttribute : PropertyAttribute
    {

        public string enumName;

        public EnumTypeAttribute()
        {

        }

        public EnumTypeAttribute(string name)
        {
            enumName = name;
        }
    }
}
