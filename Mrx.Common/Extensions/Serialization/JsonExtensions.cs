namespace Mrx.Common.Extensions;

/// <summary>
/// JSON serialization extension methods.
/// </summary>
public static class JsonExtensions
{
    public static string ToJson(this object obj) => JsonSerializer.Serialize(obj);

    public static string SerializeJson(this object obj) => JsonSerializer.Serialize(obj);

    public static T? DeserializeJson<T>(this string json)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        catch
        {
            return default;
        }
    }
}
