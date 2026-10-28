namespace Mrx.Common.Extensions
{
    public static class ConvertExtentions
    {
        public static Char ToChar(this object obj) => Convert.ToChar(obj);
        public static Boolean ToBoolean(this object obj) => Convert.ToBoolean(obj);
        public static Byte ToByte(this object obj) => Convert.ToByte(obj);
        public static Int16 ToInt16(this object obj) => Convert.ToInt16(obj);
        public static Int32 ToInt32(this object obj) => Convert.ToInt32(obj);
        public static Int64 ToInt64(this object obj) => Convert.ToInt64(obj);
        public static Single ToSingle(this object obj) => Convert.ToSingle(obj);
        public static Double ToDouble(this object obj) => Convert.ToDouble(obj);
        public static Decimal ToDecimal(this object obj) => Convert.ToDecimal(obj);
        public static DateTime ToDateTime(this object obj) => Convert.ToDateTime(obj);
        public static TimeSpan ToTimeSpan(this object obj) => TimeSpan.Parse(obj.ToString()!);

        public static Char? ToCharNullable(this object obj) => TryCatchNull<Char?>(() => Convert.ToChar(obj));
        public static Boolean? ToBooleanNullable(this object obj) => TryCatchNull<Boolean?>(() => Convert.ToBoolean(obj));
        public static Byte? ToByteNullable(this object obj) => TryCatchNull<Byte?>(() => Convert.ToByte(obj));
        public static Int16? ToInt16Nullable(this object obj) => TryCatchNull<Int16?>(() => Convert.ToInt16(obj));
        public static Int32? ToInt32Nullable(this object obj) => TryCatchNull<Int32?>(() => Convert.ToInt32(obj));
        public static Int64? ToInt64Nullable(this object obj) => TryCatchNull<Int64?>(() => Convert.ToInt64(obj));
        public static Single? ToSingleNullable(this object obj) => TryCatchNull<Single?>(() => Convert.ToSingle(obj));
        public static Double? ToDoubleNullable(this object obj) => TryCatchNull<Double?>(() => Convert.ToDouble(obj));
        public static Decimal? ToDecimalNullable(this object obj) => TryCatchNull<Decimal?>(() => Convert.ToDecimal(obj));
        public static DateTime? ToDateTimeNullable(this object obj) => TryCatchNull<DateTime?>(() => Convert.ToDateTime(obj));
        public static TimeSpan? ToTimeSpanNullable(this object obj) => TryCatchNull<TimeSpan?>(() => TimeSpan.Parse(obj.ToString()!));
        private static T? TryCatchNull<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static string? ConvertToString(this object obj) => obj == null ? null : Convert.ToString(obj);

        //public static dynamic ChangeType(this object obj, Type type)
        //{
        //    Type type2 = type.GetTypeNullableType();
        //    object val = obj == null ? null : Convert.ChangeType(obj, type2);
        //    return type.IsNullableType() ? Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type), val) : val;
        //}

        //=> obj == null ?  : Convert.ChangeType(obj, type);
    }
}
