using System;
using System.ComponentModel;
using System.Reflection;

namespace ImmoGlobalAdmin.Helpers
{
    public static class EnumTools
    {
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        public static T GetEnumFromDescriptionAttribute<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var fieldInfo in type.GetFields())
            {
                var descriptionAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (descriptionAttribute != null)
                {
                    if (descriptionAttribute.Description != description) continue;
                    return (T)fieldInfo.GetValue(null);
                }
                if (fieldInfo.Name != description) continue;
                return (T)fieldInfo.GetValue(null);
            }
            return default(T);
        }
    }
}
