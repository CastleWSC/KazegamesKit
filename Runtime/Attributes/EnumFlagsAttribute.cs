using UnityEngine;

namespace KazegamesKit
{
    public class EnumFlagsAttribute : PropertyAttribute
    {

        public string enumName;

        public EnumFlagsAttribute()
        {

        }

        public EnumFlagsAttribute(string name)
        {
            enumName = name;
        }
    }
}
