namespace Mrx.Common.Extensions
{
    public static class AttributeExtensions
    {
        public static bool HasAttribute<TAttr>(this MemberInfo member) where TAttr : Attribute
        {
            return member.GetCustomAttribute<TAttr>() != null;
        }

        public static IEnumerable<TAttr> GetAttributes<TAttr>(this MemberInfo member) where TAttr : Attribute
        {
            return member.GetCustomAttributes<TAttr>();
        }

        public static TAttr GetAttribute<TAttr>(this MemberInfo member) where TAttr : Attribute
        {
            return member.GetCustomAttribute<TAttr>();
        }

        public static string GetDisplayName(this MemberInfo member)
        {
            return member.GetAttribute<DisplayAttribute>()?.Name;
        }

        public static bool HasAttribute<TAttr>(this Type type) where TAttr : Attribute
        {
            return type.GetCustomAttribute<TAttr>() != null;
        }

        public static IEnumerable<TAttr> GetAttributes<TAttr>(this Type type) where TAttr : Attribute
        {
            return type.GetCustomAttributes<TAttr>();
        }

        public static TAttr GetAttribute<TAttr>(this Type type) where TAttr : Attribute
        {
            return type.GetCustomAttribute<TAttr>();
        }

        public static string GetDisplayName(this Type type)
        {
            return type.GetAttribute<DisplayAttribute>()?.Name;
        }

        public static TAttr GetAttribute<TAttr>(this Enum enumerationValue) where TAttr : Attribute
        {
            if (enumerationValue == null) return null;
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.IsNotNullOrEmpty())
            {
                return memberInfo[0].GetCustomAttribute<TAttr>();
            }

            return null;
        }
        public static bool TryGetAttribute<TAttr>(this Enum enumerationValue, out TAttr attr) where TAttr : Attribute
        {
            attr = enumerationValue.GetAttribute<TAttr>();
            return attr != null;
        }
        public static string GetDescription(this Enum enumerationValue)
        {
            return enumerationValue.GetAttribute<DescriptionAttribute>()?.Description;
        }

        public static string GetDisplayName(this Enum enumerationValue)
        {
            if (enumerationValue.GetAttribute<DisplayAttribute>() is DisplayAttribute displayAttribute)
            {
                return displayAttribute?.Name;
            }

            if (enumerationValue.GetAttribute<DisplayNameAttribute>() is DisplayNameAttribute displayNameAttribute)
            {
                return displayNameAttribute?.DisplayName;
            }

            return null;
        }
        public static string GetEnumDisplayName(this object enumerationValue)
        {
            var method = typeof(AttributeExtensions).GetMethods().FirstOrDefault(p => p.Name == "GetDisplayName" && p.GetParameters().Any() && p.GetParameters()[0].ParameterType == typeof(Enum));
            return method.Invoke(null, new object[] { enumerationValue }).ToString();
        }
        public static string GetDataValue(this Enum enumerationValue)
        {
            return enumerationValue.GetAttribute<DataAttribute>()?.Value?.ToString();
        }
        public static T GetDataValue<T>(this Enum enumerationValue)
        {
            return (T)enumerationValue.GetAttribute<DataAttribute>()?.Value;
        }
    }
}
