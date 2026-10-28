namespace Mrx.AspNetCore.Extensions
{
    public static class SessionHelperExtensions
    {
        public static bool HasKey(this ISession session, string key)
        {
            return session.Keys.Any(p => p == key);
        }
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
    }
}
