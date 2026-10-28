namespace Mrx.Common.Extensions
{
    public static class ReflectionExtentions
    {
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            foreach (string part in propertyName.Split('.'))
            {
                if (obj == null) { return null; }
                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }
                obj = info.GetValue(obj, null);
            }
            return obj;
        }
        //public static object GetPropertyValue(this object obj, string propertyName)
        //{
        //    return obj?.GetType().GetProperty(propertyName)?.GetValue(obj);
        //}
        public static object GetPropertyValue(this Type type, object obj, string propertyName)
        {
            return type?.GetProperty(propertyName)?.GetValue(obj);
        }
        public static bool IsEnum(this Type type)
        {
            return type.IsEnum || (type.Name == "Nullable`1" && type.GenericTypeArguments.Length == 1 && type.GenericTypeArguments[0].IsEnum);
        }
        public static bool IsNullableType<T>(this Type type)
        {
            return IsNullableType(type, typeof(T));
        }
        public static bool IsNullableType(this Type type)
        {
            return type.Name == "Nullable`1";
            //return Nullable.GetUnderlyingType(type) != null;
        }
        public static bool IsNullableType(this Type type, Type nullableType)
        {
            return type == nullableType || (type.Name == "Nullable`1" && type.GenericTypeArguments.Length == 1 && type.GenericTypeArguments[0] == nullableType);
        }
        public static Type GetTypeNullableType(this Type type)
        {
            if (type.IsNullableType())
            {
                return type.GenericTypeArguments[0];
            }

            return type;
        }
        public static bool AnyNullableTypes(this Type type, params Type[] nullableTypes)
        {
            return nullableTypes.Any(p => p == type.GetTypeNullableType());
        }
        public static bool AllNullableTypes(this Type type, params Type[] nullableTypes)
        {
            return nullableTypes.All(p => p == type.GetTypeNullableType());
        }
        public static bool IsNumber(this Type type)
        {
            return type.AnyNullableTypes(typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal));
        }
        public static string GetFullName(this Type type)
        {
            string name = type.Namespace + "." + (type.Name.Contains("`") ? type.Name.Remove(type.Name.IndexOf("`")) : type.Name);
            if (type.IsGenericType)
            {
                name += $"<{type.GetGenericArguments().Select(p => p.GetFullName()).StringJoin(",")}>";
            }

            return name;
        }
    }
}
