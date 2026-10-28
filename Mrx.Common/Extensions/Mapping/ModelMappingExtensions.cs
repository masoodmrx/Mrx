using System.Globalization;

namespace Mrx.Common.Extensions;

/// <summary>
/// Maps named string values to writable properties on a model.
/// </summary>
public static class ModelMappingExtensions
{
    /// <summary>
    /// Creates a model and assigns matching values using case-insensitive property names.
    /// Unknown keys are ignored; if a key occurs more than once, its last value is used.
    /// Invalid values cause a <see cref="FormatException"/> that identifies the key.
    /// </summary>
    public static T MapToModel<T>(
        this IEnumerable<KeyValuePair<string, string?>> values,
        JsonSerializerOptions? jsonOptions = null)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(values);

        var model = new T();
        MapToModel(model!, typeof(T), values, jsonOptions);
        return model;
    }

    /// <summary>
    /// Assigns matching values to a supplied model using case-insensitive property names.
    /// Unknown keys are ignored; if a key occurs more than once, its last value is used.
    /// Invalid values cause a <see cref="FormatException"/> that identifies the key.
    /// </summary>
    public static T MapToModel<T>(
        this T model,
        IEnumerable<KeyValuePair<string, string?>> values,
        JsonSerializerOptions? jsonOptions = null)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(values);

        MapToModel(model, typeof(T), values, jsonOptions);
        return model;
    }

    private static void MapToModel(
        object model,
        Type modelType,
        IEnumerable<KeyValuePair<string, string?>> values,
        JsonSerializerOptions? jsonOptions)
    {
        var properties = modelType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.CanWrite && property.GetIndexParameters().Length == 0)
            .ToDictionary(property => property.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var (key, value) in values)
        {
            if (string.IsNullOrWhiteSpace(key) || !properties.TryGetValue(key, out var property))
            {
                continue;
            }

            try
            {
                property.SetValue(model, ConvertValue(value, property.PropertyType, jsonOptions));
            }
            catch (Exception exception)
            {
                throw new FormatException($"The value for '{key}' could not be converted to {property.PropertyType.Name}.", exception);
            }
        }
    }

    private static object? ConvertValue(string? value, Type destinationType, JsonSerializerOptions? jsonOptions)
    {
        var targetType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;
        var allowsNull = !destinationType.IsValueType || Nullable.GetUnderlyingType(destinationType) != null;

        if (value is null)
        {
            if (allowsNull)
            {
                return null;
            }

            throw new FormatException("A null value cannot be assigned to a non-nullable property.");
        }

        if (targetType == typeof(string))
        {
            return value;
        }

        if (string.IsNullOrWhiteSpace(value) && allowsNull)
        {
            return null;
        }

        if (targetType.IsEnum)
        {
            if (Enum.TryParse(targetType, value, ignoreCase: true, out var enumValue) &&
                (Enum.IsDefined(targetType, enumValue!) || targetType.IsDefined(typeof(FlagsAttribute), inherit: false)))
            {
                return enumValue;
            }

            throw new FormatException($"'{value}' is not a valid {targetType.Name} value.");
        }

        if (targetType == typeof(Guid))
        {
            return Guid.Parse(value);
        }

        if (targetType == typeof(DateOnly))
        {
            return DateOnly.Parse(value, CultureInfo.InvariantCulture);
        }

        if (targetType == typeof(TimeOnly))
        {
            return TimeOnly.Parse(value, CultureInfo.InvariantCulture);
        }

        if (targetType == typeof(TimeSpan))
        {
            return TimeSpan.Parse(value, CultureInfo.InvariantCulture);
        }

        if (targetType == typeof(DateTime))
        {
            return DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        if (targetType == typeof(DateTimeOffset))
        {
            return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        if (targetType == typeof(bool))
        {
            return bool.Parse(value);
        }

        if (targetType.IsPrimitive || targetType == typeof(decimal))
        {
            return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
        }

        try
        {
            return JsonSerializer.Deserialize(value, destinationType, jsonOptions);
        }
        catch (JsonException exception)
        {
            throw new FormatException($"'{value}' is not valid JSON for {targetType.Name}.", exception);
        }
    }
}
