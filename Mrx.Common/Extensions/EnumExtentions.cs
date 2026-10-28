namespace Mrx.Common.Extensions
{
    public static class EnumExtentions
    {
        public static IEnumerable<T> GetValues<T>() where T : Enum => Enum.GetValues(typeof(T)).Cast<T>();
        public static IEnumerable<object> GetEnumValues(this Type enumType) => enumType.GetEnumValues().Cast<object>();
        public static TEnum? ToEnum<TEnum>(this string stringValue) where TEnum : struct => typeof(TEnum).IsEnum() && Enum.TryParse(stringValue, out TEnum @enum) ? @enum : (TEnum?)null;
    }
}
