namespace Mrx.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static T CastTo<T>(this object o)
        {
            return (T)o;
        }

        public static dynamic CastToReflected(this object o, Type type)
        {
            MethodInfo methodInfo = typeof(ObjectExtensions).GetMethod(nameof(CastTo), BindingFlags.Static | BindingFlags.Public);
            Type[] genericArguments = new[] { type };
            MethodInfo genericMethodInfo = methodInfo?.MakeGenericMethod(genericArguments);
            return genericMethodInfo?.Invoke(null, new[] { o });
        }
        //public static dynamic Value(this Enum @enum, Type type1)
        //{
        //    var enumType = type1.GetTypeNullableType();
        //    if (!enumType.IsEnum) return null;
        //    var type = Enum.GetUnderlyingType(enumType);
        //    var val = @enum == null ? null : Convert.ChangeType(@enum, type);
        //    var aa = type1.IsNullableType() && val == null ? Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type), null) : val;
        //    return type1.IsNullableType() && val == null ? Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type), null) : val;
        //}
        //public static dynamic Value(this object @enum, Type type1)
        //{
        //    var enumType = type1.GetTypeNullableType();
        //    var type = Enum.GetUnderlyingType(enumType);
        //    if (type == typeof(int))
        //    {
        //        return type1.IsNullableType() ? (int?)@enum : (int)@enum;
        //    }
        //    if (type == typeof(byte))
        //    {
        //        return type1.IsNullableType() ? (byte?)@enum : (byte)@enum;
        //    }
        //    return null;
        //    //if (!enumType.IsEnum) return null;
        //    //var type = Enum.GetUnderlyingType(enumType);
        //    //var val = @enum == null ? null : Convert.ChangeType(@enum, type);
        //    //var aa = type1.IsNullableType() && val == null ? Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type), null) : val;
        //    //return type1.IsNullableType() && val == null ? Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type), null) : val;
        //}
    }
}
