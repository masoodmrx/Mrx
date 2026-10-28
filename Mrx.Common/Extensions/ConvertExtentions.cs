namespace Mrx.Common.Extensions
{
    public static class ConvertExtentions
    {
        #region Obsolete
        [Obsolete]
        public static bool TryConvertValue<T>(this string stringValue, out T convertedValue)
        {
            var targetType = typeof(T);
            if (targetType == typeof(string))
            {
                convertedValue = (T)Convert.ChangeType(stringValue, typeof(T));
                return true;
            }
            var nullableType = targetType.IsGenericType &&
                           targetType.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (nullableType)
            {
                if (string.IsNullOrEmpty(stringValue))
                {
                    convertedValue = default(T);
                    return true;
                }
                targetType = new NullableConverter(targetType).UnderlyingType;
            }

            Type[] argTypes = { typeof(string), targetType.MakeByRefType() };
            var tryParseMethodInfo = targetType.GetMethod("TryParse", argTypes);
            if (tryParseMethodInfo == null)
            {
                convertedValue = default(T);
                return false;
            }

            object[] args = { stringValue, null };
            var successfulParse = (bool)tryParseMethodInfo.Invoke(null, args);
            if (!successfulParse)
            {
                convertedValue = default(T);
                return false;
            }

            convertedValue = (T)args[1];
            return true;
        }
        [Obsolete]
        public static T? StructConvertValue<T>(this string stringValue) where T : struct => stringValue.TryConvertValue(out T val) ? val : (T?)null;
        [Obsolete]
        public static T ClassConvertValue<T>(this string stringValue) where T : class => stringValue.TryConvertValue(out T val) ? val : null;
        [Obsolete]
        public static char? ToChar(this string stringValue) => stringValue.StructConvertValue<char>();
        [Obsolete]
        public static bool? ToBool(this string stringValue) => stringValue.StructConvertValue<bool>();
        [Obsolete]
        public static byte? ToByte(this string stringValue) => stringValue.StructConvertValue<byte>();
        [Obsolete]
        public static short? ToShort(this string stringValue) => stringValue.StructConvertValue<short>();
        [Obsolete]
        public static int? ToInt(this string stringValue) => stringValue.StructConvertValue<int>();
        [Obsolete]
        public static long? ToLong(this string stringValue) => stringValue.StructConvertValue<long>();
        [Obsolete]
        public static float? ToFloat(this string stringValue) => stringValue.StructConvertValue<float>();
        [Obsolete]
        public static double? ToDouble(this string stringValue) => stringValue.StructConvertValue<double>();
        [Obsolete]
        public static decimal? ToDecimal(this string stringValue) => stringValue.StructConvertValue<decimal>();
        [Obsolete]
        public static DateTime? ToDateTime(this string stringValue) => stringValue.StructConvertValue<DateTime>();
        [Obsolete]
        public static TimeSpan? ToTimeSpan(this string stringValue) => stringValue.StructConvertValue<TimeSpan>();
        #endregion

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
        public static string ToJson(this object obj) => JsonSerializer.Serialize(obj);
        public static string SerializeJson(this object obj) => JsonSerializer.Serialize(obj);
        public static T? DeserializeJson<T>(this string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch { }
            return default(T);
        }


        //public static dynamic ChangeType(this object obj, Type type)
        //{
        //    Type type2 = type.GetTypeNullableType();
        //    object val = obj == null ? null : Convert.ChangeType(obj, type2);
        //    return type.IsNullableType() ? Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type), val) : val;
        //}

        //=> obj == null ?  : Convert.ChangeType(obj, type);
    }
}
